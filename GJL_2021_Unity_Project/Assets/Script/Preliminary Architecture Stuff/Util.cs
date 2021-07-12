using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static string NormaliseString(this object obj)
    {
        return obj.ToString().Trim().ToLower();
    }
}
