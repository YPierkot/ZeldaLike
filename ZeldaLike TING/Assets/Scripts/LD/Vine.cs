using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : InteracteObject
{
    [Header("--- VINE")]
    [SerializeField] private Material lianaBurnMaterial;
    [SerializeField] private float burnAmount;

    public override void Update()
    {
        base.Update();
        if (burning)
        {
            if (mesh != null)
            {
                mesh.material = lianaBurnMaterial;
                burnAmount += 0.009f;
                mesh.material.SetFloat("_BurningValue", burnAmount);
                if(burnAmount >= 1) 
                {
                    mesh.transform.localScale -= Vector3.one;
                }
                if (mesh.transform.localScale.y < 0.2f)
                {
                    DestroyGM();
                }
            } 
        }
        
    }
}
