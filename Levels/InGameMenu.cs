using Godot;
using System;

public partial class InGameMenu : Control
{
	[Export] Configs configResource;
	[Export] Container configMenu;

	[ExportGroup("ConfigMenuTopics")]
	[Export] Control videoConfigs;
	[Export] Control audioConfigs;
	[Export] Control keyBindsConfigs;

    public override void _Ready()
    {
		if (FileAccess.FileExists("user://configs.tres"))
		{
			configResource = GD.Load<Configs>("user://configs.tres");
			UpdateConfigMenu();
		}
		else
		{
			UpdateConfigResource();
		}
    }


	public void UpdateConfigResource()
	{
		configResource.resolution = ResolutionByIndex(videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1).Selected);
		configResource.windowMode = GetWindowModeByIndex(videoConfigs.GetChild<Control>(1).GetChild<OptionButton>(1).Selected);
		configResource.masterAudio = (float)audioConfigs.GetChild<Control>(0).GetChild<HSlider>(1).Value;
		configResource.effectsAudio = (float)audioConfigs.GetChild<Control>(1).GetChild<HSlider>(1).Value;
		configResource.musicAudio = (float)audioConfigs.GetChild<Control>(2).GetChild<HSlider>(1).Value;
	}
	public void UpdateConfigMenu()
	{
		videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1).Select(GetIndexByResolution(configResource.resolution));
		videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1).EmitSignal("item_selected",GetIndexByResolution(configResource.resolution));

		videoConfigs.GetChild<Control>(1).GetChild<OptionButton>(1).Select(GetIndexByWindowMode(configResource.windowMode));
		videoConfigs.GetChild<Control>(1).GetChild<OptionButton>(1).EmitSignal("item_selected",GetIndexByWindowMode(configResource.windowMode));

		audioConfigs.GetChild<Control>(0).GetChild<HSlider>(1).Value = configResource.masterAudio;
		audioConfigs.GetChild<Control>(0).GetChild<HSlider>(1).EmitSignal("value_changed",configResource.masterAudio);

		audioConfigs.GetChild<Control>(1).GetChild<HSlider>(1).Value = configResource.effectsAudio;
		audioConfigs.GetChild<Control>(1).GetChild<HSlider>(1).EmitSignal("value_changed",configResource.effectsAudio);

		audioConfigs.GetChild<Control>(2).GetChild<HSlider>(1).Value = configResource.musicAudio;
		audioConfigs.GetChild<Control>(2).GetChild<HSlider>(1).EmitSignal("value_changed",configResource.musicAudio);
	}
    
	public void ButtonUp_Return()
	{
		configMenu.Visible = false;
		Visible = false;
		
		if (CarimboManager.instance.newCarimboOverlay != null)
		{
			CarimboManager.instance.freezeOverlayMovement = false;
			CarimboManager.instance.newCarimboOverlay.Visible = true;
		}
		
		Player.instance.frozen = false;
	}
	public void ButtonUp_GoToMainMenu()
	{
		StaticAudioPlayer.audioPlayer.Stop();
		CallDeferred(MethodName.PerformGoToMainMenu);
	}

	public void ButtonUp_Configs()
	{
		configMenu.Visible =! configMenu.Visible;	
	}
	public void ButtonUp_VideoMenu()
	{
		videoConfigs.Visible = true;
		audioConfigs.Visible = false;
		
	}
	public void ButtonUp_AudioMenu()
	{
		videoConfigs.Visible = false;
		audioConfigs.Visible = true;
		
	}
	public void ButtonUp_KeybindMenu()
	{
		videoConfigs.Visible = false;
		audioConfigs.Visible = false;
	}

	private Vector2I ResolutionByIndex(int index)
	{
		switch (index)
		{
			case 0: 
				return new(2560,1440);
			case 1:
				return new(1920,1080);
			case 2:
				return new(1280,720);
			case 3:
				return new(640,360);
		}
		return new(1920,1080);
	}
	private int GetIndexByResolution(Vector2I resolution)
	{
		Vector2I _0 = new(2560,1440);
		Vector2I _1 = new(1920,1080);
		Vector2I _2 = new(1280,720);
		Vector2I _3 = new(640,360);
		if (resolution == _0)
		{
			return 0;
		}else if (resolution == _1)
		{
			return 1;
		}else if (resolution == _2)
		{
			return 2;
		}else if (resolution == _3)
		{
			return 3;
		}
		return 0;
	}
	public void ItemSelected_Resolution(int index)
	{
		DisplayServer.WindowSetSize(ResolutionByIndex(index));
		UpdateConfigResource();
	}

	public DisplayServer.WindowMode GetWindowModeByIndex(int index)
	{
		return index switch
		{
			0 => DisplayServer.WindowMode.Windowed,
			1 => DisplayServer.WindowMode.Fullscreen,
			2 => DisplayServer.WindowMode.ExclusiveFullscreen,
			_ => DisplayServer.WindowMode.Windowed
		};
	}
	public int GetIndexByWindowMode(DisplayServer.WindowMode mode)
	{
		return mode switch
		{
			DisplayServer.WindowMode.Windowed => 0,
			DisplayServer.WindowMode.Fullscreen => 1,
			DisplayServer.WindowMode.ExclusiveFullscreen => 2,
			_ => 0
		};
	}
	public void ItemSelected_WindowMode(int index)
	{
		OptionButton optionButton = videoConfigs.GetChild<Control>(0).GetChild<OptionButton>(1);
		if (index != 0)
		{
			optionButton.SetItemText(3, GetViewport().GetWindow().Size.X + "x" + GetViewport().GetWindow().Size.Y);
			optionButton.Select(3);
			ItemSelected_Resolution(3);
			optionButton.Disabled = true;
		}
		else
		{
			optionButton.Disabled = false;
			optionButton.SetItemText(3, "640x360");
		}

		DisplayServer.WindowSetMode(GetWindowModeByIndex(index));
	
		UpdateConfigResource();
	}
	public void ValueChanged_BusVolume(float value, string busName)
	{
		int busIndex = AudioServer.GetBusIndex(busName);
		float db = Mathf.LinearToDb(value);

		AudioServer.SetBusVolumeDb(busIndex,db);

		UpdateConfigResource();
	}


	public void PerformGoToMainMenu()
    {
        GetTree().ChangeSceneToFile("res://Levels/MainMenu/Scenes/main_menu.tscn");
    }

}
