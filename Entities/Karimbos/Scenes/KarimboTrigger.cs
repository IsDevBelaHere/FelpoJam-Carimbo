using Godot;
using System;

public partial class KarimboTrigger : Node2D
{
	[Export]
	public Carimbo carimboRes;

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
        if (carimboRes is CarimboPlatform)
		{
			GetChild(1).GetChild<CollisionShape2D>(2).Disabled = false;
		}
    }


}
