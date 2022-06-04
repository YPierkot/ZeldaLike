using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(BossManager))]
public class BossManagerEditor : Editor
{
   public override void OnInspectorGUI()
   {
      BossManager script = (BossManager)target;
      script.TransformTP_Zone = script.transform.GetChild(1);
      script.TransformTP_Zone.position = new Vector3(script.TransformTP_Zone.position.x, script.groundY(), script.TransformTP_Zone.position.y);
      
      base.OnInspectorGUI();
   }
}
