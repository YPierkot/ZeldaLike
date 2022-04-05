using System.Collections;
using UnityEngine;

public class MoveForTest : MonoBehaviour
{
    public int speed;
    public bool canMove;
    public bool goLeft;

    private void Awake()
    {
        canMove = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(GoLeft), 0.1f, 5f);
        InvokeRepeating(nameof(GoRight), 2.5f, 5f);
    }

    private void Update()
    {
        if (!canMove)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else
        {
            if (goLeft)
            {
                GetComponent<Rigidbody>().velocity = Vector3.left * Time.deltaTime * speed;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = Vector3.right * Time.deltaTime * speed;
            }
        }
    }

    public void GoLeft()
    {
        goLeft = true;
    }

    public void GoRight()
    {
        goLeft = false;
    }
    
    public IEnumerator FreezePos()
    {
        canMove = false;
        yield return new WaitForSeconds(2.5f);
        canMove = true;
    }
}
