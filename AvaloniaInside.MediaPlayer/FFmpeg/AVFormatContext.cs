using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Explicit)]
internal unsafe struct AVFormatContext
{
    [FieldOffset(24)] internal uint nb_streams;
    [FieldOffset(28)] internal AVStream** streams;
    [FieldOffset(1072)] internal long duration;
}