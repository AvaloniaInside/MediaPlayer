using static System.GC;

namespace AvaloniaInside.MediaPlayer;

public abstract class Packet {
    ~Packet() => Dispose();
    public virtual void Dispose() => SuppressFinalize(this);

}