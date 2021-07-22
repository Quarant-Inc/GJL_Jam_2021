using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XInputDotNetPure;

public static class Control
{
    static Dictionary<PlayerNum, DualFrameStateWrapper> states = new Dictionary<PlayerNum, DualFrameStateWrapper>();
    public static float GetAxis(Axis axis)
    {
        foreach (PlayerNum player in Util.GetValues<PlayerNum>())
        {
            float val = GetAxis(axis, player);
            if (val != 0)
            {
                return val;
            }
        }
        return 0;
    }
    public static float GetAxis(Axis axis, PlayerNum player)
    {
        if (states.ContainsKey(player))
        {
            return states[player].GetAxis(axis);
        }
        //Debug.Log(player+" state not being recorded (make sure to add MainScript to scene.");
        return 0;
    }
    public static bool GetButton(ControllerButton cb)
    {
        foreach (PlayerNum player in Util.GetValues<PlayerNum>())
        {
            if (GetButton(cb, player))
            {
                return true;
            }
        }
        return false;
    }
    public static bool GetButton(ControllerButton cb, PlayerNum player)
    {
        if (states.ContainsKey(player))
        {
            return states[player].ButtonPressed(cb);
        }
        return false;
    }
    public static bool GetButtonDown(ControllerButton cb)
    {
        foreach (PlayerNum player in Util.GetValues<PlayerNum>())
        {
            if (GetButtonDown(cb, player))
            {
                return true;
            }
        }
        return false;
    }
    public static bool GetButtonDown(ControllerButton cb, PlayerNum player)
    {
        if (states.ContainsKey(player))
        {
            return states[player].ButtonDown(cb);
        }
        return false;
    }
    public static bool RightTriggerDown(PlayerNum player)
    {
        return TriggerDown(Trigger.RightTrigger, player);
    }
    public static bool TriggerDown(Trigger trigger, PlayerNum player)
    {
        ControllerButton cb;
        if (trigger.IsOfEnumType(out cb))
        {
            return GetButtonDown(cb, player);
        }
        return false;
    }
    public static bool TriggerPressed(Trigger trigger, PlayerNum player)
    {
        ControllerButton cb;
        if (trigger.IsOfEnumType(out cb))
        {
            return GetButton(cb, player);
        }
        return false;
    }
    static void UpdateState(PlayerNum p)
    {
        if (states.ContainsKey(p))
        {
            StateWrapper sw = GamePad.GetState(p.GetPlayerIndex());
            if (sw.IsConnected && sw.PacketNumber != states[p].CurrentPacketNumber)
            {
                states[p].AppendState(sw);
            }
        }
        else
        {
            GamePadState testState = GamePad.GetState(p.GetPlayerIndex());
            bool succeeded = testState.IsConnected;
            //string msg = succeeded ? "success" : "failure";
            //Debug.LogFormat("Connection State to {0} is: {1};", p, msg);
            if (succeeded)
            {
                states.Add(p, new DualFrameStateWrapper(GamePad.GetState(p.GetPlayerIndex())));
            }
        }
    }
    public static void UpdateStates()
    {
        foreach (PlayerNum player in Util.GetValues<PlayerNum>())
        {
            UpdateState(player);
        }
    }
    public static void GetControllers()
    {
        UpdateStates();
        controllers = states.Count;
    }
    static int controllers = 0;
    public static int ControllerCount
    {
        get
        {
            return controllers;
        }
    }
}
public class DualFrameStateWrapper
{
    public uint CurrentPacketNumber
    {
        get
        {
            return currentState.PacketNumber;
        }
    }
    private StateWrapper previousState;
    private StateWrapper currentState;
    public void AppendState(StateWrapper newState)
    {
        previousState = currentState;
        currentState = newState;
    }
    public bool ButtonDown(ControllerButton cb)
    {
        CheckConnection();
        bool currentlyPressed = ButtonPressed(cb);
        bool previouslyPressed = previousState.ButtonPressed(cb);
        return currentlyPressed && !previouslyPressed;
    }
    public bool ButtonPressed(ControllerButton cb)
    {
        CheckConnection();
        return currentState.ButtonPressed(cb);
    }
    void CheckConnection()
    {
        if (!currentState.IsConnected)
        {
            Debug.Log("Connection failed");
        }
    }
    public DualFrameStateWrapper(StateWrapper firstState)
    {
        previousState = currentState = firstState;
    }
    public float GetAxis(Axis axis)
    {
        CheckConnection();
        return currentState.GetAxis(axis);
    }
}
public class StateWrapper
{
    private GamePadState _state;
    public StateWrapper(GamePadState state)
    {
        _state = state;
    }
    public static implicit operator StateWrapper(GamePadState gstate)
    {
        return new StateWrapper(gstate);
    }
    public static implicit operator GamePadState(StateWrapper wrapper)
    {
        return wrapper._state;
    }
    public float GetAxis(Axis axis)
    {
        switch (axis)
        {
            case Axis.ControllerHorizontal:
                return _state.ThumbSticks.Left.X;
            case Axis.ControllerVertical:
                return -_state.ThumbSticks.Left.Y;
            case Axis.RightJoystickHorizontal:
                return _state.ThumbSticks.Right.X;
            case Axis.RightJoystickVertical:
                return -_state.ThumbSticks.Right.Y;
            case Axis.DPad_Horizontal:
                bool left = ButtonPressed(ControllerButton.DPad_Left);
                bool right = ButtonPressed(ControllerButton.DPad_Right);
                return left ? -1 : (right ? 1 : 0);
            case Axis.DPad_Vertical:
                bool down = ButtonPressed(ControllerButton.DPad_Down);
                bool up = ButtonPressed(ControllerButton.DPad_Up);
                return down ? -1 : (up ? 1 : 0);
            case Axis.LeftTrigger:
                return _state.Triggers.Left;
            case Axis.RightTrigger:
                return _state.Triggers.Right;
            default:
                return 0;
        }
    }
    public bool ButtonPressed(ControllerButton cb)
    {
        ControllerButton button;
        if (cb.IsOfEnumType(out button))
        {
            switch (button)
            {
                case ControllerButton.A_Button:
                    return _state.Buttons.A == ButtonState.Pressed;
                case ControllerButton.B_Button:
                    return _state.Buttons.B == ButtonState.Pressed;
                case ControllerButton.X_Button:
                    return _state.Buttons.X == ButtonState.Pressed;
                case ControllerButton.Y_Button:
                    return _state.Buttons.Y == ButtonState.Pressed;
                case ControllerButton.Start_Button:
                    return _state.Buttons.Start == ButtonState.Pressed;
                case ControllerButton.Select_Button:
                    return _state.Buttons.Back == ButtonState.Pressed;
                case ControllerButton.LeftBumper:
                    return _state.Buttons.LeftShoulder == ButtonState.Pressed;
                case ControllerButton.RightBumper:
                    return _state.Buttons.RightShoulder == ButtonState.Pressed;
                case ControllerButton.Left_Joystick_Button:
                    return _state.Buttons.LeftStick == ButtonState.Pressed;
                case ControllerButton.Right_Joystick_Button:
                    return _state.Buttons.RightStick == ButtonState.Pressed;
                default:
                    return false;
            }
        }
        Trigger trigger;
        if (cb.IsOfEnumType(out trigger))
        {
            switch (trigger)
            {
                case Trigger.LeftTrigger:
                    return _state.Triggers.Left > 0;
                case Trigger.RightTrigger:
                    return _state.Triggers.Right > 0;
                default:
                    return false;
            }
        }
        DPad dpadbutton;
        if (cb.IsOfEnumType(out dpadbutton))
        {
            switch (dpadbutton)
            {
                case DPad.DPad_Down:
                    return _state.DPad.Down == ButtonState.Pressed;
                case DPad.DPad_Left:
                    return _state.DPad.Left == ButtonState.Pressed;
                case DPad.DPad_Right:
                    return _state.DPad.Right == ButtonState.Pressed;
                case DPad.DPad_Up:
                    return _state.DPad.Up == ButtonState.Pressed;
                default:
                    return false;
            }
        }
        return false;
    }
    public bool IsConnected
    {
        get
        {
            return _state.IsConnected;
        }
    }
    public uint PacketNumber
    {
        get
        {
            return _state.PacketNumber;
        }
    }
}
public class Axis_Value
{
    private Axis _axis = Axis.DPad_Horizontal;
    public Axis Axis
    {
        get
        {
            return _axis;
        }
    }
    private int _value = 0;
    public int Value
    {
        get
        {
            return _value;
        }
    }
    public Axis_Value(Axis axis, int value)
    {
        _axis = axis;
        _value = value;
    }
}