using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct AVPacketList
{
    public AVPacket pkt;
    public AVPacketList* next;
}