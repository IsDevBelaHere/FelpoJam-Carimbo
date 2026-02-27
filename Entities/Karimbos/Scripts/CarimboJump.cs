using Godot;
using System;

public partial class CarimboJump : Carimbo
{
    public override void CarimboFunction(bool entering)
    {
        if (entering)
        {
            Player.instance.isInJumpKarimbo += 1;
        } else
        {
            Player.instance.isInJumpKarimbo -= 1;
        }
    }

}
