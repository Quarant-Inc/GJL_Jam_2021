using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SCENE
{
    INTRO = 0,
    MENU = 1,
    OPTIONS = 2,
    MAINGAME = 3
}

public enum DIRECTION
{
    NONE,
    FORWARD,
    BACKWARD,
    LEFT,
    RIGHT
}

public enum PLAYER_ANIM_PARAMS
{
    MOVE,
    STOP_MOVING
}

public enum TAG
{
    Item
}

public enum ItemType
{
    WEAPON,
    POTION,
    TOOL
}