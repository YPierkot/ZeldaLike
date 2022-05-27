using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesToSpawn;
    [SerializeField] private List<Transform> spawnPoints;
    [Space] 
    
    public bool spawnOnEntry;
    public Transform enemiesParent;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject appearFX;
    [Space] [SerializeField] private float timeBetweenSpawn = 0.75f;
    
    private bool hasSpawned;
    
    
    
    public void SpawnEnemies()
    {
        StartCoroutine(SpawnPlayerCo());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned && spawnOnEntry)
        {
            GameManager.Instance.actualRespawnPoint = transform;
            barrier.SetActive(true);
            hasSpawned = true;
            SpawnEnemies();
        }
    }

    private IEnumerator SpawnPlayerCo() {
        for (int i = 0; i < enemiesToSpawn.Count; i++) {
            StartCoroutine(spawnEnnemy(i));
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    /// <summary>
    /// Spawn an enemy
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private IEnumerator spawnEnnemy(int i) {
        Destroy(Instantiate(appearFX, spawnPoints[i].position + new Vector3(0,2.2f,0), Quaternion.identity, enemiesParent), 5f);
        yield return new WaitForSeconds(3f);
        Instantiate(enemiesToSpawn[i], spawnPoints[i].position, Quaternion.identity, enemiesParent);
    }
}
