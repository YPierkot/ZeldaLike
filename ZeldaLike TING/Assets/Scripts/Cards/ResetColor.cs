using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetColor : MonoBehaviour
{

    public IEnumerator ResetObjectColor()
    {
        yield return new WaitForSeconds(1.3f);
        GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
