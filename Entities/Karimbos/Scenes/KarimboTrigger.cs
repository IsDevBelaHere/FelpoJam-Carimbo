using Godot;
using System;

public partial class KarimboTrigger : Node2D
{
	
	Carimbo carimboRes;

	public void AreaEnter(Node2D collider)
	{
		if (collider is Player)
		{
			carimboRes.CarimboFunction(true);
		}
	}

	public void AreaEXit(Node2D collider)
	{
		if (collider is Player)
		{
			carimboRes.CarimboFunction(false);
		}
	}

    public override void _Ready()
    {
		carimboRes = GD.Load<Carimbo>("res://Entities/Karimbos/Resources/" + CarimboManager.instance.carimboType + ".tres");
        if (carimboRes is CarimboPlatform)
		{
			GetChild(1).GetChild<CollisionShape2D>(2).Disabled = false;


		}
		else
		{
			GetChild<Sprite2D>(2).Texture = carimboRes.CarimboImg;
			GetChild<Label>(3).Text = carimboRes.CarimboText;
			GetChild<Label>(3).LabelSettings = (LabelSettings)GetChild<Label>(3).LabelSettings.Duplicate();
			GetChild<Label>(3).LabelSettings.FontColor = carimboRes.textColor;
		}
    }
}