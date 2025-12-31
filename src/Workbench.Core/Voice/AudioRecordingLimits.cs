namespace Workbench.Core.Voice;

public sealed record AudioRecordingLimits(TimeSpan MaxDuration, long MaxBytes, long MaxFrames);
