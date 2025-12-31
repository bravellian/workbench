namespace Workbench.Core;

/// <summary>
/// Describes the source metadata for a generated document.
/// </summary>
/// <param name="Kind">Source kind (e.g., voice).</param>
/// <param name="Transcript">Optional transcript excerpt.</param>
/// <param name="Audio">Audio metadata when generated from voice.</param>
public sealed record DocSourceInfo(string Kind, string? Transcript, DocAudioInfo Audio);
