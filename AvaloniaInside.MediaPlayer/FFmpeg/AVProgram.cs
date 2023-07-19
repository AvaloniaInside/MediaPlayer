using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct AVProgram
{
    public int id;
    public int flags;
    public AVDiscard discard;
    public uint* stream_index;
    public uint nb_stream_indexes;
    public IntPtr metadata;
    public int program_num;
    public int pmt_pid;
    public int pcr_pid;
    public int pmt_version;
    public long start_time;
    public long end_time;
    public long pts_wrap_reference;
    public int pts_wrap_behavior;
}