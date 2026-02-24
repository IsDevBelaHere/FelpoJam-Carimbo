using System;
using System.Collections.Generic;
using Godot;

public enum Levels
{
    none,
    level_1,
    level_1_2,
    level_2,
    level_2_2,
    level_3,
    level_3_2,
    level_4,
    level_4_2,
    level_5,
    level_5_2,
    level_6,
    level_6_2,
}

public class Globals
{
    public static Dictionary<Levels, string> LevelsToScene = new Dictionary<Levels, string>{
        {Levels.level_1,"res://Levels/Level_debug/Scenes/main.tscn"},
    };
}