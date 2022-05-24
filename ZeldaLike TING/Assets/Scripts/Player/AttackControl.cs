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

            AI.AbstractAI abstractAI = other.gameObject.GetComponent<AI.AbstractAI>();
            
            // Apply Damage && Hit Stun
            abstractAI.LooseHp(playerDamage);
            Destroy(Instantiate(hitFx, (other.GetComponent<Collider>().ClosestPoint(transform.position)), Quaternion.identity), 0.6f);
            if (abstractAI.isHitStun) abstractAI.isHitStun = false;
            abstractAI.ChangeState(AbstractAI.AIStates.hit);
            
        }
        else if (other.transform.CompareTag("Crates"))
        {
            InteracteWithObect interacteWithObect = other.GetComponent<InteracteWithObect>();
            if(interacteWithObect != null) interacteWithObect.InteractWithObject();
            else Debug.LogError("The object you try to destroy doesn't have the script DestructableObject", other.transform);
            
        }
        else if (other.CompareTag("Boss")) other.GetComponentInParent<BossManager>().TakeDamage(playerDamage);
    }
}
