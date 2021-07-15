using UnityEngine;

[System.Serializable]
public class Tool : Item
{
    public TOOL_TYPE toolType;
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}