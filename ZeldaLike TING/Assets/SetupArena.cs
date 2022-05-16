using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupArena : MonoBehaviour
{
    private enum ArenaState
    {
        withCardsPhase = 0, withoutCardsPhase
    }

    private ArenaState State = ArenaState.withCardsPhase;
    [SerializeField] private GameObject UICompteurDeCoups;
    [SerializeField] private GameObject LifeUI;
    
    void Start()
    {
        LifeUI.SetActive(false);
    }

    private void StartArena()
    {
        switch (State)
        {
            case ArenaState.withCardsPhase:
                
                break;
            case ArenaState.withoutCardsPhase:
                
                break;
        }
    }
}
