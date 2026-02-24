using Godot;
using System;
public partial class LevelStart : Control
{
	private bool confirmed;
	[Export] public float playerSpeedMultiplier;
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
		
		

		if (Input.IsActionJustPressed("confirm") && !confirmed)
		{
			Visible = false;
			Player.instance.speedMultiplier = playerSpeedMultiplier;
			CarimboManager.instance.Start();
			confirmed = true;
		}
	}
}
