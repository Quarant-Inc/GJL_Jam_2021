using UnityEngine;

[System.Serializable]
public class Potion : Item
{
    public POTION_TYPE potionType;
    public override void Use()
    {
        switch(potionType)
        {
            case POTION_TYPE.MAGNET:
            {
                Player.Instance.IncreasePickupRadius();
                break;
            }
        }
    }
    public override string ToString()
    {
        return string.Format("Potion of type {0}",potionType);
    }
}