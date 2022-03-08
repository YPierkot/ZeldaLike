using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WallGroundCardEffect : MonoBehaviour
{
    public GameObject WallSR;
    
    public void ActivateWallGroundEffect()
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;

        Debug.Log("Wall Short Range Launched");
        var wall = Instantiate(WallSR, new Vector3(xTransform, yTransform - 1.3f, zTransform), Quaternion.identity);
        wall.transform.DOMove(new Vector3(xTransform, yTransform + .3f, zTransform), 1.8f);
        Destroy(wall, 4f);
        Destroy(gameObject, 0.2f);
    }
}
