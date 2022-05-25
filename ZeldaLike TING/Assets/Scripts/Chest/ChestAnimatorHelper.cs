using UnityEngine;

public class ChestAnimatorHelper : MonoBehaviour {
    [SerializeField] private GameObject chestFX = null;
    [SerializeField] private Transform chestFXSpawn = null;
    [Space]
    [SerializeField] private GameObject chestDrop = null;
    [SerializeField] private float dropNumber = 1;
    [Space]
    [SerializeField] private float throwForce = 1;
    [SerializeField] private float throwRangeDistance = 4f;
    
    /// <summary>
    /// Spawn an FX
    /// </summary>
    public void SpawnFX() => Instantiate(chestFX, chestFXSpawn.position, Quaternion.identity);

    /// <summary>
    /// Spawn drop on the floor
    /// </summary>
    public void SpawnObject() {
        for (int i = 0; i < dropNumber; i++) {
            GameObject ressource = Instantiate(chestDrop, transform.position, Quaternion.identity);
            
            if (ressource.GetComponent<Rigidbody>() == null) return;
            ressource.GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce + GetPointOnCircle(Random.Range(0,360), Random.Range(0,360)), ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Get a point on a circle with a radius of "throwRangeDistance"
    /// </summary>
    /// <param name="rotationX"></param>
    /// <param name="rotationY"></param>
    /// <returns></returns>
    private Vector3 GetPointOnCircle(float rotationX, float rotationY) => new Vector3(throwRangeDistance * Mathf.Sin(rotationX), 0, throwRangeDistance * Mathf.Cos(rotationX));
}
