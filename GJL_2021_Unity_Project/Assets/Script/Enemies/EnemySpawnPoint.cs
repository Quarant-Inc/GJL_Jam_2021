using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemySpawnPoint : MonoBehaviour
{
    static int count = 0;
    static Dictionary<int, EnemySpawnPoint> instances = new Dictionary<int, EnemySpawnPoint>();
    public static EnemySpawnPoint[] Instances
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