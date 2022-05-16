using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerEnemies : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemiesPrefab;
    [SerializeField] private GameObject eAppearFX;
    [SerializeField] private bool canSpawn;
    [SerializeField] private Vector2 enemyPerWave;

    [SerializeField] private GameObject[] eGameObject;
    [SerializeField] private Transform[] ePos;
    
    private Material _material;

    private void Start()
    {
        canSpawn = true;
        _material = GetComponent<Material>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ennemy"))
            return;

        if (Controller.instance.inAttack && canSpawn)
        {
            SpawnEnnemis();
            StartCoroutine(ResetSpawnTimer());
        }
    }

    private void SpawnEnnemis()
    {
        if (!canSpawn)
            return;
        
        StartCoroutine(SpawnEnnemisCo());
    }
    
    private IEnumerator ResetSpawnTimer()
    {
        canSpawn = false;
        yield return new WaitForSeconds(0.7f);
        canSpawn = true;
    }

    private IEnumerator SpawnEnnemisCo()
    {
        int alea = (int)Random.Range(enemyPerWave.x, enemyPerWave.y);
        
        eGameObject = new GameObject[(int)enemyPerWave.y];
        ePos = new Transform[(int)enemyPerWave.y];
        
        for (int i = 0; i < alea; i++)
        { 
            eGameObject[i] = enemiesPrefab[Random.Range(0, enemiesPrefab.Length)];
            ePos[i] = spawnPoints[Random.Range(0, spawnPoints.Length)];
        }
        
        for (int i = 0; i < alea; i++)
        {
            Destroy(Instantiate(eAppearFX, ePos[i].position + new Vector3(0,3.3f,0), Quaternion.identity), 5f);
        }
        
        yield return new WaitForSeconds(3.5f);
        
        for (int i = 0; i < alea; i++)
        {
            Instantiate(eGameObject[i], ePos[i].position, Quaternion.identity);
        }
    }
}
