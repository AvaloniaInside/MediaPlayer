using System.Collections.Concurrent;
using AvaloniaInside.MediaPlayer;

namespace AvaloniaInside.MediaPlayer;

public abstract class Packet : IDisposable {
    ~Packet() => Dispose();
    public virtual void Dispose() => GC.SuppressFinalize(this);
}

public unsafe class AudioPacket : Packet {
    public readonly short[] SampleBuffer;
    public readonly int TotalSampleCount;

    public AudioPacket(byte* sampleBuffer, int sampleCount, int channelCount) {
        // I have no idea why the * 2 is necessary, but without it only half of each packet is played
        TotalSampleCount = sampleCount * channelCount * 2;
        SampleBuffer = new short[TotalSampleCount];

        // copy buffer
        for(int i = 0; i < TotalSampleCount; i++) {
            SampleBuffer[i] = ((short*)sampleBuffer)[i];
        }
    }
}

public abstract class Playback<TPacket> : IDisposable where TPacket : Packet
{
    public readonly MediaSource MediaSource;
    protected readonly BlockingCollection<TPacket> PacketQueue = new();

    protected Playback(MediaSource dataSource)
    {
        MediaSource = dataSource;
    }

    internal int QueuedPackets => PacketQueue.Count;

    public virtual void Dispose()
    {
        Flush();
        PacketQueue.Dispose();
    }

    internal abstract void SourceReloaded();
    internal abstract void StateChanged(PlayState oldState, PlayState newState);

    internal virtual void Flush()
    {
        while (PacketQueue.TryTake(out var packet)) packet.Dispose();
    }

    internal void PushPacket(TPacket packet)
    {
        PacketQueue.Add(packet);
    }
}