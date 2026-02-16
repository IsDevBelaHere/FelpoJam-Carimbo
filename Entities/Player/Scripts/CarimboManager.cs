using Godot;
using System;

public partial class CarimboManager : Node
{
	[Export]
	public Carimbo carimboRes;

    public override void _Process(double delta)
    {
		if (Input.IsActionJustPressed("mouse_1"))
		{
			
		}
    }

}
