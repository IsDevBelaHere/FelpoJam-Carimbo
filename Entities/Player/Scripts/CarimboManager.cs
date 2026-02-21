using Godot;

public partial class CarimboManager : Node
{
	[Export] public PackedScene carimbo;
	[Export] public PackedScene carimboOverlay;
	[Export] public LevelInfo levelInfo;
	[Export] public Texture2D overlayOk;
	[Export] public Texture2D overlayNotOk;
	public Node2D[] carimboArray;
	public int carimboArrayCounter = 0;
	public int carimboTotalAmount = 0;
	public static CarimboManager instance;
	public string carimboType = "CarimboPlatform";
	public int roundingGrid = 8;
	public Node2D newCarimboOverlay;
	public int delay = -1;
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
		if (!newCarimboOverlay.GetChild<Area2D>(1).HasOverlappingAreas())
		{
			newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayOk;
		} else
		{
			newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayNotOk;
		}
		
		if (Input.IsActionJustPressed("karimbo_slot1"))
		{
			carimboType = Carimbo.GetCarimboByAction("karimbo_slot1");
		}

		if (Input.IsActionJustPressed("karimbo_slot2"))
		{
			carimboType = Carimbo.GetCarimboByAction("karimbo_slot2");
		}

		if (Input.IsActionJustPressed("karimbo_slot3"))
		{
			carimboType = Carimbo.GetCarimboByAction("karimbo_slot3");
		}

		if (Input.IsActionJustPressed("karimbo_slot4"))
		{
			carimboType = Carimbo.GetCarimboByAction("karimbo_slot4");
		}

		if (Input.IsActionJustPressed("karimbo_slot5"))
		{
			carimboType = Carimbo.GetCarimboByAction("karimbo_slot5");
		}

		if (Input.IsActionJustPressed("karimbo_slot6"))
		{
			carimboType = Carimbo.GetCarimboByAction("karimbo_slot6");
		}

		if (Input.IsActionJustPressed("karimbo_slot7"))
		{
			carimboType = Carimbo.GetCarimboByAction("karimbo_slot7");
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
			delay = 3;
		}
    }

	public override void _PhysicsProcess(double delta)
	{
		newCarimboOverlay.Position = roundToMultiple(GetViewport().GetMousePosition(), roundingGrid);
		if (delay > 0)
		{
			delay--;
		} else if (delay == 0)
		{
			if (carimboType.Equals("CarimboDelete"))
			{
				for (int i = 0; i < newCarimboOverlay.GetChild<Area2D>(1).GetOverlappingAreas().Count; i++)
				{
					newCarimboOverlay.GetChild<Area2D>(1).GetOverlappingAreas()[i].GetParent().QueueFree();			
				}
			} 
			else if (levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)] > 0 && newCarimboOverlay.GetChild<Sprite2D>(0).Texture == overlayOk)
			{
				Node2D new_carimbo = carimbo.Instantiate<Node2D>();
				AddChild(new_carimbo);
				carimboArray[carimboArrayCounter] = new_carimbo;
				carimboArray[carimboArrayCounter].Position = newCarimboOverlay.Position;
				carimboArray[carimboArrayCounter].Rotation = newCarimboOverlay.Rotation;
				carimboArrayCounter++;
				levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)]--; 
			}
			delay = -1;
		}
	}

	public void AreaEnter(Node2D collider)
	{
		newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayNotOk;
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

		newCarimboOverlay = carimboOverlay.Instantiate<Node2D>();
		AddChild(newCarimboOverlay);
		newCarimboOverlay.GetChild<Area2D>(1).AreaEntered += AreaEnter;
	}
}