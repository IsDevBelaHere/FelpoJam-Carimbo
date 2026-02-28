using Godot;
using System;

public partial class Progress : SavableResource
{
    public string path = "user://progress.tres";
    [Export] public byte[] levels = new byte[6];
}
