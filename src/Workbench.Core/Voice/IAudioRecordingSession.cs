namespace Workbench.Core.Voice;

public interface IAudioRecordingSession : IAsyncDisposable
{
    Task<AudioRecordingResult> Completion { get; }
    Task StopAsync(CancellationToken ct);
    Task CancelAsync(CancellationToken ct);
}
