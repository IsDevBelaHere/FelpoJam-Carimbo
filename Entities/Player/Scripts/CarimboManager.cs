using System;
using System.Threading.Tasks;
using Godot;

public partial class CarimboManager : Node
{
	[Export] public PackedScene carimbo;
	[Export] public PackedScene carimboOverlay;
	[Export] public LevelInfo levelInfo;
	[Export] public Texture2D overlayOk;
	[Export] public Texture2D overlayNotOk;
	[Export] public VBoxContainer allTheThings;
	public Node2D[] carimboArray;
	public int carimboArrayCounter = 0;
	public int carimboTotalAmount = 0;
	public static CarimboManager instance;
	public string carimboType = "CarimboPlatform";
	public int roundingGrid = 2;
	public bool freezeOverlayMovement = true;
	public Node2D newCarimboOverlay;

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

	public void UpdateLabels(bool f = false)
	{
		for (int i = 0; i < 7; i++)
		{
			allTheThings.GetChild<HBoxContainer>(i).GetChild<Label>(0).Text = levelInfo.carimboAmounts[i] + "x";	
			if (f && levelInfo.carimboAmounts[i] == 0)
			{
				allTheThings.GetChild<HBoxContainer>(i).Visible = false;
			}
		}
	}

    public override void _Process(double delta)
    {
		
		for (int i = 1; i <= 7; i++)
		{
			if (Input.IsActionJustPressed("karimbo_slot" + i))
			{
				if (carimboType == "CarimboPlatform" && i != 1)
				{
					newCarimboOverlay.GetChild<Area2D>(1).CollisionMask = 16;
				}
				carimboType = Carimbo.GetCarimboByAction("karimbo_slot" + i);
				if (i == 1)
				{
					newCarimboOverlay.GetChild<Area2D>(1).CollisionMask = 17;
				}
			}
		}

		if (freezeOverlayMovement)
		{
			return;
		}

		newCarimboOverlay.Position = roundToMultiple(GetViewport().GetMousePosition(), roundingGrid);

		if (newCarimboOverlay.GetChild<Area2D>(1).HasOverlappingAreas() || newCarimboOverlay.GetChild<Area2D>(1).HasOverlappingBodies())
		{
			newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayNotOk;
			
		} else
		{
			newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayOk;
		}

		
		if (Input.IsActionJustPressed("rotate"))
		{
			if (newCarimboOverlay.Rotation == 0)
			{
				newCarimboOverlay.Rotation = Mathf.DegToRad(270);
			} else
			{
				newCarimboOverlay.Rotation = 0;
			}	
		}

		if (Input.IsActionJustPressed("mouse_1"))
		{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			SpawnKarimbo();
			freezeOverlayMovement = true;
		}
    }

	public async Task SpawnKarimbo()
	{
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
		if (levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)] > 0 && carimboType.Equals("CarimboDelete"))
		{
			for (int i = 0; i < newCarimboOverlay.GetChild<Area2D>(1).GetOverlappingAreas().Count; i++)
			{
				newCarimboOverlay.GetChild<Area2D>(1).GetOverlappingAreas()[i].GetParent().QueueFree();	
			}
			levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)]--;
			UpdateLabels();	
		} 
		else if (levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)] > 0 && !(newCarimboOverlay.GetChild<Area2D>(1).HasOverlappingAreas() || newCarimboOverlay.GetChild<Area2D>(1).HasOverlappingBodies()))
		{
			Node2D new_carimbo = carimbo.Instantiate<Node2D>();
			AddChild(new_carimbo);
			carimboArray[carimboArrayCounter] = new_carimbo;
			carimboArray[carimboArrayCounter].Position = newCarimboOverlay.Position;
			carimboArray[carimboArrayCounter].Rotation = newCarimboOverlay.Rotation;
			carimboArrayCounter++;
			levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)]--;
			UpdateLabels();	
		}
		freezeOverlayMovement = false;
	}

	public void Start()
	{
		newCarimboOverlay = carimboOverlay.Instantiate<Node2D>();
		AddChild(newCarimboOverlay);

		if (carimboType == "CarimboPlatform")
		{
			newCarimboOverlay.GetChild<Area2D>(1).CollisionMask = 17;
		}

		freezeOverlayMovement = false;
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
		
		

		for (int i = 0; i < levelInfo.carimboAmounts.Length; i++)
		{
			carimboTotalAmount += levelInfo.carimboAmounts[i];
		}

		carimboArray = new Node2D[carimboTotalAmount];
		UpdateLabels(true);
	}
}