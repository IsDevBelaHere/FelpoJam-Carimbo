using Godot;
using System;
public partial class LevelStart : Control
{
	public static LevelStart instance;
	public override void _Ready()
	{
		if (instance != this)
		{
			instance = this;
		}else
		{
			QueueFree();
		}
	}

	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("confirm") && Visible)
		{
			Visible = false;
			CarimboManager.instance.Start();
			Player.instance.speedMultiplier = Player.instance.sceneSpeedMultiplier;
		}
	}
}
