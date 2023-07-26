using System.Drawing;
using FFmpeg.AutoGen;

namespace AvaloniaInside.MediaPlayer;

public unsafe partial class MediaSource : IMediaSource
{
    // VIDEO
    private int _videoStreamId = -1;
    private SwsContext* _videoSwContext;

    private void InitVideo()
    {
        
    }

    private VideoPacket? DecodeVideo(AVPacket* packet)
    {
        return null;
    }
}