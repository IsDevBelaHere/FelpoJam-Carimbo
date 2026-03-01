using Godot;
using System;

public partial class ProgressManager : Node
{
    [Export] public Progress progress;
    static ProgressManager instance;
    public static Progress Progress {get {return instance.progress;} private set {}}
    public override void _Ready()
    {
        instance = this;
        if (FileAccess.FileExists("user://progress.tres"))
        {
            progress = GD.Load<Progress>("user://progress.tres");
        }
        else
        {
            progress.levels[0] = 1;
            progress.SaveData("user://progress.tres");
        }
    }
}
