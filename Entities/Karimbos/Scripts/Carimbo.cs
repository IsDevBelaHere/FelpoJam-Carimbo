using Godot;
using System;

public abstract partial class Carimbo : Resource
{
    [Export]
    public virtual string CarimboText {get;set;}
    [Export]
    public virtual Texture2D CarimboImg {get;set;}
    public abstract void CarimboFunction(bool entering);
}
