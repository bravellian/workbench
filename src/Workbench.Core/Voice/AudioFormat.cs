namespace Workbench.Core.Voice;

public sealed record AudioFormat(int SampleRateHz, int Channels, int BitsPerSample)
{
    public int BytesPerSample => BitsPerSample / 8;
    public int BlockAlign => Channels * this.BytesPerSample;
    public int BytesPerSecond => SampleRateHz * this.BlockAlign;
}
