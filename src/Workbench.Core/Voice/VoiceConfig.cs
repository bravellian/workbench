// Voice capture configuration loaded from environment variables.
// Invariants: defaults are safe for short recordings and transcription limits.
namespace Workbench.Core.Voice;

/// <summary>
/// Voice capture and transcription configuration.
/// </summary>
/// <param name="Format">Audio format for recording.</param>
/// <param name="MaxDuration">Maximum recording duration.</param>
/// <param name="ChunkDuration">Optional chunk duration for splitting recordings.</param>
/// <param name="MaxUploadBytes">Maximum upload size for transcription.</param>
/// <param name="TranscriptionModel">Transcription model identifier.</param>
/// <param name="TranscriptionLanguage">Optional language hint for transcription.</param>
/// <param name="TranscriptExcerptMaxChars">Max characters to store as transcript excerpt.</param>
public sealed record VoiceConfig(
    AudioFormat Format,
    TimeSpan MaxDuration,
    TimeSpan? ChunkDuration,
    long MaxUploadBytes,
    string TranscriptionModel,
    string? TranscriptionLanguage,
    int TranscriptExcerptMaxChars)
{
    /// <summary>
    /// Loads voice configuration from environment variables with defaults.
    /// </summary>
    public static VoiceConfig Load()
    {
        var sampleRate = 16000;
        var channels = 1;
        var bitsPerSample = 16;
        var maxDurationSeconds = ReadInt("WORKBENCH_VOICE_MAX_DURATION_SECONDS", 240);
        var chunkSeconds = ReadNullableInt("WORKBENCH_VOICE_CHUNK_SECONDS", 90);
        var maxBytes = ReadLong("WORKBENCH_VOICE_MAX_BYTES", 20 * 1024 * 1024);
        var excerptChars = ReadInt("WORKBENCH_VOICE_TRANSCRIPT_EXCERPT", 280);

        var model = Environment.GetEnvironmentVariable("WORKBENCH_AI_TRANSCRIPTION_MODEL")
            ?? Environment.GetEnvironmentVariable("WORKBENCH_AI_MODEL")
            ?? Environment.GetEnvironmentVariable("OPENAI_MODEL")
            ?? "gpt-4o-mini-transcribe";

        var language = Environment.GetEnvironmentVariable("WORKBENCH_AI_TRANSCRIPTION_LANGUAGE");

        return new VoiceConfig(
            new AudioFormat(sampleRate, channels, bitsPerSample),
            TimeSpan.FromSeconds(maxDurationSeconds),
            chunkSeconds.HasValue ? TimeSpan.FromSeconds(chunkSeconds.Value) : null,
            maxBytes,
            model,
            string.IsNullOrWhiteSpace(language) ? null : language,
            excerptChars);
    }

    private static int ReadInt(string key, int fallback)
    {
        var raw = Environment.GetEnvironmentVariable(key);
        return int.TryParse(raw, CultureInfo.InvariantCulture, out var parsed) && parsed > 0 ? parsed : fallback;
    }

    private static int? ReadNullableInt(string key, int fallback)
    {
        var raw = Environment.GetEnvironmentVariable(key);
        if (string.IsNullOrWhiteSpace(raw))
        {
            return fallback;
        }
        return int.TryParse(raw, CultureInfo.InvariantCulture, out var parsed) && parsed > 0 ? parsed : null;
    }

    private static long ReadLong(string key, long fallback)
    {
        var raw = Environment.GetEnvironmentVariable(key);
        return long.TryParse(raw, CultureInfo.InvariantCulture, out var parsed) && parsed > 0 ? parsed : fallback;
    }
}
