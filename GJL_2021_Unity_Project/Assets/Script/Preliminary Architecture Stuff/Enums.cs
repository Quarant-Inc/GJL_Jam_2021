using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SCENE
{
    //INTRO = 0,
    MENU,
    OPTIONS,
    MAINGAME
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
    SPEED_INCREASE,
    HEALTH_INCREASE,
    ARMOUR_INCREASE
}

public enum TOOL_TYPE
{
m
}

public enum Axis
{
    DPad_Horizontal,
    DPad_Vertical,
    ControllerHorizontal,
    ControllerVertical,
    RightJoystickHorizontal,
    RightJoystickVertical,
    LeftTrigger,
    RightTrigger
}

public enum ControllerButton
{
    LeftTrigger,
    RightTrigger,
    A_Button,
    B_Button,
    X_Button,
    Y_Button,
    Start_Button,
    Select_Button,
    LeftBumper,
    RightBumper,
    Left_Joystick_Button,
    Right_Joystick_Button,

    DPad_Down,
    DPad_Left,
    DPad_Right,
    DPad_Up
}

public enum PlayerNum
{
    _P1,
    _P2
}

public enum Trigger
{
    LeftTrigger,
    RightTrigger,
}

public enum DPad
{
    DPad_Down,
    DPad_Left,
    DPad_Right,
    DPad_Up
}