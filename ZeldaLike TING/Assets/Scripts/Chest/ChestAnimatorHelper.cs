using UnityEngine;
using UnityEngine.InputSystem.Android;

public class ChestAnimatorHelper : MonoBehaviour {
    [SerializeField] private GameObject chestFX = null;
    [SerializeField] private Transform chestFXSpawn = null;
    [Space]
    [SerializeField] private GameObject chestDrop = null;
    [SerializeField] private Vector2 dropRange = new Vector2(1,1);
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
    public void SpawnObject()
    {
        Dropper dropper = GetComponent<Dropper>();
        for (int i = 0; i < Random.Range((int)dropRange.x, (int)dropRange.y); i++) {
            GameObject ressource = Instantiate(dropper.Loots[0].Item, transform.position, Quaternion.identity);
            if(dropper != null) dropper.Loot();
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
