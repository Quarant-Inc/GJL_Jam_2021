using UnityEngine;


[System.Serializable]
public abstract class Item
{
    public GameObject itemPrefab;
    public ItemType type;
    public string name;

    public abstract void Use();

    public static bool operator==(Item item1, Item item2)
    {
        Debug.LogFormat("item1: {0}; item2: {1};", item1, item2);
        bool item1Null = object.ReferenceEquals(item1, null);
        bool item2Null = object.ReferenceEquals(item2, null);
        if (item1Null || item2Null)
        {
            return item1Null && item2Null;
        }
        //Debug.LogFormat("item1Name: {0}; item2Name: {1}; item1Type: {2}; item2Type: {3};", item1.name, item2.name, item1.type, item2.type);
        //TODO: Put something better here later on
        return item1.name == item2.name && item1.type == item2.type;
    }
    public static bool operator!=(Item item1, Item item2)
    {
        return !(item1 == item2);
    }
}
/*public class Item : MonoBehaviour
{
    public ItemType type;
}*/