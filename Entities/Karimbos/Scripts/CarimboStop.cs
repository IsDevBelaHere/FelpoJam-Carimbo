using Godot;
using System;

public partial class CarimboStop : Carimbo
{
    public override void CarimboFunction(bool entering)
    {
        if (entering)
        {
            if (Player.instance.speedMultiplier > Player.instance.sceneSpeedMultiplier)
            {
                Player.instance.speedMultiplier = Player.instance.sceneSpeedMultiplier;
            } else
            {
                Player.instance.speedMultiplier = 0f;
                Player.instance.IsEnteringStopKarimbo = true;
            }
        }
    }

}
