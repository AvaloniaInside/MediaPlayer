using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Sequential)]
internal struct AVProbeData
{
    public IntPtr filename;
    public IntPtr buf;
    public int buf_size;
    public IntPtr mime_type;
}