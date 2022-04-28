using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private RaycastHit[] hits;
    [Space]
    public Transform cameraPoint;

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float changePointSpeed;

    public bool changingPoint;
    

    [System.NonSerialized] public bool dashing = false;
    public bool moveToPlayer;

    [Space] [Header("CameraSet")] 
    [SerializeField] private Transform firstSet;
    [SerializeField] private Transform secondSet;
     private bool isFirstSet = true;

    private void Update()
    {
       /*hits = Physics.RaycastAll(transform.position, (player.position - transform.position), Vector3.Distance(player.position, transform.position));
       if (hits.Length != 0)
       {
           foreach (RaycastHit hit in hits)
           {
               if (hit.transform.GetComponent<MeshRenderer>() != null)
               {
                   MeshRenderer mesh = hit.collider.transform.GetComponent<MeshRenderer>();
                   //mesh.color.a = 0.3f;
               }
           }
       }*/
       if (Input.GetKeyDown(KeyCode.L))
       {
           if (isFirstSet)
           {
               transform.position = secondSet.position;
               transform.rotation = secondSet.rotation;
               cameraPoint = secondSet;
               isFirstSet = false;
           }
           else
           {
               transform.position = firstSet.position;
               transform.rotation = firstSet.rotation;
               cameraPoint = firstSet;
               isFirstSet = true;
           }
       }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (changingPoint)
        {
           if(moveToPlayer) transform.position = Vector3.Lerp(transform.position, cameraPoint.position, changePointSpeed*0.1f*(1/Vector3.Distance(transform.position, cameraPoint.position)*15));
           else transform.position = Vector3.Lerp(transform.position, cameraPoint.position, changePointSpeed*0.1f);
            Debug.Log("Slow change");
            
            if (Vector3.Distance(transform.position, cameraPoint.position) < 0.3f)
            {
                changingPoint = false;
            }
        }
        else if (!dashing)
        {
            transform.position = Vector3.Lerp(transform.position, cameraPoint.position, cameraSpeed*0.1f);
        }

    }

    public void ChangePoint(Transform newTransform, bool _moveToPlayer = false)
    {
        changingPoint = true;
        cameraPoint = newTransform;
        moveToPlayer = _moveToPlayer;
    }
}
