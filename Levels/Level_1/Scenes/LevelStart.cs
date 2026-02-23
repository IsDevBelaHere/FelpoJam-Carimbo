using Godot;
using System;
public partial class LevelStart : Control
{
	
	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("confirm"))
		{
			Visible = false;
		}
	}
}
