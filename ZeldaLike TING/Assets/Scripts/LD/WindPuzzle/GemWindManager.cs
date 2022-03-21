using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GemWindManager : MonoBehaviour
{
    [SerializeField] private List<GemWindPuzzle> gemList;

    private UnityEvent onFinish;

    void Start()
    {
        foreach (GemWindPuzzle gem in gemList)
        {
            gem.gemManager = this;
        }
    }


    public void UpdatePuzzle(GemWindPuzzle part)
    {
        gemList.Remove(part);
        if (gemList.Count == 0)
        {
            onFinish.Invoke();
        }
        
    }
}
