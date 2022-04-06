using System;
using System.Collections;
using UnityEngine;

public class InteracteObject : MonoBehaviour
{

    protected MeshRenderer mesh;
    private Rigidbody rb;
    private MeshRenderer[] meshChilds;
    
    
    [Header("--- FIRE")] public bool fireAffect;
    [SerializeField] private bool canBurn;
    [SerializeField] private bool burnDestroy;
    [SerializeField] private bool burning;
    [SerializeField] private UnityEngine.Events.UnityEvent onBurn;
    [SerializeField] private UnityEngine.Events.UnityEvent onBurnDestroy;

    
    [Header("--- WIND")] public bool windAffect;
    public bool windThrough;
    [SerializeField] public UnityEngine.Events.UnityEvent onWind;

    [Header("--- ICE")] public bool iceAffect;
    public bool canFreeze;
    [SerializeField] protected GameObject freezeCollider;
    [SerializeField] public float freezeTime;

    [Header("--- MOVEMENT")] public bool moveRestricted;
    [SerializeField] bool moveTop;
    [SerializeField] bool moveLeft;
    [SerializeField] bool moveRight;
    [SerializeField] bool moveBot;
    
    //[Header("--- HEARTH")]
    
    [Space]
    [SerializeField] private LayerMask destroyInteract;

    
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody>() != null) rb = GetComponent<Rigidbody>();
        if (GetComponent<MeshRenderer>() != null) mesh = GetComponent<MeshRenderer>();
        else meshChilds = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRestricted && rb != null)
        {
            if (!moveTop   && rb.velocity.z > 0) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            if (!moveBot   && rb.velocity.z < 0) rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            if (!moveLeft  && rb.velocity.x > 0) rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            if (!moveRight && rb.velocity.x < 0) rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }
        
        if (burning)
        {
            if (mesh != null)
            {
                mesh.material.color = new Color(mesh.material.color.r+1, mesh.material.color.g-1, mesh.material.color.b-1);
                if(mesh.material.color.r >= 255) Destroy(gameObject); 
            }
            else
            {
                int indexDebug = 0;
                foreach (MeshRenderer meshC in meshChilds)
                {
                    indexDebug++;
                    
                    meshC.material.color = new Color(meshC.material.color.r+(1f/255), meshC.material.color.g-(1f/255), meshC.material.color.b-(1f/255));
                    if (meshC.material.color.r >= 1)
                    {
                        DestroyGM();
                        break;
                    }
                }
            }
            onBurn.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        if (moveRestricted)
        {
            Gizmos.color = Color.red;
            if(moveTop) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward);
            
            if(moveLeft) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.right);
            
            if(moveRight) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.right);
            
            if(moveBot) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.forward);
        }
    }

    void DestroyGM()
    {
        if (burning)
        {
            PropageFire();
            onBurnDestroy.Invoke();
        }
        Destroy(gameObject);
    }

    void PropageFire()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f, destroyInteract);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Interactable"))
            {
                col.GetComponent<InteracteObject>().OnFireEffect();
            }
        }
    }
    
    virtual public void OnFireEffect()
    {
//        Debug.Log("OnfireEffect");
        if (fireAffect)
        {
            if (canBurn)
            {
                burning = true;
                //Debug.LogError($"Start color : {meshChilds[0].material.color}" );
            }
        }

    }

    virtual public void Freeze(Vector3 cardPos)
    {
        Debug.Log("Freeze");
        if (canFreeze)
        {
            freezeCollider.transform.gameObject.transform.gameObject.SetActive(true);
            freezeCollider.transform.position = new Vector3(cardPos.x, freezeCollider.transform.position.y, cardPos.z);
            StartCoroutine(FreezeTimer());
        }
    }

    virtual public IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(freezeTime);
        freezeCollider.SetActive(false);
        mesh.material.color = Color.gray;
        
    }
}
