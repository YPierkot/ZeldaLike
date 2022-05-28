using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WallDeseapear : MonoBehaviour
{

    public void WallDeseapearFct()
    {
        StartCoroutine(WallDeseapearCo());
    }
    
    private IEnumerator WallDeseapearCo()
    {
        yield return new WaitForSeconds(4f);
        transform.DOMove(new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z),
            2F).OnComplete(() => Destroy(gameObject, 0.4f));
        
    }
}
