using System;
using AI;
using UnityEngine;

public class BlueCardLongRange : MonoBehaviour
{
    public LayerMask enemyLayerMask;
    public LayerMask groundMask;
    public float effectRadius = 4f;

    public void IceCardLongEffet()
    {
        var iceFX= PoolManager.Instance.PoolInstantiate(PoolManager.Object.fxLongIceCard);
        iceFX.transform.position = transform.position + new Vector3(0, -1.03f, 0);
        
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Collider[] colliders = Physics.OverlapSphere(transform.position, effectRadius);
        foreach (var col in colliders)
        {
            if (col.transform.CompareTag("Interactable")) col.GetComponent<InteracteObject>().Freeze(transform.position);
            else if (col.transform.CompareTag("Ennemy")) col.transform.GetComponent<AI.AbstractAI>().SlowEnemy();
            else if (col.CompareTag("Boss")) col.transform.GetComponentInParent<BossManager>().Freeze();
        }
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponentInParent<Transform>().CompareTag("Player") && !other.isTrigger)
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
