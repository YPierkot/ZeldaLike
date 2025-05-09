using UnityEngine;

public class PuzzleDoorActivation : MonoBehaviour {
    [SerializeField] private int numberOfToObjectToActivate = 0;
    [SerializeField] private Transform enemyParent = null;
    [Space]
    [SerializeField] private GameObject wall = null;

    [SerializeField] private GameObject windCard;
    [SerializeField] private GameObject windChest;
    
    private int activatedObject = 0;
    private bool checkForParent = false;

    public void ActivateObject() {
        activatedObject++;
        checkForParent = activatedObject >= numberOfToObjectToActivate;
    }

    private void Update() {
        if (!checkForParent) return;
        if (enemyParent.childCount > 0) return;
        if (windCard != null)
        {
            windCard.SetActive(true);
        }
        GameManager.Instance.Disable(wall);
        Destroy(gameObject);
    }
}
