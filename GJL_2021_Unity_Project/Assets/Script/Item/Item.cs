using UnityEngine;


[System.Serializable]
public abstract class Item
{
    public ItemType type;
    public string name;

    public abstract void Use();
}
/*public class Item : MonoBehaviour
{
    public ItemType type;
}*/