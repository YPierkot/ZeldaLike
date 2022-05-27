using System;
using UnityEngine;
using UnityEngine.Events;

public class CheckEntrance : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private UnityEvent OnEntrance;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains(name))
        {
            OnEntrance.Invoke();
        }
    }
}
