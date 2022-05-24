using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RunePuzzleManager : MonoBehaviour {
    [SerializeField] private pushBlock block;
    [SerializeField] private pushWayPoint startWaypoint;
    [SerializeField] private UnityEvent onFinishEvent;

    [HideInInspector] public List<RunePlate> runesList = new List<RunePlate>();

    private void Start() => ResetRune();
    

    private void Update() {
        if (!CardsController.instance.windCardUnlock) forPlaytestBuild();
        if (!CardsController.instance.iceCardUnlock) forPlaytestBuild();
    }

    
    /// <summary>
    /// Method for playtest only
    /// </summary>
    void forPlaytestBuild() {
        CardsController.instance.fireCardUnlock = CardsController.instance.iceCardUnlock = CardsController.instance.windCardUnlock = true;
        UIManager.Instance.UpdateCardUI();
    }

    /// <summary>
    /// Method to call when a rune is updated
    /// </summary>
    public void CheckRunes() {
        if (runesList.Any(rune => !rune.IsActivate)) return;
        onFinishEvent.Invoke();
    }

    /// <summary>
    /// Reset all the runes
    /// </summary>
    public void ResetRune() {
        foreach (var rune in runesList) {
            rune.IsActivate = false;
        }

        block.transform.position = startWaypoint.transform.position;
        block.currentWaypoint = startWaypoint;
    }
}