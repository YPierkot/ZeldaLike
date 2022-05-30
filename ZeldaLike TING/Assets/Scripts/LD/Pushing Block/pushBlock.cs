using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class pushBlock : InteracteObject
{
    public enum Side
    {
        none, top, left, right, bot
    }
    [Header("---PUSH BLOCK")]
    public pushWayPoint currentWaypoint;
    [SerializeField] private float speed;
    [SerializeField] private float distanceThreshold;
    // Start is called before the first frame update

    [SerializeField] private pushWayPoint newWaypoint;
    private bool move;
    public override void Start()
    {
        base.Start();
        transform.position = currentWaypoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFreeze) 
        {
            onFreeze.Invoke();
             mesh.material.SetColor("_Emission_Teinte", Color.blue);
        }
        else if(burning)
        {
             onBurn.Invoke();
             mesh.material.SetColor("_Emission_Teinte", Color.red);
        }
        if (move)
        {
            if (Vector3.Distance(transform.position, newWaypoint.transform.position) <= distanceThreshold)
            {
                move = false;
                transform.position = newWaypoint.transform.position;
                currentWaypoint = newWaypoint;
                pushWayPoint actualNewWaitpoint = newWaypoint;
                newWaypoint = null;
                actualNewWaitpoint.OnBlockEnter.Invoke();
                return;
            }
            
            transform.position = Vector3.Lerp(transform.position, newWaypoint.transform.position, speed / Vector3.Distance(transform.position, newWaypoint.transform.position) * Time.deltaTime);
        }
    }

    public override void OnWindEffect(CardsController card)
    {
        base.OnWindEffect(card);
        if (windAffect)
        {
            
            Vector2 cardPosVector = new Vector2(card.transform.position.x - transform.position.x, card.transform.position.z - transform.position.z).normalized;
            Vector2 intCardPosVector = new Vector2(Mathf.Round(cardPosVector.x),Mathf.Round(cardPosVector.y));
            //Debug.Log($"player:{card.transform.position}, block:{transform.position} goes  {intCardPosVector}, vector.up: {Vector2.up}");
            
            /*float angle = Vector2.Angle(Vector2.right, cardPosVector);
            if (angle < 0) angle = 360 + angle;
            angle -= 90;#1#
            Debug.Log(angle);*/
            if(Vector2.Equals(intCardPosVector, Vector2.right)) MoveWaypoint(Side.left);
            else if(Vector2.Equals(intCardPosVector, Vector2.up)) MoveWaypoint(Side.bot);
            else if(Vector2.Equals(intCardPosVector, Vector2.left)) MoveWaypoint(Side.right);
            else if(Vector2.Equals(intCardPosVector, Vector2.down)) MoveWaypoint(Side.top);
            else
            {
                Debug.Log("No angle side");
            }
        }
    }

    void MoveWaypoint(Side side)
    {
        pushWayPoint waypoint = null;
        switch (side)
        {
            case Side.top: if(currentWaypoint.topPoint != null) waypoint = currentWaypoint.topPoint;
                break;
            
            case Side.left: if(currentWaypoint.leftPoint != null) waypoint = currentWaypoint.leftPoint;
                break;

            case Side.right: if(currentWaypoint.rightPoint != null) waypoint = currentWaypoint.rightPoint;
                break;

            case Side.bot: if(currentWaypoint.botPoint != null) waypoint = currentWaypoint.botPoint;
                break;
        }

        if (waypoint != null)
        {
            newWaypoint = waypoint;
            move = true;
        }
    }


    public void MoveWaytpointGravity() => MoveWaypoint(Side.bot);
}
