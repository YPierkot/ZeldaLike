using System;
using UnityEngine;

public class BarrierTrigger : MonoBehaviour
{
    [SerializeField] private Transform enemyParent;
    [SerializeField] private GameObject barrier;
    private bool zoneBeaten;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            barrier.SetActive(true);
        }
    }

    private void Update()
    {
        if (enemyParent.childCount == 0 && !zoneBeaten)
        {
            zoneBeaten = true;
            GameManager.Instance.Disable(barrier);
            gameObject.SetActive(false);
        }
    }
}
