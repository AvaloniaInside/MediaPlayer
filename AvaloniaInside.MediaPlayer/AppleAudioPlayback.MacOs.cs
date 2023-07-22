using System;
using System.Runtime.InteropServices;
using AVFoundation;
using Foundation;

namespace AvaloniaInside.MediaPlayer;

public class AppleAudioPlayback : Playback<AudioPacket>
{
	private AVAudioEngine audioEngine;
	private AVAudioPlayerNode playerNode;
	private AVAudioFormat audioFormat;

	public AppleAudioPlayback(IMediaSource dataSource) : base(dataSource)
	{
		// Set up the audio engine and player node
		audioEngine = new AVAudioEngine();
		playerNode = new AVAudioPlayerNode();
		audioEngine.AttachNode(playerNode);

		// Connect the player node to the output of the audio engine
		var mixer = audioEngine.MainMixerNode;
		audioEngine.Connect(playerNode, mixer, audioFormat);

		// Start the audio engine
		NSError error;
		audioEngine.StartAndReturnError(out error);
		if (error != null)
		{
			Console.WriteLine($"Error starting audio engine: {error}");
		}
	}

	internal override void SourceReloaded()
	{
	}

	internal override void StateChanged(PlayState oldState, PlayState newState)
	{
	}

	public override void Update(TimeSpan deltaTime)
	{
		while (PacketQueue.TryTake(out var packet))
		{
			PlayWaveBuffer(packet);
		}
	}

	public void PlayWaveBuffer(AudioPacket waveData)
	{
		if (waveData == null || waveData.TotalSampleCount == 0)
		{
			Console.WriteLine("Wave data is empty.");
			return;
		}

		// Set up the audio format based on the wave data
		int sampleRate = MediaSource.AudioSampleRate; // Adjust the sample rate as needed
		int channels = MediaSource.AudioChannelCount; // Mono audio
		int bytesPerSample = 2; // 16-bit audio
		audioFormat = new AVAudioFormat(sampleRate, (uint)channels);

		playerNode.Play();

		// Create an AVAudioPCMBuffer and set the audio data
		var buffer = new AVAudioPcmBuffer(audioFormat, (uint)waveData.TotalSampleCount / (uint)bytesPerSample);

		buffer.MutableAudioBufferList.SetData(0, NSData.FromArray(waveData.SampleBuffer).Handle);
		// Write audio data to the buffer
		// unsafe
		// {
		// 	// Get the pointer to the audio buffer data (first buffer)
		// 	buffer.MutableAudioBufferList.Handle.Handle
		// 	var audioBufferList = buffer.MutableAudioBufferList;
		// 	var audioBuffer = audioBufferList[0];
		// 	var dataPointer = (short*)audioBuffer.Data;
		// 	for (int i = 0; i < waveData.SampleBuffer.Length / bytesPerSample; i++)
		// 	{
		// 		// Convert bytes to short and store in the buffer
		// 		dataPointer[i] = BitConverter.ToInt16(waveData.SampleBuffer, i * bytesPerSample);
		// 	}
		//
		// 	buffer.FrameLength = (uint)(waveData.SampleBuffer.Length / bytesPerSample);
		// }

		playerNode.ScheduleBuffer(buffer, () => { Console.WriteLine("Wave buffer playback completed."); });
	}
}
