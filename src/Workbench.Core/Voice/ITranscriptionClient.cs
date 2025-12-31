namespace Workbench.Core.Voice;

/// <summary>
/// Abstraction over transcription providers for audio files.
/// </summary>
public interface ITranscriptionClient
{
    /// <summary>Transcribes a WAV file and returns the raw transcript text.</summary>
    Task<string> TranscribeAsync(string wavPath, CancellationToken ct);
}
