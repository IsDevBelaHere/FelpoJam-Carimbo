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
        {Levels.level_1,"res://Levels/Level_1/Scenes/Level_1.tscn"},
        {Levels.level_1_2, "res://Levels/Level_1/Scenes/Level_1_2.tscn"},
        {Levels.level_2, "res://Levels/Level_2/Scenes/Level_2.tscn"},
        {Levels.level_2_2, "res://Levels/Level_2/Scenes/Level_2_2.tscn"},
        {Levels.level_3, "res://Levels/Level_3/Scenes/Level_3.tscn"},
        {Levels.level_3_2, "res://Levels/Level_3/Scenes/Level_3_2.tscn"},
        {Levels.level_4, "res://Levels/Level_4/Scenes/Level_4.tscn"},
        {Levels.level_4_2, "res://Levels/Level_4/Scenes/Level_4_2.tscn"},
        {Levels.level_5, "res://Levels/Level_5/Scenes/Level_5.tscn"},
        {Levels.level_5_2, "res://Levels/Level_5/Scenes/Level_5_2.tscn"},
        {Levels.level_6, "res://Levels/Level_6/Scenes/Level_6.tscn"},
        {Levels.level_6_2, "res://Levels/Level_6/Scenes/Level_6_2.tscn"},
    };
}