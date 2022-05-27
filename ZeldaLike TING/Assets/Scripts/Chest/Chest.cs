using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Chest : MonoBehaviour {
    [SerializeField] private Animator chestAnim = null;
    private bool chestAppear;
    [SerializeField] private bool isCardChest;
    [SerializeField] private GameObject card;
    [SerializeField] private WindCardTutorialManager WindCardTutorialManager;
    [SerializeField] private WallCardTutorialManager WallCardTutorialManager;
    /// <summary>
    /// When the player enter the collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Controller.instance.playerInteraction = Interact;
        }
    }
    
    /// <summary>
    /// When the player exit the collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && Controller.instance.playerInteraction == Interact) {
            Controller.instance.playerInteraction = null;
        }
    }

    /// <summary>
    /// The method for the interaction
    /// </summary>
    private void Interact() {
        chestAnim.SetTrigger("OpenChest");
        if (isCardChest)
        {
            StartCoroutine(LootCard());
        }
        Controller.instance.playerInteraction = null;
        enabled = false;
    }

    private void Update()
    {
        if (chestAppear && transform.localScale.y < 1.5f)
        {
            transform.localScale += Vector3.one/10;
        }
    }

    private void OnEnable()
    {
        chestAppear = true;
    }

    private IEnumerator LootCard()
    {
        yield return new WaitForSeconds(1);
        card.SetActive(true);
        if (WindCardTutorialManager != null)
        {
            WindCardTutorialManager.canStart = true;
        }
        else
        {
            WallCardTutorialManager.canStart = true;
        }
    }
}
