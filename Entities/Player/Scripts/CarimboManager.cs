using Godot;
using System;

public partial class CarimboManager : Node
{
	[Export] public PackedScene carimbo;
	public static CarimboManager instance;
	public string carimboType = "CarimboPlatform";
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

		if (Input.IsActionJustPressed("pause"))
		{
			if (Player.instance.pauseToggle)
			{
				Player.instance.pauseToggle = false;
			}
			else
			{
				Player.instance.pauseToggle = true;				
			}
		}

		if (Input.IsActionJustPressed("mouse_1"))
		{
			Node2D new_carimbo = carimbo.Instantiate<Node2D>();
			AddChild(new_carimbo);
			new_carimbo.Position = GetViewport().GetMousePosition();
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
