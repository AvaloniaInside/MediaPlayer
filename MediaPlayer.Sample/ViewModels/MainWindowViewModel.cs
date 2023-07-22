using System;
using AvaloniaInside.MediaPlayer;

namespace MediaPlayer.Sample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
	public string Greeting => "Welcome to Avalonia!";

	public void Test()
	{
		var source = new MediaSource();
		source.Load("/Users/omidmafakher/Downloads/sample-15s.mp3");



#if MACOS
		var player = new AvaloniaInside.MediaPlayer.MediaPlayer(null, new AppleAudioPlayback(source));
#else
		var player = new AvaloniaInside.MediaPlayer.MediaPlayer(null, new AudioPlayback(source));
#endif

		player.MediaSource = source;
		player.Play();
	}
}
