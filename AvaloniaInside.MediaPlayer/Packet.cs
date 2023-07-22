namespace AvaloniaInside.MediaPlayer;

public abstract class Packet : IDisposable {
    ~Packet() => Dispose();
    public virtual void Dispose() => GC.SuppressFinalize(this);
}