using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public static class GameObjectOrderHierarchy {
    
    #region ParentReorder
    /// <summary>
    /// Reorder the child inside of a gameObject based on their names
    /// </summary>
    [MenuItem("Level Design/Scene Editor/Reorder Childs #i")]
    public static void ReorderChildsInsideparent() {
        if (Selection.activeGameObject == null || Selection.activeGameObject.transform.childCount <= 1) return;

        //Run the code foreach gameObject selected
        foreach (GameObject gamSelected in Selection.gameObjects) {
            int numberOfChild = gamSelected.transform.childCount;
            
            //Get all the child of the object
            List<GameObject> childGamList = new List<GameObject>();
            for (int i = 0; i < numberOfChild; i++) {
                childGamList.Add(gamSelected.transform.GetChild(0).gameObject);
                gamSelected.transform.GetChild(0).transform.SetParent(null);
            }

            ReOrderChildsList(childGamList, gamSelected.transform, true);
        }
    }
    
    /// <summary>
    /// Reorder the child inside of a gameObject based on the inverse of their names
    /// </summary>
    [MenuItem("Level Design/Scene Editor/Reorder Childs Invert #o")]
    public static void ReorderChildsInsideparentReverse() {
        if (Selection.activeGameObject == null || Selection.activeGameObject.transform.childCount <= 1) return;

        //Run the code foreach gameObject selected
        foreach (GameObject gamSelected in Selection.gameObjects) {
            int numberOfChild = gamSelected.transform.childCount;
            
            //Get all the child of the object
            List<GameObject> childGamList = new List<GameObject>();
            for (int i = 0; i < numberOfChild; i++) {
                childGamList.Add(gamSelected.transform.GetChild(0).gameObject);
                gamSelected.transform.GetChild(0).transform.SetParent(null);
            }
        
            ReOrderChildsList(childGamList, gamSelected.transform, false);
        }
    }

    /// <summary>
    /// Reorder a list of gameObject inside of a parent
    /// </summary>
    /// <param name="childList"></param>
    /// <param name="parent"></param>
    /// <param name="byNameOrder"></param>
    private static void ReOrderChildsList(List<GameObject> childList, Transform parent, bool byNameOrder) {
        //Reorder List
        List<GameObject> newChildList = byNameOrder ? childList.OrderBy(p => p.name).ToList() : childList.OrderBy(p => p.name).Reverse().ToList();
        
        //Reorder child inside selected gam
        foreach (GameObject child in newChildList) {
            child.transform.SetParent(parent);
        }
    }
    #endregion ParentReorder
    
    #region ChildOrder
    /// <summary>
    /// Reorder all the selected gameObjects inside new parents based on their names
    /// </summary>
    [MenuItem("Level Design/Scene Editor/Arrange Child Parents and order #p")]
    public static void ArrangeChildInsideGam() {
        if (Selection.activeGameObject == null || Selection.gameObjects.Length <= 1) return;

        //Get the list of selected gameObjects
        List<GameObject> selectedGam = Selection.gameObjects.OrderBy(p => p.name).ToList();
        
        //Order gams, create parents and reorder them
        List<GameObject> actualNameList = new List<GameObject>();
        string gamName = selectedGam[0].name.Split(' ')[0].ToUpper();
        
        foreach (GameObject gam in selectedGam) {
            if ((gam.name.Split(' ')[0].ToUpper() == gamName || gam == selectedGam[0]) && gam != selectedGam[^1]) {
                actualNameList.Add(gam);
            }
            else if (gam == selectedGam[^1]) {
                actualNameList.Add(gam);
                CreateParentGamAndStartReOrder(gamName, actualNameList);

                actualNameList.Clear();
            }
            else {
                CreateParentGamAndStartReOrder(gamName, actualNameList);
               
                actualNameList.Clear();
                actualNameList.Add(gam);
                gamName = gam.name.Split(' ')[0].ToUpper();
            }
        }
    }

    /// <summary>
    /// Create a parent, set all child inside and order them
    /// </summary>
    /// <param name="gamName"></param>
    /// <param name="actualNameList"></param>
    private static void CreateParentGamAndStartReOrder(string gamName, List<GameObject> actualNameList) {
        GameObject parentGam = new GameObject();
        Vector3 centerPos = GetCenterOfAllGameObject(actualNameList);
        parentGam.transform.position = centerPos;
        
        if (Selection.activeGameObject.transform.parent != null) parentGam.transform.parent = Selection.activeGameObject.transform.parent;
        parentGam.name = gamName;
        if(actualNameList.Count >= 1) ReOrderChildsList(actualNameList, parentGam.transform, true);
    }
    
    /// <summary>
    /// Get the center of all the childs inside the list
    /// </summary>
    /// <param name="childList"></param>
    /// <returns></returns>
    private static Vector3 GetCenterOfAllGameObject(List<GameObject> childList) {
        Vector3 pos = childList.Aggregate(Vector3.zero, (current, gam) => current + gam.transform.position);
        return pos / childList.Count;
    }
    
    #endregion ChildOrder
}
