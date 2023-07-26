namespace AvaloniaInside.MediaPlayer;

public class PlaybackBuilder
{
    private IMediaSource _mediaSource;
    private bool _withAudio;
    private bool _withVideo;

    public PlaybackBuilder WithMediaSource(IMediaSource mediaSource)
    {
        _mediaSource = mediaSource;
        return this;
    }

    public PlaybackBuilder WithAudio()
    {
        _withAudio = true;
        return this;
    }

    public PlaybackBuilder WithVideo()
    {
        _withVideo = true;
        return this;
    }

    public BasePlayback Build()
    {
        if (_withAudio && _withVideo)
            throw new InvalidOperationException("Cannot build a playback with both audio and video");

        BasePlayback playback = null;
        if (_withAudio)
            playback = new AudioPlayback();

        if (_withVideo) 
            playback = new VideoPlayback();

        if (playback == null)
            throw new InvalidOperationException("Cannot build a playback without audio or video");

        playback.MediaSource = _mediaSource;
        playback.Initialize();

        // reset for reuse
        Reset();

        return playback;
    }

    private void Reset()
    {
        _withAudio = false;
        _withVideo = false;
        _mediaSource = null;
    }
}