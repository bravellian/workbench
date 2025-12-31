namespace Workbench.Core.Voice;

/// <summary>
/// Abstraction over audio capture implementations.
/// </summary>
public interface IAudioRecorder
{
    /// <summary>
    /// Starts recording audio based on the provided options.
    /// </summary>
    Task<IAudioRecordingSession> StartAsync(AudioRecordingOptions options, CancellationToken ct);
}
