namespace Workbench.Core.Voice;

public sealed class VoiceTranscriptionService
{
    private readonly ITranscriptionClient client;
    private readonly VoiceConfig config;

    public VoiceTranscriptionService(ITranscriptionClient client, VoiceConfig config)
    {
        this.client = client;
        this.config = config;
    }

    public async Task<VoiceTranscriptionResult> TranscribeAsync(AudioRecordingResult recording, CancellationToken ct)
    {
        var paths = recording.WavPaths;
        var tempFiles = new List<string>();

        if (paths.Count == 1 && this.config.ChunkDuration.HasValue)
        {
            var split = await WaveFileSplitter.SplitAsync(paths[0], this.config.ChunkDuration.Value, ct).ConfigureAwait(false);
            if (split.HasTempFiles)
            {
                tempFiles.AddRange(split.Chunks);
                paths = split.Chunks;
            }
        }

        var transcripts = new List<string>(paths.Count);
        foreach (var path in paths)
        {
            ct.ThrowIfCancellationRequested();
            var text = await this.client.TranscribeAsync(path, ct).ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(text))
            {
                transcripts.Add(text);
            }
        }

        var combined = TranscriptCombiner.Combine(transcripts);
        return new VoiceTranscriptionResult(combined, tempFiles);
    }
}
