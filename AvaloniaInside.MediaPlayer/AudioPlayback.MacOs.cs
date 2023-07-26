using AVFoundation;

namespace AvaloniaInside.MediaPlayer;

public class AudioPlayback : Playback<AudioPacket>
{
    private AVAudioEngine _audioEngine;
    private AVAudioFormat _audioFormat;
    private AVAudioPlayerNode _playerNode;

    public override void Initialize()
    {
        base.Initialize();
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
    }

    public override void Update(TimeSpan deltaTime)
    {
        if (!IsInitialized)
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
        var sampleRate = MediaSource.AudioSampleRate; // Adjust the sample rate as needed
        var channels = MediaSource.AudioChannelCount;
        var bytesPerSample = MediaSource.BytesPerSample;
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

    protected override void OnSourceReloaded()
    {
        throw new NotImplementedException();
    }
}