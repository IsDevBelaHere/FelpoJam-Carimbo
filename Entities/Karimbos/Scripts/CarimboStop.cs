using Godot;
using System;

public partial class CarimboStop : Carimbo
{
    public override void CarimboFunction(bool entering)
    {
        if (entering)
        {
            Player.instance.isEnteringStopKarimbo = true;
        }
    }

}
