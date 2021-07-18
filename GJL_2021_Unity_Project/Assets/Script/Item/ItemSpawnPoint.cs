using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemSpawnPoint : MonoBehaviour
{
    static int count = 0;
    static Dictionary<int, ItemSpawnPoint> instances = new Dictionary<int, ItemSpawnPoint>();
    public static ItemSpawnPoint[] Instances
    {
        get
        {
            return instances.Values.ToArray();
        }
    }
    int id;
    void Awake()
    {
        id = count++;
        instances.Add(id, this);
    }
}