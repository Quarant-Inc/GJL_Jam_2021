using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    public GameObject[] pickupPrefabs;

    public uint itemQuantity;

    // Start is called before the first frame update
    void Start()
    {
        if (ItemSpawnPoint.Instances.Length > 0)
        {
            for (uint i = 0; i < itemQuantity; i++)
            {
                SpawnItem();
            }
            Player.Instance.ItemPickedUp += ItemPickedUp;
        }
        else
        {
            Debug.LogError("Implement the spawners prefab please xo");
        }
    }

    void ItemPickedUp()
    {
        SpawnItem();
    }

    void SpawnItem()
    {
        Vector3 pos = GetRandomPosition();
        //Vector3 pos = Util.GetRandomNavMeshLocation();
        pos.y += 0.5f;
        GameObject prefab = pickupPrefabs.GetRandomElement();
        GameObject item = Instantiate(prefab,pos, Quaternion.identity);
    }

    Vector3 GetRandomPosition()
    {
        return ItemSpawnPoint.Instances.GetRandomElement().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}