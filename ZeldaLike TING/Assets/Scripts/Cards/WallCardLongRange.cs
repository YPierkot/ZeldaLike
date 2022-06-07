using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WallCardLongRange : MonoBehaviour
{
    public GameObject WallLR;
    public LayerMask groundMask;
    public Quaternion wallRotation;
    
    public void WallCardLongEffect()
    {
        float zTransform = transform.position.z;
        float xTransform = transform.position.x;
        float yTransform = transform.position.y;

        Debug.Log("Wall Long Range Launched");
        var wall = Instantiate(WallLR, new Vector3(xTransform, yTransform - 1.5f, zTransform), Quaternion.identity);
        wall.transform.DOMove(new Vector3(xTransform, yTransform, zTransform), 2f);
        wall.transform.rotation = wallRotation;
        wall.GetComponent<WallDeseapear>().WallDeseapearFct();
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<Transform>().CompareTag("Player") && !other.isTrigger)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
    private void OnDestroy()
    {
        CardsController.isWallGround = false;
        CardsController.instance.LaunchCardCD(3);
        UIManager.Instance.UpdateCardUI();
    }
}
