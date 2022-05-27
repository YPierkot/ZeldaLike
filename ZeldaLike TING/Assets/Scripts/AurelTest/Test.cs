using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3[] directions;
    public int count;
    public GameObject objd;
    [Range(0.2f, 5)] public float anchorDst = 1f;

    // Update is called once per frame
    void Update()
    {
        Testd();
    }
    

    private void Testd()
    {
        Vector3[] boidsRays = Method();
        for (int i = 0; i < boidsRays.Length; i++)
        {
            Vector3 dir = boidsRays[i];
            Debug.DrawLine(transform.position, dir);
            Destroy(Instantiate(objd, dir, Quaternion.identity), .05f); 
        }
    }
    
    private Vector3[] Method() 
    {
        directions = new Vector3[8];

        directions[0] = transform.position + Vector3.back * anchorDst;
        directions[1] = transform.position + (Vector3.back + Vector3.right).normalized * anchorDst;
        directions[2] = transform.position + Vector3.right * anchorDst;
        directions[3] = transform.position + (Vector3.forward + Vector3.right).normalized * anchorDst;
        directions[4] = transform.position + Vector3.forward * anchorDst;
        directions[5] = transform.position + (Vector3.forward + Vector3.left).normalized * anchorDst;
        directions[6] = transform.position + Vector3.left * anchorDst;
        directions[7] = transform.position + (Vector3.back + Vector3.left).normalized * anchorDst;
        
        
        
        return directions;
    }
}
