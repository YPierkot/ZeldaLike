using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{

    [SerializeField] private Transform cameraPoint;
    [SerializeField] private GameObject puzzleCube;
    private bool freedAeryn;
    [SerializeField] private List<DialogueScriptable> aerynDialogues;
    private Queue<DialogueScriptable> dialogues;
    public bool startIce;
    [SerializeField] private GameObject iceCard;
    [SerializeField] private GameObject puzzleBounds;
    


    private void Start()
    {
        dialogues = new Queue<DialogueScriptable>();
        foreach (var dialogue in aerynDialogues)
        {
            dialogues.Enqueue(dialogue);
        }
    }

    private IEnumerator OnEntrance()
    {
        //Controller.instance.FreezePlayer(true);
        //DialogueManager.Instance.IsCinematic();
        //GameManager.Instance.cameraController.ChangePoint(cameraPoint);
        yield return new WaitForSeconds(3f);
        puzzleCube.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
        Controller.instance.FreezePlayer(false);
        //DialogueManager.Instance.IsCinematic();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.Instance.isTutorial && other.CompareTag("Player"))
        {
            StartCoroutine(OnEntrance());
        }
    }

    public void FreeingAeryn()
    {
        DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
        DialogueManager.Instance.IsCinematic();
        Controller.instance.FreezePlayer(true);
        freedAeryn = true;
    }

    private void Update()
    {
        switch (dialogues.Count)
        {
            case 6:
                if (freedAeryn && !DialogueManager.Instance.isPlayingDialogue)
                {
                    DialogueManager.Instance.IsCinematic();
                    Controller.instance.FreezePlayer(false);
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                }

                break;
            case 5:
                if (startIce)
                {
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                    iceCard.SetActive(true);
                }
                break;
            case 4 :
                if (CardsController.instance.iceCardUnlock && !DialogueManager.Instance.isPlayingDialogue)
                {
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                }
                break;
            case 3 :
                if (!puzzleBounds.activeSelf)
                {
                    DialogueManager.Instance.AssignDialogue(dialogues.Dequeue().dialogue.ToList());
                }
                break;
            
        }
    }

    public void IceDialogue()
    {
        startIce = true;
    }

    public void ActivateLever()
    {
        puzzleBounds.SetActive(false);
    }
}
