using DG.Tweening;
using UnityEngine;

public class WallCardLongRange : MonoBehaviour
{
    public GameObject WallLR;
    public LayerMask groundMask;
    
    public void WallCardLongEffect()
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;

        Debug.Log("Wall Long Range Launched");
        var wall = Instantiate(WallLR, new Vector3(xTransform, yTransform - 1.3f, zTransform), Quaternion.identity);
        wall.transform.DOMove(new Vector3(xTransform, yTransform + .3f, zTransform), 2f);
        Destroy(wall, 4f);
        Destroy(gameObject, 0.2f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<Transform>().CompareTag("Player") && !other.isTrigger)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    
    private void OnDestroy()
    {
        CardsController.isWallGround = false;
        CardsController.instance.LaunchCardCD(3);
        UIManager.Instance.UpdateCardUI();
    }
}
