using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Sequential)]
internal struct AVChapter
{
    public long id;
    public AVRational time_base;
    public long start;
    public long end;
    public IntPtr metadata; // AVDictionary* metadata;
}