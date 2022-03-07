using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WallCardLongRange : MonoBehaviour
{
    public GameObject WallLR;
    
    public void WallCardLongEffect()
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;

        Debug.Log("Wall Long Range Launched");
        var wall = Instantiate(WallLR, new Vector3(xTransform, yTransform - 1.3f, zTransform), Quaternion.identity);
        wall.transform.DOMove(new Vector3(xTransform, yTransform + .3f, zTransform), 2f);
        Destroy(wall, 4f);
        Destroy(gameObject, 0.2f);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Collider"))
        {
            WallCardLongEffect();
        }
    }
}
