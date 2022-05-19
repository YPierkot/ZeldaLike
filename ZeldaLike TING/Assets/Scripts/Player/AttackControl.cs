using System.Collections;
using AI;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    [SerializeField] Controller control;
    private PlayerStat playerStat;
    [SerializeField] public int playerDamage = 1;
    [SerializeField] public float repusleEnnemyForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ennemy"))
        {
            Rigidbody eRb = other.GetComponent<Rigidbody>();
            
            // Apply Damage
            other.gameObject.GetComponent<AI.AbtractAI>().LooseHp(playerDamage);
            
            // Trigger Knockback
            eRb.isKinematic = false;
            Vector3 repulse = (transform.position - other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position)).normalized * repusleEnnemyForce;
            eRb.velocity = repulse;
            /* Vector3 differance = eRb.transform.position - transform.position;
            differance = differance.normalized * repusleEnnemyForce;
            eRb.AddForce(differance, ForceMode.Impulse); */
            StartCoroutine(KnockbackCo(eRb));
        }
        else if (other.transform.CompareTag("Crates")) {
            if(other.GetComponent<DestructableObject>() != null) other.GetComponent<DestructableObject>().DestroyObject();
            else Debug.LogError("The object you try to destroy doesn't have the script DestructableObject", other.transform);
        }
        else if (other.CompareTag("Boss")) other.GetComponentInParent<BossManager>().TakeDamage(playerDamage);
    }

    private IEnumerator KnockbackCo(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.25f);
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
