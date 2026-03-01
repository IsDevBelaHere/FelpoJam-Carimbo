using System;
using System.Data;
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
	public int[] keyToCarimboArray = new int[7];
	public int carimboArrayCounter = 0;
	public int carimboTotalAmount = 0;
	public static CarimboManager instance;
	public string carimboType = "CarimboPlatform";
	public int roundingGrid = 2;
	public bool freezeOverlayMovement = true;
	public Node2D newCarimboOverlay;
	public bool succeded = false;
	public bool isOutOfCarimbos = false;
	#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
		int counter = 0;
		for (int i = 0; i < 7; i++)
		{
			allTheThings.GetChild<HBoxContainer>(i).GetChild<Label>(0).Text = levelInfo.carimboAmounts[i] + "x";	
			if (f)
			{
				if (levelInfo.carimboAmounts[i] == 0)
				{
					allTheThings.GetChild<HBoxContainer>(i).Visible = false;
				}
				else
				{
					keyToCarimboArray[counter] = i + 1;
					counter++;
				}
			}
		}
	}

	public void MouseEnter()
	{
		if (!freezeOverlayMovement && !isOutOfCarimbos)
		{
			newCarimboOverlay.Visible = false;	
		}
	}

	public void MouseExited()
	{	
		if (!freezeOverlayMovement && !isOutOfCarimbos)
		{
			newCarimboOverlay.Visible = true;
		}
	}

	public void CarimboSelect(int carimboToSpawn, bool silent = false)
	{
		if ((carimboType == "CarimboPlatform" && carimboToSpawn != 1) || (carimboType == "CarimboSlime" && carimboToSpawn != 5) || (carimboType == "CarimboDelete" && carimboToSpawn != 7))
			{
				newCarimboOverlay.GetChild<Area2D>(1).CollisionMask = 1<<(6-1)|1<<(5-1);
			}

			if (carimboType != Carimbo.GetCarimboByAction("karimbo_slot" + carimboToSpawn) && !silent)
			{
				StaticAudioPlayer.instance.CreatePlaySFX("res://Models/Audios/fx/selecionar carimbo/SelecionarCarimboCURTO.ogg");
			}

			carimboType = Carimbo.GetCarimboByAction("karimbo_slot" + carimboToSpawn);
			
			if (carimboType == "CarimboPlatform" || carimboType == "CarimboSlime")
			{
				newCarimboOverlay.GetChild<Area2D>(1).CollisionMask = 1<<(6-1)|1<<(5-1)|1<<(1-1);
			}

			if (carimboType == "CarimboDelete")
			{
				newCarimboOverlay.GetChild<Area2D>(1).CollisionMask = 1<<(6-1);
			}
	}

    public override void _Process(double delta)
    {
		if (freezeOverlayMovement)
		{
			return;
		}

		for (int i = 0; i < 7; i++)
		{
			if (Input.IsActionJustPressed("karimbo_slot" + (i + 1)))
			{
				if (keyToCarimboArray[i] == 0)
				{
					break;
				}
				CarimboSelect(keyToCarimboArray[i]);
			}
		}

		newCarimboOverlay.Position = roundToMultiple(GetViewport().GetMousePosition(), roundingGrid);
		if (carimboType == "CarimboDelete")
		{
			if (newCarimboOverlay.GetChild<Area2D>(1).HasOverlappingAreas())
			{
				newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayOk;
			} else
			{
				newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayNotOk;
			}
		}
		else
		{
			if (newCarimboOverlay.GetChild<Area2D>(1).HasOverlappingAreas() || newCarimboOverlay.GetChild<Area2D>(1).HasOverlappingBodies())
			{
				newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayNotOk;
			} else
			{
				newCarimboOverlay.GetChild<Sprite2D>(0).Texture = overlayOk;
			}
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
			if (newCarimboOverlay.GetChild<Sprite2D>(0).Texture == overlayOk)
			{
				for (int i = 0; i < newCarimboOverlay.GetChild<Area2D>(1).GetOverlappingAreas().Count; i++)
				{
					newCarimboOverlay.GetChild<Area2D>(1).GetOverlappingAreas()[i].GetParent().QueueFree();	
				}
				
				levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)]--;
				UpdateLabels();	
			}
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
		if (levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)] == 0)
		{
			bool succeded = false;

			for (int i = Carimbo.GetIndexByCarimbo(carimboType); i < 7; i++)
			{
				if (levelInfo.carimboAmounts[i] > 0)
				{
					CarimboSelect(i + 1,true);
					succeded = true;
					break;
				}
			}

			if (!succeded)
			{
				for (int i = 0; i < 7; i++)
				{
					if (levelInfo.carimboAmounts[i] > 0)
					{
						CarimboSelect(i + 1,true);
						break;
					}
				}
			}

			if (levelInfo.carimboAmounts[Carimbo.GetIndexByCarimbo(carimboType)] == 0)
			{
				newCarimboOverlay.Visible = false;
				isOutOfCarimbos = true;
			}
		}
	}

	public void Start()
	{
		newCarimboOverlay = carimboOverlay.Instantiate<Node2D>();
		AddChild(newCarimboOverlay);

		CarimboSelect(keyToCarimboArray[0]);

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