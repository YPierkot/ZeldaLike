using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FireCardTutorialManager : MonoBehaviour
{
    private Queue<DialogueScriptable> dialogueQueue;
    [SerializeField] private List<DialogueScriptable> dialogues;
    public bool canStart;
    [SerializeField] private GameObject lianas;
    private bool lianaSet;
    [SerializeField] private GameObject puzzle;
    [SerializeField] private EnemySpawnTrigger enemySpawner;
    [SerializeField] private GameObject barrier;
    public bool isFinished;
    private bool spawnedEnemies;
    [SerializeField] private TextMeshProUGUI helpText;
    [TextArea(4, 10)]
    [SerializeField] private List<string> helps;

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
        PlayerStat.instance.life = PlayerStat.instance.lifeMax;
        if (canStart && !DialogueManager.Instance.isPlayingDialogue)
        {
            barrier.SetActive(true);
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
                            helpText.text = helps[0];
                            break;
                    }
                    lianas.SetActive(true);
                    if (lianas.transform.childCount == 0)
                    {
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    }
                    break;
                case 1 :
                    puzzle.SetActive(true);
                    if (puzzle.transform.childCount == 4)
                    {
                        helpText.text = helps[1];
                        puzzle.SetActive(false);
                        DialogueManager.Instance.AssignDialogue(dialogueQueue.Dequeue().dialogue.ToList());
                    }
                    break;
                
                case 0 :
                    if (!spawnedEnemies)
                    {
                        enemySpawner.SpawnEnemies();
                        spawnedEnemies = true;
                    }
                    
                    if (enemySpawner.enemiesParent.childCount == 0)
                    {
                        canStart = false;
                        isFinished = true;
                        barrier.SetActive(false);
                    }
                    break;
                    
            }
        }
    }
}
