using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WallSR : MonoBehaviour
{
    public GameObject ParentObj;
    public Vector3 t_parentobj;
    
    void Start()
    {
        StartCoroutine(ResetObjPos());
        t_parentobj = ParentObj.transform.position;
    }

    public IEnumerator ResetObjPos()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        gameObject.transform.position =
            new Vector3(0,-2.07f, 0);
    }
}
