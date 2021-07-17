using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    public int enemyQuantity = 50;

    void Start()
    {
        for (int i = 0; i < enemyQuantity; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 pos = Util.GetRandomNavMeshLocation();
        GameObject prefab = enemyPrefabs.GetRandomElement();
        GameObject enemy = Instantiate(prefab);
    }
}