using Godot;
using System;
using System.Runtime.InteropServices.Marshalling;



public partial class LevelGoal : Area2D
{
    [Export] public Levels nextLevel;
	public void AreaEnter(Node collider)
    {
        if (collider is Player)
        {
            CallDeferred(MethodName.PerformSceneChange);  
        }
    }
    public void PerformSceneChange()
    {
        GetTree().ChangeSceneToFile(Globals.LevelsToScene[nextLevel]);
    }
}
