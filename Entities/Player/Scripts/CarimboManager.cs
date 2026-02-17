using Godot;
using System;

public partial class CarimboManager : Node
{
	[Export] public PackedScene carimbo;
    public override void _Process(double delta)
    {
		if (Input.IsActionJustPressed("mouse_1"))
		{
			Node2D new_carimbo = carimbo.Instantiate<Node2D>();
			AddChild(new_carimbo);
			new_carimbo.Position = GetViewport().GetMousePosition();
		}
    }

}
