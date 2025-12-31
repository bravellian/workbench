namespace Workbench.Core.Voice;

public sealed record WaveFileSplitResult(IReadOnlyList<string> Chunks, bool HasTempFiles);
