using NAudio.Wave;

namespace AvaloniaInside.MediaPlayer;

public class AudioPlayback : Playback<AudioPacket>
{
    private WaveOutEvent _outputDevice;
    private BufferedWaveProvider _waveProvider;

    public override void Init(IMediaSource mediaSource)
    {
        base.Init(mediaSource);
        _waveProvider = new BufferedWaveProvider(new WaveFormat(mediaSource.AudioSampleRate, mediaSource.AudioChannelCount));
        _outputDevice = new WaveOutEvent();
        _outputDevice.Init(_waveProvider);
        _isInitialized = true;
    }

    public override void Dispose()
    {
        _outputDevice?.Stop();
        _outputDevice?.Dispose();
        _waveProvider?.ClearBuffer();
        base.Dispose();
    }

    internal override void SourceReloaded()
    {
    }

    public override void Update(TimeSpan deltaTime)
    {
        if(!_isInitialized)
            throw new InvalidOperationException("Audio playback is not initialized");
        while (PacketQueue.TryTake(out var packet))
            _waveProvider.AddSamples(packet.SampleBuffer, 0, packet.SampleBuffer.Length);
    }
}