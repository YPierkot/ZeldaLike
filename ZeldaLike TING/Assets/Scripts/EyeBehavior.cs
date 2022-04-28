using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EyeBehavior : MonoBehaviour
{
    public Vector3 playerPos;
    public Vector3 shootPointDir;
    public float moveSpeed;
    public Controller player;
    public bool goRoll;
    public Rigidbody rb;
    public LayerMask groundMask;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        goRoll = false;
        player = Controller.instance;
    }

    private void Start()
    {
        Vector3 newMoveTarget = new Vector3(transform.position.x, transform.position.y - 3f, transform.position.z);
        transform.DOMove(newMoveTarget, 1f);
            
        StartCoroutine(InitEye());
    }

    private void FixedUpdate()
    {
        RaycastHit groundHit;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 0.3f, groundMask)) transform.position = groundHit.point + new Vector3(0, 0.2f, 0);
        else transform.position += new Vector3(0, -0.2f, 0);
    }

    private IEnumerator InitEye()
    {
        yield return new WaitForSeconds(0.6f);
        playerPos = player.transform.position;
        shootPointDir = playerPos - transform.position;
        
        Debug.DrawRay(transform.position, shootPointDir, Color.green, 2f);

        Vector3 newMoveTarget = new Vector3((transform.position.x + shootPointDir.x), playerPos.y + 0.4f, (transform.position.z + shootPointDir.z));
        
        yield return new WaitForSeconds(0.3f);

        transform.DOMove(newMoveTarget, 1.2f).OnComplete(() => Destroy(gameObject));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.GetComponent<PlayerStat>().TakeDamage();
            Destroy(gameObject);
        }
    }
}
