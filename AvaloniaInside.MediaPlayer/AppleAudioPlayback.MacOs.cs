using AVFoundation;

namespace AvaloniaInside.MediaPlayer;

public class AppleAudioPlayback : Playback<AudioPacket>
{
    private AVAudioEngine _audioEngine;
    private AVAudioFormat _audioFormat;
    private AVAudioPlayerNode _playerNode;

    public override void Init(IMediaSource mediaSource)
    {
        base.Init(mediaSource);
        // Set up the audio engine and player node
        _audioEngine = new AVAudioEngine();
        _playerNode = new AVAudioPlayerNode();
        _audioEngine.AttachNode(_playerNode);

        // Connect the player node to the output of the audio engine
        var mixer = _audioEngine.MainMixerNode;
        _audioEngine.Connect(_playerNode, mixer, _audioFormat);

        // Start the audio engine
        NSError error;
        _audioEngine.StartAndReturnError(out error);
        if (error != null)
            Console.WriteLine($"Error starting audio engine: {error}");
        _isInitialized = true;
    }

    internal override void SourceReloaded()
    {
    }

    public override void Update(TimeSpan deltaTime)
    {
        if (!_isInitialized)
            throw new InvalidOperationException("Audio playback is not initialized");
        while (PacketQueue.TryTake(out var packet)) PlayWaveBuffer(packet);
    }

    private void PlayWaveBuffer(AudioPacket waveData)
    {
        if (waveData == null || waveData.TotalSampleCount == 0)
        {
            Console.WriteLine("Wave data is empty.");
            return;
        }

        // Set up the audio format based on the wave data
        var sampleRate = _mediaSource.AudioSampleRate; // Adjust the sample rate as needed
        var channels = _mediaSource.AudioChannelCount; // Mono audio
        var bytesPerSample = 2; // 16-bit audio
        _audioFormat = new AVAudioFormat(sampleRate, (uint)channels);

        _playerNode.Play();

        var frameLength = waveData.SampleBuffer.Length / (channels * bytesPerSample);
        // Create an AVAudioPCMBuffer and set the audio data
        var buffer = new AVAudioPcmBuffer(_audioFormat, (uint)waveData.TotalSampleCount / (uint)bytesPerSample);

        unsafe
        {
            var floatChannelData = (float**)buffer.FloatChannelData;
            for (var channel = 0; channel < channels; channel++)
            {
                var dataPointer = floatChannelData[channel];
                for (var i = 0; i < frameLength; i++)
                {
                    float value;
                    if (bytesPerSample == 2)
                    {
                        var sampleValue = BitConverter.ToInt16(waveData.SampleBuffer, (i * channels + channel) * 2);
                        value = (float)sampleValue / short.MaxValue;
                    }
                    else // Assuming bytesPerSample == 4 for 32-bit float
                    {
                        value = BitConverter.ToSingle(waveData.SampleBuffer, (i * channels + channel) * 4);
                    }

                    dataPointer[i] = value;
                }
            }
        }

        buffer.FrameLength = (uint)frameLength;
        _playerNode.ScheduleBuffer(buffer, () => { Console.WriteLine("Wave buffer playback completed."); });
    }
}