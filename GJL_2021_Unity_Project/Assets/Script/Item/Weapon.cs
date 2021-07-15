using UnityEngine;

[System.Serializable]
public abstract class Weapon : Item
{
    public WEAPON_TYPE weaponType;

    public override string ToString()
    {
        return string.Format("Weapon of type {0}", weaponType);
    }
}