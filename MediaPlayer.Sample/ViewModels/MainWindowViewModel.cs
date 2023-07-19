using System;
using AvaloniaInside.MediaPlayer;

namespace MediaPlayer.Sample.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";

    public void Test()
    {
        AvaloniaInside.MediaPlayer.MediaPlayer player = new();

        var source = new MediaSource();
        source.Load("/Users/mw/Downloads/file_example_MP3_1MG.mp3");
    }
}