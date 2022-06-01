using UnityEngine;

public class Vine : InteracteObject
{
    [Header("--- VINE")]
    [SerializeField] private Material lianaBurnMaterial;
    [SerializeField] private float burnAmount;
    [SerializeField] private GameObject sparks;
    private Animator animator;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        if (burning)
        {
            if (lianaTouched)
            {
                lianaTouched = false;
                animator.Play("LianaTouched");
            }
            //Debug.Log(mesh.name);

            if (mesh != null)
            {
                sparks.SetActive(true);
                mesh.material = lianaBurnMaterial;
                burnAmount += 0.005f;
                mesh.material.SetFloat("_BurningValue", burnAmount);
                if(burnAmount >= 0.7f) 
                {
                    animator.Play("LianaDestroy");
                }

                if (burnAmount >= 0.8f)
                {
                    sparks.SetActive(false);
                    DestroyGM();
                    
                }
            } 
        }
    }
}
