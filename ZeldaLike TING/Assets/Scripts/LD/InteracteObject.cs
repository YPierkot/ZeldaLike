using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InteracteObject : MonoBehaviour
{
    [SerializeField] private bool canBurn;
    [SerializeField] private LayerMask destroyInteract;

    private bool burning;

    private MeshRenderer mesh;
    private MeshRenderer[] meshChilds;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            mesh = GetComponent<MeshRenderer>();
        }
        else meshChilds = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (burning)
        {
            if (mesh != null)
            {
                mesh.material.color = new Color(mesh.material.color.r+1, mesh.material.color.g-1, mesh.material.color.b-1);
                if(mesh.material.color.r >= 255) Destroy(gameObject); 
            }
            else
            {
                int indexDebug = 0;
                foreach (MeshRenderer meshC in meshChilds)
                {
                    indexDebug++;
                    
                    meshC.material.color = new Color(meshC.material.color.r+(1f/255), meshC.material.color.g-(1f/255), meshC.material.color.b-(1f/255));
                    if (indexDebug == meshChilds.Length) Debug.Log($"Mat : {meshC.material.name}, color :{meshC.material.color}");
                    if (meshC.material.color.r >= 1)
                    {
                        DestroyGM();
                        break;
                    }
                }
            }
        }
    }

    void DestroyGM()
    {
        if (burning)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2, destroyInteract);
            foreach (var col in colliders)
            {
                if (col.CompareTag("Interactable"))
                {
                    col.GetComponent<InteracteObject>().Burn();
                }
            }
        }
        Destroy(gameObject);
    }
    
    public void Burn()
    {
        if (canBurn)
        {
            burning = true;
            //Debug.LogError($"Start color : {meshChilds[0].material.color}" );
        }
        
    }
}