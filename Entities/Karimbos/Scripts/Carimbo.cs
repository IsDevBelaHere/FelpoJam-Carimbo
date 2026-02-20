using Godot;
using System;

public abstract partial class Carimbo : Resource
{
    [Export]
    public virtual string CarimboText {get;set;}
    [Export]
    public virtual Texture2D CarimboImg {get;set;}
    public abstract void CarimboFunction(bool entering);

    public static string GetCarimboByAction(string carimbo )
    {
        switch (carimbo)
        {
            case "karimbo_slot1": 
                return "CarimboPlatform";

            case "karimbo_slot2":
                return "CarimboJump" ;

            case "karimbo_slot3":
                return "CarimboSwap" ;

            case "karimbo_slot4":
                return "CarimboBoost" ;

            case "karimbo_slot5":
                return "CarimboSlime" ;

            case "karimbo_slot6":
                return "CarimboStop"  ;

            case "karimbo_slot7":
                return "CarimboDelete";
        }
        return string.Empty;
    }

    public static string GetActionByCarimbo(string action)
    {
        switch (action)
        {
            case "CarimboPlatform":
                return "karimbo_slot1";
                
            case "CarimboJump":
                return "karimbo_slot2";

            case "CarimboSwap":
                return "karimbo_slot3";

            case "CarimboBoost":
                return "karimbo_slot4";

            case "CarimboSlime":
                return "karimbo_slot5";

            case "CarimboStop":
                return "karimbo_slot6";

            case "CarimboDelete":
                return "karimbo_slot7";
        }
        return string.Empty;
    }
}
