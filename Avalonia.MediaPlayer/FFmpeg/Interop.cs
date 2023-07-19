using System.Runtime.InteropServices;

namespace Avalonia.MediaPlayer.FFmpeg;

internal static unsafe class Interop
{
    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void* av_malloc(ulong size);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void av_free(void* ptr);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern AVFrame* av_frame_alloc();

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void av_frame_unref(AVFrame* frame);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void av_frame_free(AVFrame** frame);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_image_get_buffer_size(AVPixelFormat pixFmt, int width, int height, int align);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_image_fill_arrays(ref BytePtrArray4 dstData, ref IntArray4 dstLinesize, byte* src,
        AVPixelFormat pixFmt, int width, int height, int align);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_samples_get_buffer_size(int* linesize, int nbChannels, int nbSamples,
        AVSampleFormat sampleFmt, int align);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_samples_alloc(byte** audioData, int* linesize, int nbChannels, int nbSamples,
        AVSampleFormat sampleFmt, int align);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern long av_get_default_channel_layout(int nbChannels);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_get_channel_layout_nb_channels(ulong channelLayout);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_opt_set_int(void* obj, [MarshalAs(UnmanagedType.LPStr)] string name, long val,
        int searchFlags);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_opt_set_sample_fmt(void* obj, [MarshalAs(UnmanagedType.LPStr)] string name,
        AVSampleFormat fmt, int searchFlags);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int avformat_open_input(AVFormatContext** ps, [MarshalAs(UnmanagedType.LPStr)] string url,
        AVInputFormat* fmt, AVDictionary** options);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void avformat_close_input(AVFormatContext** s);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int avformat_find_stream_info(AVFormatContext* ic, AVDictionary** options);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_read_frame(AVFormatContext* s, AVPacket* pkt);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int av_seek_frame(AVFormatContext* s, int streamIndex, long timestamp, int flags);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern AVCodec* avcodec_find_decoder(AVCodecID id);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int avcodec_open2(AVCodecContext* avctx, AVCodec* codec, AVDictionary** options);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int avcodec_close(AVCodecContext* avctx);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void av_packet_free(AVPacket** pkt);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void av_init_packet(AVPacket* pkt);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void av_packet_unref(AVPacket* pkt);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern AVPacket* av_packet_alloc();

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int avcodec_send_packet(AVCodecContext* avctx, AVPacket* avpkt);

    [DllImport(Constants.LIBAVUTIL, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int avcodec_receive_frame(AVCodecContext* avctx, AVFrame* frame);

    [DllImport(Constants.LIBAVCODEC, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void avcodec_flush_buffers(AVCodecContext* avctx);

    [DllImport(Constants.LIBSWSCALE, CallingConvention = CallingConvention.Cdecl)]
    internal static extern SwsContext* sws_getCachedContext(SwsContext* context, int srcW, int srcH,
        AVPixelFormat srcFormat, int dstW, int dstH, AVPixelFormat dstFormat, int flags, SwsFilter* srcFilter,
        SwsFilter* dstFilter, double* param);

    [DllImport(Constants.LIBSWSCALE, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sws_scale(SwsContext* c, byte*[] srcSlice, int[] srcStride, int srcSliceY, int srcSliceH,
        byte*[] dst, int[] dstStride);

    [DllImport(Constants.LIBSWSCALE, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sws_freeContext(SwsContext* c);

    [DllImport(Constants.LIBSWRESAMPLE, CallingConvention = CallingConvention.Cdecl)]
    internal static extern SwrContext* swr_alloc();

    [DllImport(Constants.LIBSWRESAMPLE, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int swr_init(SwrContext* s);

    [DllImport(Constants.LIBSWRESAMPLE, CallingConvention = CallingConvention.Cdecl)]
    internal static extern int swr_convert(SwrContext* s, byte** @out, int outCount, byte** @in, int inCount);

    [DllImport(Constants.LIBSWRESAMPLE, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void swr_free(SwrContext** s);
}