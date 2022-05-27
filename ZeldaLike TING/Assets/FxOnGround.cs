using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxOnGround : MonoBehaviour
{
    public LayerMask groundLayerMask;
    
    void Update()
    {
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.15f, groundLayerMask)) transform.position = groundHit.point + new Vector3(0, 0.15f, 0);
        else transform.position += new Vector3(0, -0.15f, 0);
    }
}
