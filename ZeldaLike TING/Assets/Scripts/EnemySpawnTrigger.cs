using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    public Transform enemiesParent;
    private bool hasSpawned;
    [SerializeField] private GameObject barrier;

    public void SpawnEnemies()
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
            GameManager.Instance.actualRespawnPoint = transform;
            barrier.SetActive(true);
            hasSpawned = true;
            SpawnEnemies();
        }
    }
}