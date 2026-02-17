using Godot;
using System;

public partial class KarimboTrigger : Node2D
{
	public string carimboType = "CarimboSlime";
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
		carimboRes = GD.Load<Carimbo>("res://Entities/Karimbos/Resources/" + carimboType + ".tres");
        if (carimboRes is CarimboPlatform)
		{
			GetChild(1).GetChild<CollisionShape2D>(2).Disabled = false;
		}
    }
}