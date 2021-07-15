using UnityEngine;

[System.Serializable]
public class Tool : Item
{
    public TOOL_TYPE toolType;
    public override void Use()
    {
        switch(toolType)
        {

        }
    }

    public override string ToString()
    {
        return string.Format("Tool of type {0}",toolType);
    }
}