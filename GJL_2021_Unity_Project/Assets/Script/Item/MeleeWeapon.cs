using UnityEngine;

[System.Serializable]
public class MeleeWeapon : Weapon
{
    public float range;
    public float coneAngle;
    public bool penetrative;
    public override void Use()
    {
        switch(weaponType)
        {

        }
    }
}