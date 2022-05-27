using System;
using UnityEngine;

public class DissolveShader : MonoBehaviour
{
    [SerializeField] private Material material;

    [SerializeField] private float yValue;
    [SerializeField] private float startYValue;
    [SerializeField] private float noiseScale;
    [SerializeField] private float noiseStrength;
    [SerializeField] private float appearSpeed;
    [SerializeField] private float maxY;
    public bool appear;
    private bool appearing = true;
    void OnEnable()
    {
        if (GetComponent<MeshRenderer>() != null)
        {
            material = GetComponent<MeshRenderer>().material;
        }
        else if (GetComponent<SkinnedMeshRenderer>() != null)
        {
            material = GetComponent<SkinnedMeshRenderer>().material;
        }
        if (appearing)
        {
            appear = true;
            yValue = startYValue;
            material.SetFloat("_NoiseScale", noiseScale);
            material.SetFloat("_NoiseStrentgh", noiseStrength);
            material.SetFloat("_CutoffHeight", startYValue);
        }
        
    }
    
    void Update()
    {
        if (appear)
        {
            yValue+= appearSpeed;
            material.SetFloat("_CutoffHeight", yValue);
            if (yValue >= maxY)
            {
                appear = false;
            }
        }

        if (!appearing)
        {
            yValue -= appearSpeed;
            material.SetFloat("_CutoffHeight", yValue);
            if (yValue < startYValue)
            {
                gameObject.SetActive(false);
            }
        }

    }

    private void OnDisable()
    {
        if (appearing)
        {
            appearing = false;
        }
    }
}
