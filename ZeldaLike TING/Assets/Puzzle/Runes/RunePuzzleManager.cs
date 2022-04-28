using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RunePuzzleManager : MonoBehaviour
{
  [SerializeField] private pushBlock block;
  [SerializeField] private pushWayPoint startWaypoint;
  [SerializeField] private UnityEvent onFinishEvent;

  [HideInInspector] public List<RunePlate> runesList = new List<RunePlate>();

  private void Start()
  {
    ResetRune();
  }

  void CheckRunes()
  {
    foreach (var rune in runesList)
    {
      if(rune.isActivate == false)return;
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
