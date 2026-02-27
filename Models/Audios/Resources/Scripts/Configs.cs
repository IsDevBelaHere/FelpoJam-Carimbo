using Godot;

public partial class Configs : SavableResource
{
    [ExportGroup("Audio")]
    [Export] public float masterAudio = 1f;
    [Export] public float effectsAudio = 1f;
    [Export] public float musicAudio = 1f;
    [ExportGroup("Video")]
    [Export] public Vector2I resolution = new(640,360);
    [Export] public DisplayServer.WindowMode windowMode = DisplayServer.WindowMode.Windowed;
}
