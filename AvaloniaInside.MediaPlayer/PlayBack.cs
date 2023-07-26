using System.Collections.Concurrent;

namespace AvaloniaInside.MediaPlayer;

public abstract class Playback<TPacket> : BasePlayback where TPacket : Packet
{
    protected readonly BlockingCollection<TPacket> PacketQueue = new();

    /// <summary>
    ///     Gets the packet queue count
    /// </summary>
    public int QueuedPacketsCount => PacketQueue.Count;

    public override void Dispose()
    {
        Flush();
        PacketQueue.Dispose();
        base.Dispose();
    }

    protected abstract void OnSourceReloaded();

    /// <summary>
    ///     Flush all packets in queue
    /// </summary>
    private void Flush()
    {
        while (PacketQueue.TryTake(out var packet)) packet.Dispose();
    }

    /// <summary>
    ///     Add a packet to the queue
    /// </summary>
    /// <param name="packet"></param>
    protected internal void AddPacket(TPacket packet)
    {
        PacketQueue.Add(packet);
    }
}