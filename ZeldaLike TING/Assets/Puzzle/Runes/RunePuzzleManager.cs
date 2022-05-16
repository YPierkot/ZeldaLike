using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RunePuzzleManager : MonoBehaviour
{
  public static RunePuzzleManager Instance;

  private void Awake()
  {
    if(Instance == null)Instance = this;
  }

  [SerializeField] private pushBlock block;
  [SerializeField] private pushWayPoint startWaypoint;
  [SerializeField] private UnityEvent onFinishEvent;

  public List<RunePlate> runesList = new List<RunePlate>();

  private void Start()
  {
    ResetRune();
    
  }

  private void Update()
  {
    if(!CardsController.instance.windCardUnlock) forPlaytestBuild();
    if(!CardsController.instance.iceCardUnlock) forPlaytestBuild();
  }

  void forPlaytestBuild()
  {
    CardsController.instance.fireCardUnlock = CardsController.instance.iceCardUnlock = CardsController.instance.windCardUnlock = true;
    UIManager.Instance.UpdateCardUI();
  }

  public void CheckRunes()
  {
    foreach (var rune in runesList)
    {
      if (!rune.isActivate)
      {
        Debug.Log($"{rune.transform.name} is not Activ");
        return;
      }
    }
    onFinishEvent.Invoke();
  }
  
  public void ResetRune()
  {
    foreach (var rune in runesList)
    {
      rune.isActivate = false;
    }

    block.transform.position = startWaypoint.transform.position;
    block.currentWaypoint = startWaypoint;
  }
}
