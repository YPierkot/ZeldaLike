using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RunePuzzleManager : MonoBehaviour {
    [SerializeField] private pushBlock block;
    [SerializeField] private pushWayPoint startWaypoint;
    [SerializeField] private UnityEvent onFinishEvent;
    [SerializeField] private Material iceDissolve;
    [SerializeField] private Material fireDissolve;
    public List<RunePlate> runesList = new List<RunePlate>();
    private float dissolveAmount;

    private void Start() => ResetRune();
    

    private void Update() {
        //if (!CardsController.instance.windCardUnlock) forPlaytestBuild();
        //if (!CardsController.instance.iceCardUnlock) forPlaytestBuild();
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
    public void CheckRunes(){
        Debug.Log("Je check les runes");
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

    public void PuzzleDisappear()
    {
        dissolveAmount = 19;
        foreach (var rune in runesList)
        {
            if (rune.plateType == RunePlate.Element.Fire)
            {
                rune.mesh.material = fireDissolve;
                rune.mesh.material.SetFloat("_CutoffHeight", dissolveAmount);
            }
            else
            {
                rune.mesh.material = iceDissolve;
                rune.mesh.material.SetFloat("_CutoffHeight", dissolveAmount);
            }
        }
    }

    public void RunesDisappear()
    {
        foreach (var rune in runesList)
        {
            rune.mesh.material.SetFloat("_CutoffHeight", dissolveAmount);
        }

        dissolveAmount -= 0.01f;
    }
}