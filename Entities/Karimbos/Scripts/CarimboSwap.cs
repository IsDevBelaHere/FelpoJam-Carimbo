using Godot;
using System;

public partial class CarimboSwap : Carimbo
{
    public override void CarimboFunction()
    {
        Player.instance.direction *= -1;
    }

}
