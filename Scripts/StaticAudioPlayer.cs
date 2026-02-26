using Godot;
using System;
using System.Threading.Tasks;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

public partial class StaticAudioPlayer : Node
{
    public static StaticAudioPlayer instance;
    public static AudioStreamPlayer2D audioPlayer;
    public static AudioListener2D audioListener;
    public bool looping;
    public override void _Ready()
    {
        if (instance != this)
        {
            instance = this;
        }
        else
        {
            QueueFree();
        }
        audioPlayer = GetChild<AudioStreamPlayer2D>(0);
        audioListener = GetChild<AudioListener2D>(1);
    }
    public void Stop()
    {
        looping = false;
        audioPlayer.Stop();
    }

    private async Task Loop()
    {
        while (looping)
        {
            audioPlayer.Play();
            await ToSignal(audioPlayer,"finished");
        }
    }
    public void PlayCD(string path, bool looping = false)
    {
        if (audioPlayer.Playing)
        {
            Stop();
        }

        this.looping = looping;
        audioPlayer.Stream = (AudioStream)GD.Load<AudioStream>(path).Duplicate();
        if (looping)
        {
            Loop();
        }
        else
        {
            audioPlayer.Play();
        }
    }
    public void PlayCD(AudioStream stream, bool looping = false)
    {
        if (audioPlayer.Playing)
        {
            Stop();
        }
        
        this.looping = looping;
        audioPlayer.Stream = stream;
        if (looping)
        {
            Loop();
        }
        else
        {
            audioPlayer.Play();
        }
    }
}
