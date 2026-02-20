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
	public bool skipCheck = false;
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

		if (Input.IsActionJustPressed("mouse_1") && carimboArrayCounter < carimboTotalAmount && newCarimboOverlay.GetChild<Sprite2D>(0).Texture == overlayOk)
		{
			Node2D new_carimbo = carimbo.Instantiate<Node2D>();
			AddChild(new_carimbo);
			carimboArray[carimboArrayCounter] = new_carimbo;
			carimboArray[carimboArrayCounter].Position = roundToMultiple(GetViewport().GetMousePosition().Floor(), roundingGrid);
			carimboArrayCounter++;
			skipCheck = true;
			newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayNotOk;
		}
    }

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMove)
		{
			newCarimboOverlay.Position = roundToMultiple(mouseMove.Position, roundingGrid);
		}
	}

	public void AreaEnter(Node2D collider)
	{
		if (skipCheck)
		{
			skipCheck = false;
		} else 
		{
			if (newCarimboOverlay.GetChild<Area2D>(1).GetOverlappingBodies().Count == 0)
			{
				newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayOk;
			} else
			{
				newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayNotOk;
			}
		}
	}

	public void AreaExited(Node2D collider)
	{
		if (skipCheck)
		{
			skipCheck = false;
		} else 
		{
			if (newCarimboOverlay.GetChild<Area2D>(1).GetOverlappingBodies().Count == 0)
			{
				newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayOk;
			} else
			{
				newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayNotOk;
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
		
		for (int i = 0; i < levelInfo.carimboAmounts.Length; i++)
		{
			carimboTotalAmount += levelInfo.carimboAmounts[i];
		}
		carimboArray = new Node2D[carimboTotalAmount];

		newCarimboOverlay = carimboOverlay.Instantiate<Node2D>();
		AddChild(newCarimboOverlay);
		newCarimboOverlay.GetChild<Area2D>(1).AreaEntered += AreaEnter;
		newCarimboOverlay.GetChild<Area2D>(1).AreaExited += AreaExited;
	}
}