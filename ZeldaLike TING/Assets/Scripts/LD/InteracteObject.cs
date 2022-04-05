using System.Collections;
using UnityEngine;

public class InteracteObject : MonoBehaviour
{

    [Header("--- FIRE")] public bool fireAffect;
    [SerializeField] private bool canBurn;
    [SerializeField] private bool burning;
    [SerializeField] private UnityEngine.Events.UnityEvent onBurnDestroy;

    
    [Header("--- WIND")] public bool windAffect;
    public bool windThrough;

    [Header("--- ICE")] public bool iceAffect;
    public bool canFreeze;
    [SerializeField] protected GameObject freezeCollider;
    [SerializeField] public float freezeTime; 
    
    //[Header("--- HEARTH")]
    
    [Space]
    [SerializeField] private LayerMask destroyInteract;

    protected MeshRenderer mesh;
    private MeshRenderer[] meshChilds;
    // Start is called before the first frame update
    void Start()
    {
        onBurnDestroy.Invoke();
        if (GetComponent<MeshRenderer>() != null)
        {
            mesh = GetComponent<MeshRenderer>();
        }
        else meshChilds = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }

    void DestroyGM()
    {
        if (burning)
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
        Destroy(gameObject);
    }
    
    virtual public void OnFireEffect()
    {
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
