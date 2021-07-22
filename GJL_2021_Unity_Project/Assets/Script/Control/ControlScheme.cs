using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlScheme
{
    protected string[] controls;
}
public class ControllerControlScheme : ControlScheme
{
    public ControllerControlScheme()
    {
        controls = new string[]
        {
            "RightJoystickHorizontal",
            "RightJoystickVertical",
            "ControllerHorizontal",
            "ControllerVertical",
            "DPad_Horizontal",
            "DPad_Vertical",
            "B_Button",
            "RightTrigger",
            "X_Button"
        };
    }
    public Axis horizontalAimAxis
    {
        get
        {
            Axis x;
            controls[0].IsOfEnumType(out x);
            return x;
        }
        set
        {
            controls[0] = value.ToString();
        }
    }
    public Axis verticalAimAxis
    {
        get
        {
            Axis y;
            controls[1].IsOfEnumType(out y);
            return y;
        }
        set
        {
            controls[1] = value.ToString();
        }
    }
    public Axis horizontalMovementAxis
    {
        get
        {
            Axis x;
            controls[2].IsOfEnumType(out x);
            return x;
        }
        set
        {
            controls[2] = value.ToString();
        }
    }
    public Axis verticalMovementAxis
    {
        get
        {
            Axis y;
            controls[3].IsOfEnumType(out y);
            return y;
        }
        set
        {
            controls[3] = value.ToString();
        }
    }
    public Axis horizontalDpad
    {
        get
        {
            Axis x;
            controls[4].IsOfEnumType(out x);
            return x;
        }
        set
        {
            controls[4] = value.ToString();
        }
    }
    public Axis verticalDpad
    {
        get
        {
            Axis y;
            controls[5].IsOfEnumType(out y);
            return y;
        }
        set
        {
            controls[5] = value.ToString();
        }
    }
    public ControllerButton dashButton
    {
        get
        {
            ControllerButton dash;
            controls[6].IsOfEnumType(out dash);
            return dash;
        }
        set
        {
            controls[6] = value.ToString();
        }
    }
    public ControllerButton throwButton
    {
        get
        {
            ControllerButton throwe;
            controls[7].IsOfEnumType(out throwe);
            return throwe;
        }
        set
        {
            controls[7] = value.ToString();
        }
    }
    public ControllerButton grabButton
    {
        get
        {
            ControllerButton grab;
            controls[8].IsOfEnumType(out grab);
            return grab;
        }
        set
        {
            controls[8] = value.ToString();
        }
    }
}
public class KeyboardControlScheme : ControlScheme
{
    public KeyboardControlScheme()
    {
        controls = new string[]
        {
            KeyCode.Space.ToString(),
            KeyCode.LeftShift.ToString(),
            KeyCode.Mouse0.ToString(),
            KeyCode.S.ToString(),
            KeyCode.DownArrow.ToString(),
            KeyCode.A.ToString(),
            KeyCode.LeftArrow.ToString(),
            KeyCode.D.ToString(),
            KeyCode.RightArrow.ToString(),
            KeyCode.W.ToString(),
            KeyCode.UpArrow.ToString()
        };
    }
    public KeyCode grabButton
    {
        get
        {
            KeyCode grab;
            controls[0].IsOfEnumType(out grab);
            return grab;
        }
        set
        {
            controls[0] = value.ToString();
        }
    }
    public KeyCode dashButton
    {
        get
        {
            KeyCode dash;
            controls[1].IsOfEnumType(out dash);
            return dash;
        }
        set
        {
            controls[1] = value.ToString();
        }
    }
    public KeyCode throwButton
    {
        get
        {
            KeyCode throwe;
            controls[2].IsOfEnumType(out throwe);
            return throwe;
        }
        set
        {
            controls[2] = value.ToString();
        }
    }
    public KeyCode downButton
    {
        get
        {
            KeyCode d;
            controls[3].IsOfEnumType(out d);
            return d;
        }
        set
        {
            controls[3] = value.ToString();
        }
    }
    public KeyCode downArrowButton
    {
        get
        {
            KeyCode d;
            controls[4].IsOfEnumType(out d);
            return d;
        }
        set
        {
            controls[4] = value.ToString();
        }
    }
    public KeyCode leftButton
    {
        get
        {
            KeyCode l;
            controls[5].IsOfEnumType(out l);
            return l;
        }
        set
        {
            controls[5] = value.ToString();
        }
    }
    public KeyCode leftArrowButton
    {
        get
        {
            KeyCode l;
            controls[6].IsOfEnumType(out l);
            return l;
        }
        set
        {
            controls[6] = value.ToString();
        }
    }
    public KeyCode rightButton
    {
        get
        {
            KeyCode r;
            controls[7].IsOfEnumType(out r);
            return r;
        }
        set
        {
            controls[7] = value.ToString();
        }
    }
    public KeyCode rightArrowButton
    {
        get
        {
            KeyCode r;
            controls[8].IsOfEnumType(out r);
            return r;
        }
        set
        {
            controls[8] = value.ToString();
        }
    }
    public KeyCode upButton
    {
        get
        {
            KeyCode u;
            controls[9].IsOfEnumType(out u);
            return u;
        }
        set
        {
            controls[9] = value.ToString();
        }
    }
    public KeyCode upArrowButton
    {
        get
        {
            KeyCode u;
            controls[10].IsOfEnumType(out u);
            return u;
        }
        set
        {
            controls[10] = value.ToString();
        }
    }
}
public class ButcheredKeyboardControlScheme : ControlScheme
{
    public ButcheredKeyboardControlScheme()
    {
        controls = new string[]
        {
            KeyCode.Q.ToString(),
            KeyCode.LeftShift.ToString(),
            KeyCode.E.ToString(),
            KeyCode.S.ToString(),
            KeyCode.A.ToString(),
            KeyCode.D.ToString(),
            KeyCode.W.ToString()
        };
    }
    public KeyCode grabButton
    {
        get
        {
            KeyCode grab;
            controls[0].IsOfEnumType(out grab);
            return grab;
        }
        set
        {
            controls[0] = value.ToString();
        }
    }
    public KeyCode dashButton
    {
        get
        {
            KeyCode dash;
            controls[1].IsOfEnumType(out dash);
            return dash;
        }
        set
        {
            controls[1] = value.ToString();
        }
    }
    public KeyCode throwButton
    {
        get
        {
            KeyCode throwe;
            controls[2].IsOfEnumType(out throwe);
            return throwe;
        }
        set
        {
            controls[2] = value.ToString();
        }
    }
    public KeyCode downButton
    {
        get
        {
            KeyCode d;
            controls[3].IsOfEnumType(out d);
            return d;
        }
        set
        {
            controls[3] = value.ToString();
        }
    }
    public KeyCode leftButton
    {
        get
        {
            KeyCode l;
            controls[4].IsOfEnumType(out l);
            return l;
        }
        set
        {
            controls[4] = value.ToString();
        }
    }
    public KeyCode rightButton
    {
        get
        {
            KeyCode r;
            controls[5].IsOfEnumType(out r);
            return r;
        }
        set
        {
            controls[5] = value.ToString();
        }
    }
    public KeyCode upButton
    {
        get
        {
            KeyCode u;
            controls[6].IsOfEnumType(out u);
            return u;
        }
        set
        {
            controls[6] = value.ToString();
        }
    }
}