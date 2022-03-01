using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    private DialogueLine[] DialogueLines;
    [SerializeField] private TextMeshProUGUI dialogueDisplay;
    private Queue<string> sentences;
    private int currentDialogue;
    private float defaultDelay = 2f;
    private int sentenceIndex;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        sentences = new Queue<string>();
        DialogueLines = new DialogueLine[1];
    }

    public void AssignDialogue(DialogueLine[] dialogue)
    {
        currentDialogue = 0;
        DialogueLines = dialogue;
        StartDialogue(DialogueLines[currentDialogue]);
    }
    public void StartDialogue(DialogueLine dialogueLine)
    {
        sentenceIndex = 0;
        SoundManager.Instance.PlayVoiceline(dialogueLine.voiceLine);
        sentences.Clear();
        Debug.Log("The dialogue started");
        foreach (dialogueProp line in dialogueLine.dialogLines)
        {
            sentences.Enqueue(line.line);
        }
        Invoke("DisplayNextSentence", DialogueLines[currentDialogue].startingDelay);
    }

    public void DisplayNextSentence()
    {
        Debug.Log($"Il reste {sentences.Count} phrases" );

        
        float delay;
        if (sentences.Count <= 0)
        {
            Debug.Log("fin du dialogue");
            EndDialogue();
        }
        else
        {
            string currentLine = sentences.Dequeue();
            dialogueDisplay.text = currentLine;
            if (DialogueLines[currentDialogue].dialogLines[sentenceIndex].delay == 0)
            {
                Debug.Log(DialogueLines[currentDialogue].dialogLines[sentenceIndex].delay);
                delay = defaultDelay;
            }
            else
            {
                Debug.Log(DialogueLines[currentDialogue].dialogLines[sentenceIndex].delay);
                delay = DialogueLines[currentDialogue].dialogLines[sentenceIndex].delay;
            }
            sentenceIndex++;
            Invoke("DisplayNextSentence", delay);
        }
    }

    private void EndDialogue()
    {
        if (currentDialogue != DialogueLines.Length - 1)
        {
            currentDialogue++;
            StartDialogue(DialogueLines[currentDialogue]);
        }
        else
        {
            Debug.Log("fin du dialogue");
            sentenceIndex = 0;
            dialogueDisplay.text = null;
        }
    }
    
}
