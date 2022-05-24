using System.Collections;
using AI;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    [SerializeField] Controller control;
    private PlayerStat playerStat;
    [SerializeField] public int playerDamage = 1;
    [SerializeField] public float repusleEnnemyForce;
    [SerializeField] private GameObject hitFx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ennemy"))
        {
            if (other.GetComponent<EyeBehavior>())
            {
                Destroy(other.gameObject);
                return;
            }

            // Apply Damage
            other.gameObject.GetComponent<AI.AbstractAI>().LooseHp(playerDamage);
            Destroy(Instantiate(hitFx, (other.GetComponent<Collider>().ClosestPoint(transform.position)), Quaternion.identity), 0.6f);
            //other.gameObject.GetComponent<AI.AbstractAI>().ChangeState(AbstractAI.AIStates.hit);
        }
        else if (other.transform.CompareTag("Crates")) 
        {
            if(other.GetComponent<InteracteWithObect>() != null) other.GetComponent<InteracteWithObect>().InteractWithObject();
            else Debug.LogError("The object you try to destroy doesn't have the script DestructableObject", other.transform);
            
        }
        else if (other.CompareTag("Boss")) other.GetComponentInParent<BossManager>().TakeDamage(playerDamage);
    }
}
