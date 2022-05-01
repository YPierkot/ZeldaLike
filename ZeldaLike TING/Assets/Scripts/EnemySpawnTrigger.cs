using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    public Transform enemiesParent;
    private bool hasSpawned;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject appearFX;

    public void SpawnEnemies()
    {
        StartCoroutine(SpawnPlayerCo());
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

    private IEnumerator SpawnPlayerCo()
    {
        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            Destroy(Instantiate(appearFX, spawnPoints[i].position, Quaternion.identity, enemiesParent), 5f);
        }

        yield return new WaitForSeconds(3f);
        
        for (int i = 0; i < enemiesToSpawn.Count; i++)
        {
            Instantiate(enemiesToSpawn[i], spawnPoints[i].position, Quaternion.identity, enemiesParent);
        }
    }
}
