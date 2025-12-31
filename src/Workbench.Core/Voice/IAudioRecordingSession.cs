namespace Workbench.Core.Voice;

/// <summary>
/// Represents an in-progress audio recording session.
/// </summary>
public interface IAudioRecordingSession : IAsyncDisposable
{
    /// <summary>Task that completes when recording finishes or is cancelled.</summary>
    Task<AudioRecordingResult> Completion { get; }
    /// <summary>Signals the recorder to stop gracefully.</summary>
    Task StopAsync(CancellationToken ct);
    /// <summary>Signals the recorder to cancel and discard capture.</summary>
    Task CancelAsync(CancellationToken ct);
}
