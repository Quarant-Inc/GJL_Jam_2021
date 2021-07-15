using UnityEngine;

[System.Serializable]
public class Weapon : Item
{
    public WEAPON_TYPE weaponType;
    public override void Use()
    {
        throw new System.NotImplementedException();
    }
}