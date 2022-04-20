using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushWayPoint : MonoBehaviour
{
    public pushWayPoint topPoint;
    public pushWayPoint leftPoint;
    public pushWayPoint rightPoint;
    public pushWayPoint botPoint;
    
    [Header("---EDITOR Options")]
    public bool canSetWaypoint = true;
}
