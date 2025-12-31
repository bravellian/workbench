namespace Workbench.Core.Voice;

/// <summary>
/// Computed limits for audio recording.
/// </summary>
/// <param name="MaxDuration">Maximum recording duration.</param>
/// <param name="MaxBytes">Maximum size in bytes.</param>
/// <param name="MaxFrames">Maximum frame count.</param>
public sealed record AudioRecordingLimits(TimeSpan MaxDuration, long MaxBytes, long MaxFrames);
