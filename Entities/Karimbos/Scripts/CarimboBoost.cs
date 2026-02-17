using Godot;
using System;

public partial class CarimboBoost : Carimbo
{
    public override void CarimboFunction(bool entering)
    {
        if (entering)
        {
            Player.instance.speedMultiplier = 1.5f;
        } else
        {
            Player.instance.isExitingBoostKarimbo = true;
        }
    }

}
