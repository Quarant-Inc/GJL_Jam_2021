using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    public int enemyQuantity = 50;

    void Start()
    {
        if (EnemySpawnPoint.Instances.Length > 0)
        {
            for (int i = 0; i < enemyQuantity; i++)
            {
                SpawnEnemy();
            }
            
            Enemy.EnemyKilled += SpawnEnemy;
        }
        else
        {
            Debug.LogError("Implement the spawners prefab please xo");
        }
    }

    void SpawnEnemy()
    {
        Vector3 pos = GetRandomPosition();
        //Vector3 pos = Util.GetRandomNavMeshLocation();
        GameObject prefab = enemyPrefabs.GetRandomElement();
        GameObject enemy = Instantiate(prefab,pos,Quaternion.identity);
        enemy.SetActive(true);
    }

    Vector3 GetRandomPosition()
    {
        return EnemySpawnPoint.Instances.GetRandomElement().transform.position;
    }
}