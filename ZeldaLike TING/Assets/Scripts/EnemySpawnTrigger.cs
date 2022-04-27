using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Transform enemiesParent;
    private bool hasSpawned;

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            Instantiate(enemiesToSpawn[i], spawnPoints[i].position, Quaternion.identity, enemiesParent);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;
            SpawnEnemies();
        }
    }
}
