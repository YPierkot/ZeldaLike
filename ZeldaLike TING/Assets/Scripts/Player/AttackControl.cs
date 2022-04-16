using AI;
using UnityEngine;

public class AttackControl : MonoBehaviour
{
    [SerializeField] Controller control;
    [SerializeField] private int playerDamage;
    private static AttackControl instance;
    
    // Game Instance Singleton
    public static AttackControl Instance
    {
        get
        { 
            return instance; 
        }
    }
    
    private void Awake()
    {
        UpdateDMG();
        
        if (instance != null && instance != this)
            Destroy(this.gameObject);

        instance = this;
    }

    public void AttackFinish() //Call In Animations
    {
        control.CheckAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ennemy"))
        {
            Debug.Log("Touch " + other.name);

            if (other.gameObject.GetComponent<SwingerAI>())
            {
                other.gameObject.GetComponent<SwingerAI>().LooseHp(playerDamage);
            }
            else if (other.gameObject.GetComponent<KamikazeAI>())
            {
                other.gameObject.GetComponent<KamikazeAI>().LooseHp(playerDamage);
            }
            else if (other.gameObject.GetComponent<MageAI>())
            {
                other.gameObject.GetComponent<MageAI>().LooseHp(playerDamage);
            }
            else if (other.gameObject.GetComponent<BomberAI>())
            {
                other.gameObject.GetComponent<BomberAI>().LooseHp(playerDamage);
            }
        }
    }

    public void UpdateDMG()
    {
        playerDamage = control.GetComponent<PlayerStat>().attackDamageValue;
    }
}
