namespace Workbench.Core;

/// <summary>
/// Audio metadata attached to a generated doc source reference.
/// </summary>
/// <param name="SampleRateHz">Sample rate in Hz.</param>
/// <param name="Channels">Number of audio channels.</param>
/// <param name="Format">Audio format label.</param>
public sealed record DocAudioInfo(int SampleRateHz, int Channels, string Format);
