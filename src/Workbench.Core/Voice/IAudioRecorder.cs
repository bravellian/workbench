namespace Workbench.Core.Voice;

public interface IAudioRecorder
{
    Task<IAudioRecordingSession> StartAsync(AudioRecordingOptions options, CancellationToken ct);
}
