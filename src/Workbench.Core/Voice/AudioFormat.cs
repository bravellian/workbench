namespace Workbench.Core.Voice;

/// <summary>
/// PCM audio format parameters used for recording and transcription.
/// </summary>
/// <param name="SampleRateHz">Sample rate in Hz.</param>
/// <param name="Channels">Number of audio channels.</param>
/// <param name="BitsPerSample">Bit depth per sample.</param>
public sealed record AudioFormat(int SampleRateHz, int Channels, int BitsPerSample)
{
    /// <summary>Bytes per sample based on bit depth.</summary>
    public int BytesPerSample => BitsPerSample / 8;
    /// <summary>Bytes per audio frame across channels.</summary>
    public int BlockAlign => Channels * this.BytesPerSample;
    /// <summary>Bytes per second for the given format.</summary>
    public int BytesPerSecond => SampleRateHz * this.BlockAlign;
}
