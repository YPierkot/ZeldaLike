using UnityEngine;

public class Vine : InteracteObject
{
    [Header("--- VINE")]
    [SerializeField] private Material lianaBurnMaterial;
    [SerializeField] private float burnAmount;
    [SerializeField] private GameObject sparks;

    public void FixedUpdate()
    {
        if (burning)
        {
            if (mesh != null)
            {
                sparks.SetActive(true);
                mesh.material = lianaBurnMaterial;
                burnAmount += 0.005f;
                mesh.material.SetFloat("_BurningValue", burnAmount);
                if(burnAmount >= 0.7f) 
                {
                    mesh.transform.localScale -= Vector3.one*5f;
                }
                if (mesh.transform.localScale.y < 0.2f)
                {
                    sparks.SetActive(false);
                    DestroyGM();
                }
            } 
        }
    }
}
