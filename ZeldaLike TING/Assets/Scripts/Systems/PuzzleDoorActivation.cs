using UnityEngine;

public class PuzzleDoorActivation : MonoBehaviour {
    [SerializeField] private int numberOfToObjectToActivate = 0;
    [SerializeField] private Transform enemyParent = null;
    [Space]
    [SerializeField] private GameObject wall = null;
    private int activatedObject = 0;
    private bool checkForParent = false;

    public void ActivateObject() {
        activatedObject++;
        checkForParent = activatedObject >= numberOfToObjectToActivate;
    }

    private void Update() {
        if (!checkForParent) return;
        if (enemyParent.childCount > 0) return;
        Destroy(wall);
        Destroy(gameObject);
    }
}
