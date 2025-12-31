using System.Runtime.InteropServices;
using System.Text;

namespace Workbench.Core.Voice;

public sealed class WaveFileWriter : IDisposable
{
    private readonly FileStream stream;
    private readonly BinaryWriter writer;
    private readonly AudioFormat format;
    private long dataLength;
    private bool disposed;

    public WaveFileWriter(string path, AudioFormat format)
    {
        if (format.BitsPerSample != 16)
        {
            throw new InvalidOperationException("Only 16-bit PCM is supported.");
        }

        this.format = format;
        this.stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
        this.writer = new BinaryWriter(this.stream, Encoding.ASCII, leaveOpen: true);
        this.WriteHeader(0);
    }

    public void WriteSamples(ReadOnlySpan<short> samples)
    {
        if (samples.IsEmpty)
        {
            return;
        }

        var bytes = MemoryMarshal.AsBytes(samples);
        this.writer.Write(bytes);
        this.dataLength += bytes.Length;
    }

    public void WriteBytes(ReadOnlySpan<byte> bytes)
    {
        if (bytes.IsEmpty)
        {
            return;
        }

        this.writer.Write(bytes);
        this.dataLength += bytes.Length;
    }

    public void Dispose()
    {
        if (this.disposed)
        {
            return;
        }
        this.disposed = true;
        this.writer.Flush();
        this.stream.Seek(0, SeekOrigin.Begin);
        this.WriteHeader(this.dataLength);
        this.writer.Flush();
        this.writer.Dispose();
        this.stream.Dispose();
    }

    private void WriteHeader(long dataBytes)
    {
        var byteRate = this.format.BytesPerSecond;
        var blockAlign = this.format.BlockAlign;

        this.writer.Write("RIFF"u8.ToArray());
        this.writer.Write((int)(36 + dataBytes));
        this.writer.Write("WAVE"u8.ToArray());
        this.writer.Write("fmt "u8.ToArray());
        this.writer.Write(16);
        this.writer.Write((short)1);
        this.writer.Write((short)this.format.Channels);
        this.writer.Write(this.format.SampleRateHz);
        this.writer.Write(byteRate);
        this.writer.Write((short)blockAlign);
        this.writer.Write((short)this.format.BitsPerSample);
        this.writer.Write("data"u8.ToArray());
        this.writer.Write((int)dataBytes);
    }
}
