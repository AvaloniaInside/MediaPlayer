using System.Collections.Concurrent;
using AvaloniaInside.MediaPlayer;

namespace AvaloniaInside.MediaPlayer;

public abstract class Playback<TPacket> : IDisposable where TPacket : Packet
{
    protected bool _isInitialized = false;
    protected IMediaSource _mediaSource;
    protected readonly BlockingCollection<TPacket> PacketQueue = new();

    public virtual void Init(IMediaSource mediaSource)
    {
        _mediaSource = mediaSource;
    }

    internal int QueuedPackets => PacketQueue.Count;

    public virtual void Dispose()
    {
        Flush();
        PacketQueue.Dispose();
    }

    internal abstract void SourceReloaded();
    internal virtual void Flush()
    {
        while (PacketQueue.TryTake(out var packet)) packet.Dispose();
    }

    internal void PushPacket(TPacket packet)
    {
        PacketQueue.Add(packet);
    }

    public abstract void Update(TimeSpan deltaTime);
}
