#if  UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class InformationRelation : MonoBehaviour {
    [SerializeField, Tooltip("WHere is the information for the object")] private Transform informationObjectLocation = null;
    [SerializeField, Tooltip("Where is the location of the object")] private Transform objectLocation = null;
    [Space]
    [SerializeField] private Color gizmosColor = new Color(255/255f, 124/255f, 0/255f);
    [SerializeField, Range(1,7)] private float gizmosLineWidth = 5f;

    private void OnDrawGizmos() {
        if (informationObjectLocation == null || objectLocation == null) return;
        if (!CustomLDData.showGizmos || !CustomLDData.showGizmosRelation) return;
        
        Handles.DrawBezier(informationObjectLocation.position, objectLocation.position, informationObjectLocation.position, (objectLocation.position + informationObjectLocation.position) / 2 + Vector3.up * 40, gizmosColor, null, gizmosLineWidth);
    }
}
#endif
