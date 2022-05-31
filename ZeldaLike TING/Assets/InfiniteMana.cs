using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteMana : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CardsController.instance.cardCooldown = 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CardsController.instance.cardCooldown = 4f;
        }
    }
}
