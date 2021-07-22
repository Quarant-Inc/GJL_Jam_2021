using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public static class ControlUtil
{
    static Dictionary<PlayerNum, XInputDotNetPure.PlayerIndex> numToIndex = new Dictionary<PlayerNum, XInputDotNetPure.PlayerIndex>()
    {
        {PlayerNum._P1,XInputDotNetPure.PlayerIndex.One},
        {PlayerNum._P2,XInputDotNetPure.PlayerIndex.Two}
    };
    public static XInputDotNetPure.PlayerIndex GetPlayerIndex(this PlayerNum playerNum)
    {
        return numToIndex[playerNum];
    }
}