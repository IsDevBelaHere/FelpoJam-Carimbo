using Godot;
using System;

public partial class CarimboSwap : Carimbo
{
    public override void CarimboFunction(bool entering)
    {
        if (entering)
        {
            Player.instance.direction *= -1;
        } 
    }
}
