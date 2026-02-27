using Godot;
using System;

public abstract partial class SavableResource : Resource
{
    public virtual void SaveData(string path)
    {
        ResourceSaver.Save(this, path);
    }
}
