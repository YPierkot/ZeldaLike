using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteracteObject
{
    [Header("=== DOOR ===")]
    [SerializeField] private Transform door;
    [Space]
    [SerializeField] private Transform openPosition;
    [SerializeField] private Transform closePosition;
    [Space] 
    [SerializeField] private float speed = 0.3f;

    [SerializeField] private bool canFreeze;
    private bool frozen = false;
    private bool open = false;
    private bool move = false;

    private Vector3 moveToPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (open) door.position = openPosition.position;
        else door.position = closePosition.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (move && !frozen)
        {
            if (Vector3.Distance(door.position, moveToPosition)>speed)
            {
                door.position = Vector3.Lerp(door.position, moveToPosition, speed);
            }
            else
            {
                move = false;
                door.position = moveToPosition;
            }
        } 
    }

    public void Switch()
    {
        open = !open;
        if (open) moveToPosition = openPosition.position;
        else moveToPosition = closePosition.position;

        move = true;
    }

    public override void Freeze(Vector3 cardPos)
    {
        base.Freeze(cardPos);
        frozen = true;
    }

    public override IEnumerator FreezeTimer()
    {
        yield return new WaitForSeconds(freezeTime);
        freezeCollider.SetActive(false);
        mesh.material.color = Color.gray;
        frozen = false;
    }
}
