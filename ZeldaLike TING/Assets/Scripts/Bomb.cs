using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float explosionRadius = 2;
    public void ExploseBomb()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, playerLayer);
        foreach (var player in colliders)
        {
            PlayerStat.instance.TakeDamage();
        }
        
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
