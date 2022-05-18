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
            // Apply Damage
            other.gameObject.GetComponent<AI.AbtractAI>().LooseHp(playerDamage);
        }
        else if (other.transform.CompareTag("Crates")) {
            if(other.GetComponent<DestructableObject>() != null) other.GetComponent<DestructableObject>().DestroyObject();
            else Debug.LogError("The object you try to destroy doesn't have the script DestructableObject", other.transform);
        }
    }
}
