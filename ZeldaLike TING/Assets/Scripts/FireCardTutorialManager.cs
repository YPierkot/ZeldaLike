using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireCardTutorialManager : MonoBehaviour
{
    private Queue<DialogueScriptable> dialogueQueue;
    [SerializeField] private List<DialogueScriptable> dialogues;
    public bool canStart;
    [SerializeField] private GameObject lianas;
    private bool lianaSet;

    private void Start()
    {
        dialogueQueue = new Queue<DialogueScriptable>();
        foreach (DialogueScriptable dialogue in dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }
    }

    private void Update()
    {
        if (canStart && !DialogueManager.Instance.isPlayingDialogue)
        {
            int remainingDialogue = dialogueQueue.Count;
            switch (remainingDialogue)
            {
                case 3:
                    DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    break;
                case 2 :
                    switch (lianaSet)
                    {
                        case false:
                            lianaSet = true;
                            lianas.transform.position = Controller.instance.transform.position;
                            break;
                    }
                    lianas.SetActive(true);
                    if (lianas.transform.childCount == 0)
                    {
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    }
                    break;
            }
        }
    }
}
