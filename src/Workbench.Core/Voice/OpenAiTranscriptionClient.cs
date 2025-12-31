using System.Net.Http.Headers;

namespace Workbench.Core.Voice;

public sealed class OpenAiTranscriptionClient : ITranscriptionClient
{
    private const string? HttpsApiOpenaiComV1AudioTranscriptions = "https://api.openai.com/v1/audio/transcriptions";
    private readonly HttpClient httpClient;
    private readonly string apiKey;
    private readonly string model;
    private readonly string? language;

    private OpenAiTranscriptionClient(HttpClient httpClient, string apiKey, string model, string? language)
    {
        this.httpClient = httpClient;
        this.apiKey = apiKey;
        this.model = model;
        this.language = language;
    }

    public static bool TryCreate(out OpenAiTranscriptionClient? client, out string? reason)
    {
        client = null;
        reason = null;

        var provider = Environment.GetEnvironmentVariable("WORKBENCH_AI_PROVIDER") ?? "openai";
        if (string.Equals(provider, "none", StringComparison.OrdinalIgnoreCase))
        {
            reason = "WORKBENCH_AI_PROVIDER=none";
            return false;
        }
        if (!string.Equals(provider, "openai", StringComparison.OrdinalIgnoreCase))
        {
            reason = $"Unsupported provider '{provider}'.";
            return false;
        }

        var localApiKey = Environment.GetEnvironmentVariable("WORKBENCH_AI_OPENAI_KEY")
            ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        if (string.IsNullOrWhiteSpace(localApiKey))
        {
            reason = "Missing OPENAI_API_KEY or WORKBENCH_AI_OPENAI_KEY.";
            return false;
        }

        var localModel = Environment.GetEnvironmentVariable("WORKBENCH_AI_TRANSCRIPTION_MODEL")
            ?? Environment.GetEnvironmentVariable("WORKBENCH_AI_MODEL")
            ?? Environment.GetEnvironmentVariable("OPENAI_MODEL")
            ?? "gpt-4o-mini-transcribe";

        var localLanguage = Environment.GetEnvironmentVariable("WORKBENCH_AI_TRANSCRIPTION_LANGUAGE");

        client = new OpenAiTranscriptionClient(new HttpClient(), localApiKey, localModel, string.IsNullOrWhiteSpace(localLanguage) ? null : localLanguage);
        return true;
    }

    public async Task<string> TranscribeAsync(string wavPath, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(wavPath))
        {
            throw new ArgumentException("WAV path is required.", nameof(wavPath));
        }

        var fileInfo = new FileInfo(wavPath);
        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException("WAV file not found.", wavPath);
        }

        var waveInfo = WaveFileInfo.Read(wavPath);
        VoiceLog.Debug($"Transcribing {wavPath} ({fileInfo.Length} bytes, {waveInfo.Duration:c}).");
        VoiceLog.Debug($"Transcription model: {this.model}.");

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(this.model), "model");
        if (!string.IsNullOrWhiteSpace(this.language))
        {
            content.Add(new StringContent(this.language), "language");
        }

        var fileStream = File.OpenRead(wavPath);
        await using (fileStream.ConfigureAwait(false))
        {
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
            content.Add(fileContent, "file", Path.GetFileName(wavPath));

            using var request =
                new HttpRequestMessage(HttpMethod.Post, HttpsApiOpenaiComV1AudioTranscriptions)
                {
                    Content = content
                };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.apiKey);

            using var response = await this.httpClient.SendAsync(request, ct).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                throw new InvalidOperationException(
                    $"OpenAI transcription failed ({(int)response.StatusCode}): {errorBody}");
            }

            if (response.Headers.TryGetValues("x-request-id", out var requestIds))
            {
                var requestId = requestIds.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(requestId))
                {
                    VoiceLog.Debug($"OpenAI request id: {requestId}.");
                }
            }

            var responseStream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
            await using (responseStream.ConfigureAwait(false))
            {
                using var doc = await JsonDocument.ParseAsync(responseStream, cancellationToken: ct)
                    .ConfigureAwait(false);
                if (!doc.RootElement.TryGetProperty("text", out var textElement))
                {
                    throw new InvalidOperationException("OpenAI transcription response missing text.");
                }

                return textElement.GetString() ?? string.Empty;
            }
        }
    }
}
