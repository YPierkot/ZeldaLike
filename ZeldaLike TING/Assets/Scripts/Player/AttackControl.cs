using AI;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    [SerializeField] Controller control;
    [SerializeField] private int playerDamage;
    [SerializeField] private float repusleEnnemyForce;
    
    
    private void Awake()
    {
        UpdateAttackStats();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ennemy"))
        {
            Debug.Log("Touch " + other.name);

            if (other.gameObject.GetComponent<SwingerAI>())
            {
                other.gameObject.GetComponent<SwingerAI>().LooseHp(playerDamage);
                Vector3 repulse = (transform.position - other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position)).normalized * repusleEnnemyForce;
                control.rb.velocity = repulse;
            }
            else if (other.gameObject.GetComponent<KamikazeAI>())
            {
                other.gameObject.GetComponent<KamikazeAI>().LooseHp(playerDamage);
                Vector3 repulse = (transform.position - other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position)).normalized * repusleEnnemyForce;
                control.rb.velocity = repulse;
            }
            else if (other.gameObject.GetComponent<MageAI>())
            {
                other.gameObject.GetComponent<MageAI>().LooseHp(playerDamage);
                Vector3 repulse = (transform.position - other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position)).normalized * repusleEnnemyForce;
                control.rb.velocity = repulse;
            }
            else if (other.gameObject.GetComponent<BomberAI>())
            {
                other.gameObject.GetComponent<BomberAI>().LooseHp(playerDamage);
                Vector3 repulse = (transform.position - other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position)).normalized * repusleEnnemyForce;
                control.rb.velocity = repulse;
            }
        }
    }

    public void UpdateAttackStats()
    {
        PlayerStat playerStat = PlayerStat.instance;
        
        playerDamage = playerStat.attackDamageValue;
        repusleEnnemyForce = playerStat.enemyKBForce;
    }
    
}
