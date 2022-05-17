using AI;
using UnityEngine;

public class BlueCardLongRange : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public LayerMask groundMask;
    public float effectRadius = 4f;
    public GameObject DebugSphere;
    public GameObject iceFX;
    
    public void IceCardLongEffet()
    {
        Destroy(Instantiate(DebugSphere, transform.position, Quaternion.identity), 2f);
        Destroy(Instantiate(iceFX, transform.position + new Vector3(0,-.9f,0), Quaternion.identity),6f);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, effectRadius);
        foreach (var col in colliders)
        {
            if (col.transform.CompareTag("Interactable")) col.GetComponent<InteracteObject>().Freeze(transform.position);
            else if (col.transform.CompareTag("Ennemy")) col.transform.GetComponent<AI.AbtractAI>().FreezeEnnemy();
            else if (col.CompareTag("Boss")) col.transform.GetComponentInParent<BossManager>().Freeze();
        }
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.ToString() == groundMask.ToString() ||!other.transform.CompareTag("Player"))
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void OnDestroy()
    {
        CardsController.isIceGround = false;
        CardsController.instance.LaunchCardCD(2);
        UIManager.Instance.UpdateCardUI();
    }
}
