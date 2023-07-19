using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Sequential)]
internal struct AVIOInterruptCB
{
    public IntPtr callback; // int (*callback)(void*)
    public IntPtr opaque; // void* opaque;
}