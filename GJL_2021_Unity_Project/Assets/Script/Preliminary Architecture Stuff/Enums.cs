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
    RIGHT,
    FORWARD_RIGHT,
    FORWARD_LEFT,
    BACKWARD_RIGHT,
    BACKWARD_LEFT
}

public enum PLAYER_ANIM_PARAMS
{
    MOVE,
    STOP_MOVING,
    PISTOL,
    BOW,
    DAGGER_THROW,
    SWORD,
    THROW,
    CONSUME,
    DASH
}

public enum TAG
{
    Item,
    Enemy,
    Player
}

public enum ItemType
{
    WEAPON,
    POTION,
    TOOL
}

public enum WEAPON_TYPE
{
    MELEE,
    STRAIGHT_RANGED,
    ARC_RANGED
}

public enum POTION_TYPE
{
    MAGNET,
    SPEED_INCREASE
}

public enum TOOL_TYPE
{
m
}