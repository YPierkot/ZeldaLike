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
    [SerializeField] private bool canSpawn;

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
            Debug.Log("ColColCol");
            SpawnEnnemis();
            StartCoroutine(ResetSpawnTimer());
            _material.DOColor(Color.red, 0.1f).OnComplete(() => _material.DOColor(Color.white, 0.1f));
            _material.DOFade(0.25f, 0.1f).OnComplete(()=> _material.DOFade(1, 0.1f));
        }
        
        Debug.Log("Collision");
    }

    private void SpawnEnnemis()
    {
        if (!canSpawn)
            return;

        int alea = Random.Range(4, 10);
        for (int i = 0; i < alea; i++)
        {
            GameObject eGameObject = enemiesPrefab[Random.Range(0, enemiesPrefab.Length)];
            Transform ePos = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(eGameObject, ePos.position, Quaternion.identity);
        }
    }


    private IEnumerator ResetSpawnTimer()
    {
        canSpawn = false;
        yield return new WaitForSeconds(0.7f);
        canSpawn = true;
    }
}
