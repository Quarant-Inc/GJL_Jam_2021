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
            case POTION_TYPE.SPEED_INCREASE:
            {
                Player.Instance.IncreaseSpeed();
                break;
            }
        }
        Player.Instance.SetAnimation(PLAYER_ANIM_PARAMS.CONSUME);
    }
    public override string ToString()
    {
        return string.Format("Potion of type {0}",potionType);
    }
}