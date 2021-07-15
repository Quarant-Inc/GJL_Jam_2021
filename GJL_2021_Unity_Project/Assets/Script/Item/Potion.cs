using UnityEngine;

[System.Serializable]
public class Potion : Item
{
    public POTION_TYPE potionType;
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}