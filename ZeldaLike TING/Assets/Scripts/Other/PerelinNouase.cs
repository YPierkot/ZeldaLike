using UnityEngine;
using Random = UnityEngine.Random;

public class PerelinNouase : MonoBehaviour
{
    private GameObject treePref;
    [SerializeField] private Vector2 perlingSize;
    [SerializeField] private float detailSize = 1;
    
// Start is called before the first frame update
    void Start()
    {
        //Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            float x = Random.Range(0f, 100000f);
            float y = Random.Range(0f, 100000f);
            //Debug.Log($"x: {perlingSize.x}, y: {perlingSize.y}, perlinValue: {Mathf.PerlinNoise(perlingSize.x,perlingSize.y)}");
            Debug.Log($"x: {x}, y: {y}, perlinValue: {Mathf.PerlinNoise(x, y)}");
        }
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(perlingSize.x, 0, perlingSize.y));
    }

    void Generate()
    {
        for (float x = 0; x < perlingSize.x; x += detailSize)
        {
            for (float y = 0; y < perlingSize.y; y += detailSize)
            {
            
            }
        }
    }
}
