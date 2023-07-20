using System;
using AvaloniaInside.MediaPlayer;

namespace MediaPlayer.Sample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public void Test()
    {
        var source = new MediaSource();
        source.Load("C:\\Users\\Omid\\Personal\\Omid\\Galery\\Music\\Amir\\Almoraima.mp3");


        var player = new AvaloniaInside.MediaPlayer.MediaPlayer(null, new AudioPlayback(source));
        player.MediaSource = source;
        player.Play();
    }
}