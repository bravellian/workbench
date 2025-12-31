using System.Text;

namespace Workbench.Core.Voice;

public sealed record WaveFileInfo(
    int SampleRateHz,
    int Channels,
    int BitsPerSample,
    long DataLength,
    long DataOffset)
{
    public int BytesPerSample => BitsPerSample / 8;
    public int BlockAlign => Channels * this.BytesPerSample;
    public int BytesPerSecond => SampleRateHz * this.BlockAlign;
    public TimeSpan Duration => this.BytesPerSecond == 0
        ? TimeSpan.Zero
        : TimeSpan.FromSeconds(DataLength / (double)this.BytesPerSecond);

    public static WaveFileInfo Read(string path)
    {
        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream, Encoding.ASCII, leaveOpen: true);

        var riff = new string(reader.ReadChars(4));
        if (!string.Equals(riff, "RIFF", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Invalid WAV header.");
        }
        _ = reader.ReadInt32();
        var wave = new string(reader.ReadChars(4));
        if (!string.Equals(wave, "WAVE", StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Invalid WAV header.");
        }

        var sampleRate = 0;
        var channels = 0;
        var bitsPerSample = 0;
        long dataLength = 0;
        long dataOffset = 0;

        while (stream.Position + 8 <= stream.Length)
        {
            var chunkId = new string(reader.ReadChars(4));
            var chunkSize = reader.ReadInt32();
            if (string.Equals(chunkId, "fmt ", StringComparison.Ordinal))
            {
                var formatTag = reader.ReadInt16();
                if (formatTag != 1)
                {
                    throw new InvalidOperationException("Unsupported WAV format.");
                }
                channels = reader.ReadInt16();
                sampleRate = reader.ReadInt32();
                _ = reader.ReadInt32();
                _ = reader.ReadInt16();
                bitsPerSample = reader.ReadInt16();
                if (chunkSize > 16)
                {
                    stream.Position += chunkSize - 16;
                }
            }
            else if (string.Equals(chunkId, "data", StringComparison.Ordinal))
            {
                dataOffset = stream.Position;
                dataLength = chunkSize;
                stream.Position += chunkSize;
            }
            else
            {
                stream.Position += chunkSize;
            }
        }

        if (sampleRate <= 0 || channels <= 0 || bitsPerSample <= 0 || dataOffset == 0)
        {
            throw new InvalidOperationException("Missing WAV format metadata.");
        }

        return new WaveFileInfo(sampleRate, channels, bitsPerSample, dataLength, dataOffset);
    }
}
