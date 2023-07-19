using System.Runtime.InteropServices;

namespace AvaloniaInside.MediaPlayer.FFmpeg;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct AVCodecParser
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
    public int[] codec_ids;

    public int priv_data_size;

    public ParserInitDelegate parser_init;
    public ParserParseDelegate parser_parse;
    public ParserCloseDelegate parser_close;
    public SplitDelegate split;

    public delegate int ParserInitDelegate(AVCodecParserContext* s);
    public delegate int ParserParseDelegate(AVCodecParserContext* s, AVCodecContext* avctx, byte** poutbuf, int* poutbuf_size, byte* buf, int buf_size);
    public delegate void ParserCloseDelegate(AVCodecParserContext* s);
    public delegate int SplitDelegate(AVCodecContext* avctx, byte* buf, int buf_size);
}