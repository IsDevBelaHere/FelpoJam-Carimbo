using Godot;
using System;

public partial class CarimboBoost : Carimbo
{
    public override void CarimboFunction(bool entering)
    {
        if (entering)
        {
            Player.instance.speedMultiplier += Player.instance.sceneSpeedMultiplier;
        } else
        {
            Player.instance.isExitingBoostKarimbo = true;
        }
    }

}
