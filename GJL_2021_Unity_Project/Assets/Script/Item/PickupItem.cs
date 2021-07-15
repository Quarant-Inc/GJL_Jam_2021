using UnityEngine;

public class PickupItem<T> : MonoBehaviour where T : Item
{
    public T itemSpec;
}