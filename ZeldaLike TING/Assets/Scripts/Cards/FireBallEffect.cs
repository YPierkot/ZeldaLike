using AI;
using UnityEngine;

public class FireBallEffect : MonoBehaviour
{
    public LayerMask mask; //Ennemy & Interact
    
    public void ActivateRedGroundEffect()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2, mask);
        foreach (var col in colliders)
        {
            switch (col.transform.tag)
            {
                case "Interactable":
                    col.GetComponent<InteracteObject>().OnFireEffect();
                    break;
                
                case "Ennemy":
                    if (col.transform.GetComponent<SwingerAI>())
                    {
                        col.transform.GetComponent<SwingerAI>().LooseHp(2);
                        Debug.Log("OH LE BOSS");
                    }
                    else if (col.transform.GetComponent<KamikazeAI>())
                    {
                        col.transform.GetComponent<KamikazeAI>().LooseHp(2);
                    }
                    else if (col.transform.GetComponent<MageAI>())
                    {
                        col.transform.GetComponent<MageAI>().LooseHp(2);
                    }
                    else if (col.transform.GetComponent<BomberAI>())
                    {
                        col.transform.GetComponent<BomberAI>().LooseHp(2);
                    }
                    break;
            }
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Player"))
        {
            ActivateRedGroundEffect();
        }
    }
}
