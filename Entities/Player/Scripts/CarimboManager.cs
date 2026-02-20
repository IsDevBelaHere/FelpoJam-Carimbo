using Godot;
using System;

public partial class CarimboManager : Node
{
	[Export] public PackedScene carimbo;
	public static CarimboManager instance;
	public string carimboType = "CarimboPlatform";
	public int roundingGrid = 8;
	public Vector2 roundToMultiple(Vector2 coords, int rounding)
	{
		int numberX = (int)coords.X / rounding * rounding;
		int numberY = (int)coords.Y / rounding * rounding;

		if (coords.X - numberX < numberX + rounding - coords.X)
		{
			coords.X = numberX;
		} else
		{
			coords.X = numberX + rounding;
		}
		
		if (coords.Y - numberY < numberY + rounding - coords.Y)
		{
			coords.Y = numberY;
		} else
		{
			coords.Y = numberY + rounding;
		}

		return coords;
	}

    public override void _Process(double delta)
    {
		if (Input.IsActionJustPressed("karimbo_slot1"))
		{
			carimboType = "CarimboPlatform";
		}

		if (Input.IsActionJustPressed("karimbo_slot2"))
		{
			carimboType = "CarimboSwap";
		}

		if (Input.IsActionJustPressed("karimbo_slot3"))
		{
			carimboType = "CarimboJump";
		}

		if (Input.IsActionJustPressed("karimbo_slot4"))
		{
			carimboType = "CarimboBoost";
		}

		if (Input.IsActionJustPressed("karimbo_slot5"))
		{
			carimboType = "CarimboStop";
		}

		if (Input.IsActionJustPressed("karimbo_slot6"))
		{
			carimboType = "CarimboSlime";
		}

		if (Input.IsActionJustPressed("mouse_1"))
		{
			Node2D new_carimbo = carimbo.Instantiate<Node2D>();
			AddChild(new_carimbo);
			new_carimbo.Position = roundToMultiple(GetViewport().GetMousePosition().Floor(), roundingGrid);
			if (carimboType == "CarimboPlatform")
			{
				new_carimbo.GetChild<Area2D>(0).QueueFree();
			}
		}
    }

	public override void _Ready()
    {
        if (instance != this)
		{
			instance = this;
		} else
		{
			QueueFree();	
		}
		
    }
}
