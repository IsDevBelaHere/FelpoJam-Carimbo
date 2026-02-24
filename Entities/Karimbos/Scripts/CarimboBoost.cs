using Godot;
using System;

public partial class CarimboBoost : Carimbo
{
    public override void CarimboFunction(bool entering)
    {
        if (entering)
        {
            Player.instance.speedMultiplier = LevelStart.instance.playerSpeedMultiplier + 0.5f;
        } else
        {
            Player.instance.isExitingBoostKarimbo = true;
        }
    }

}
