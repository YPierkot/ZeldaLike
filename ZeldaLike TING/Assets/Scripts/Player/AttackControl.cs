using UnityEngine;

public class AttackControl : MonoBehaviour
{
    [SerializeField] Controller control;

    public void AttackFinish() //Call In Animations
    {
        control.CheckAttack();
        Debug.Log("AttackFinish");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ennemy"))
        {
            Debug.Log("Touch " + other.name);
        }
    }

   
}
