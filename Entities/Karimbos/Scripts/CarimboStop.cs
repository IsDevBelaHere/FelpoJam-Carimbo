using Godot;
using System;

public partial class CarimboStop : Carimbo
{
    public override void CarimboFunction(bool entering)
    {
        if (entering)
        {
            if (Player.instance.speedMultiplier > 1)
            {
                Player.instance.speedMultiplier = 1f;
            } else
            {
                Player.instance.speedMultiplier = 0f;

            }
            Player.instance.IsEnteringStopKarimbo = true;
        }
    }

}
