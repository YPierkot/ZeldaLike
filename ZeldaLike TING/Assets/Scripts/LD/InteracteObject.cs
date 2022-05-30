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
    [SerializeField] private bool isRune;
    [SerializeField] public bool burning;
    [SerializeField] private UnityEngine.Events.UnityEvent onBurn;
    [SerializeField] private UnityEngine.Events.UnityEvent onBurnDestroy;
    public bool lianaTouched;
    [SerializeField] private Material lianaBurning;

    
    [Header("--- WIND")] public bool windAffect;
    public bool windThrough;
    [SerializeField] public UnityEngine.Events.UnityEvent onWind;

    [Header("--- ICE")] public bool iceAffect;
    public bool canFreeze;
    public bool barrier;
    [SerializeField] protected GameObject freezeCollider;
    [SerializeField] public float freezeTime;
    public bool isFreeze;

    [Header("--- MOVEMENT")] public bool moveRestricted;
    [SerializeField] bool moveTop;
    [SerializeField] bool moveLeft;
    [SerializeField] bool moveRight;
    [SerializeField] bool moveBot;
    
    //[Header("--- HEARTH")]
    
    [Space]
    [SerializeField] private LayerMask destroyInteract;

    
    // Start is called before the first frame update
    public virtual void Start()
    {
        if (GetComponent<Rigidbody>() != null) rb = GetComponent<Rigidbody>();
        if (GetComponent<MeshRenderer>() != null) mesh = GetComponent<MeshRenderer>();
        else meshChilds = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    public virtual void Update()
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
            if (isRune)
            {
                burning = false;
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

    public void DestroyGM()
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
            isFreeze = false;
            if (canBurn)
            {
                lianaTouched = true;
                burning = true;
                //Debug.LogError($"Start color : {meshChilds[0].material.color}" );
            }
        }

    }

    virtual public void OnWindEffect(CardsController card)
    {
        if(windAffect) onWind.Invoke();
    }

    virtual public void Freeze(Vector3 cardPos)
    {
        if (iceAffect)
        {
            isFreeze = true;
            burning = false;
            if (canFreeze)
            {
                freezeCollider.transform.gameObject.transform.gameObject.SetActive(true);
                freezeCollider.transform.position = new Vector3(cardPos.x, freezeCollider.transform.position.y, cardPos.z);
                StartCoroutine(FreezeTimer());
            }

            if (barrier)
            {
                GameManager.Instance.Disable(gameObject);
            }
        }
    }

    virtual public IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(freezeTime);
        freezeCollider.SetActive(false);
        mesh.material.color = Color.gray;
        
    }
}
