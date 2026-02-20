using System;
using System.Collections.Generic;
using Godot;

public enum Levels
{
    none,
    level_1,
    level_2,
    level_3,
    level_4,
    level_5,
    level_6
}

public class Globals
{
    public static Dictionary<Levels, string> LevelsToScene = new Dictionary<Levels, string>{
        {Levels.level_1,"res://Levels/Level_debug/Scenes/main.tscn"},
    };
}