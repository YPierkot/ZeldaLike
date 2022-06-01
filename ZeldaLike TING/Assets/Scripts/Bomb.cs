using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float explosionRadius = 2;
    [SerializeField] private GameObject explosionDebug;
    public void ExploseBomb()
    {
        //Destroy(Instantiate(explosionDebug, transform.position, Quaternion.identity),2f);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, playerLayer);
        foreach (var player in colliders)
        {
            player.transform.GetComponent<PlayerStat>().TakeDamage(1);
        }
        
        Destroy(gameObject, 0.1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
