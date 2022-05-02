using AI;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    [SerializeField] Controller control;
    private PlayerStat playerStat;
    [SerializeField] public int playerDamage = 1;
    [SerializeField] public float repusleEnnemyForce = 3;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ennemy"))
        {
            Debug.Log("Touch " + other.name);

            if (other.gameObject.GetComponent<SwingerAI>())
            {
                other.gameObject.GetComponent<AI.AbtractAI>().LooseHp(playerDamage);
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
        else if (other.transform.CompareTag("Crates")) {
            if(other.GetComponent<DestructableObject>() != null) other.GetComponent<DestructableObject>().DestroyObject();
            else Debug.LogError("The object you try to destroy doesn't have the script DestructableObject", other.transform);
        }
    }
}
