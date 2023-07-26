using NAudio.Wave;

namespace AvaloniaInside.MediaPlayer;

public class AudioPlayback : Playback<AudioPacket>
{
    private WaveOutEvent _outputDevice;
    private BufferedWaveProvider _waveProvider;

    public override void Initialize()
    {
        base.Initialize();
        _waveProvider =
            new BufferedWaveProvider(new WaveFormat(MediaSource.AudioSampleRate, MediaSource.AudioChannelCount));
        _outputDevice = new WaveOutEvent();
        _outputDevice.Init(_waveProvider);
    }

    public override void Dispose()
    {
        _outputDevice?.Stop();
        _outputDevice?.Dispose();
        _waveProvider?.ClearBuffer();
        base.Dispose();
    }

    protected override void OnSourceReloaded()
    {
        throw new NotImplementedException();
    }

    public override void Update(TimeSpan deltaTime)
    {
        if (!IsInitialized)
            throw new InvalidOperationException("Audio playback is not initialized");
        while (PacketQueue.TryTake(out var packet) && packet is AudioPacket audioPacket)
            _waveProvider.AddSamples(audioPacket.SampleBuffer, 0, audioPacket.SampleBuffer.Length);
    }
}