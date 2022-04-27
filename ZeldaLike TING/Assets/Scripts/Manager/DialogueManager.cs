using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] private List<DialogueLine> DialogueLines = new List<DialogueLine>{};
    [SerializeField] private TextMeshProUGUI dialogueDisplay;
    private Queue<string> sentences;
    private int currentDialogue;
    private float defaultDelay = 2f;
    private int sentenceIndex;
    [SerializeField] private DialogueScriptable tutorialDialogue;
    [SerializeField] private UnityEngine.UI.Image characterEmotion;
    private int lastDialogueStopIndex;
    private int lastSentenceStopIndex;
    [SerializeField] private List<DialogueLine> StoppedDialogue;
    [SerializeField] private List<DialogueScriptable> ThievesLairMS;
    [SerializeField] private List<DialogueScriptable> ClearingRune;
    public bool isPlayingDialogue;
    private int dialogueMistKellTimer = 30;
    private float timeSinceLastDialogue;

    private void Awake()
    {
        Instance = this;
        
        sentences = new Queue<string>();
        timeSinceLastDialogue = Time.time;
        //Tutorial();
    }

    private void Update()
    {
        if (!isPlayingDialogue && Time.time >= timeSinceLastDialogue + dialogueMistKellTimer && ThievesLairMS.Count > 0)
        {
            var dialogueToPlay = ThievesLairMS[UnityEngine.Random.Range(0, ThievesLairMS.Count)];
            AssignDialogue(dialogueToPlay.dialogue.ToList());
            ThievesLairMS.Remove(dialogueToPlay);
        }
    }

    public void AssignDialogue(List<DialogueLine> dialogue)
    {
        isPlayingDialogue = true;
        Debug.Log(DialogueLines.Count);
        if (DialogueLines.Count != 0 && isPlayingDialogue)
        {
            lastDialogueStopIndex = currentDialogue;
            lastSentenceStopIndex = sentenceIndex;
            StoppedDialogue = DialogueLines;
            CancelInvoke("DisplayNextSentence");
        }
        currentDialogue = 0;
        DialogueLines = dialogue;
        StartDialogue(DialogueLines[currentDialogue]);
    }
    public void StartDialogue(DialogueLine dialogueLine)
    {
        sentenceIndex = 0;
        SoundManager.Instance.PlayVoiceline(dialogueLine.voiceLine);
        sentences.Clear();
        foreach (dialogueProp line in dialogueLine.dialogLines)
        {
            sentences.Enqueue(line.line);
        }
        Invoke("DisplayNextSentence", DialogueLines[currentDialogue].startingDelay);
    }

    public void DisplayNextSentence()
    {
        float delay;
        
        if (sentences.Count <= 0)
        {
            EndDialogue();
        }
        else
        {
            string currentLine = sentences.Dequeue();
            dialogueDisplay.text = currentLine;
            //SetCharacterEmotion();
            if (DialogueLines[currentDialogue].dialogLines[sentenceIndex].delay == 0)
            {
                delay = defaultDelay;
            }
            else
            {
                delay = DialogueLines[currentDialogue].dialogLines[sentenceIndex].delay;
            }
            sentenceIndex++;
            Invoke("DisplayNextSentence", delay);
        }
    }

    private void EndDialogue()
    {
        if (currentDialogue != DialogueLines.Count - 1)
        {
            currentDialogue++;
            sentenceIndex = 0;
            StartDialogue(DialogueLines[currentDialogue]);
        }
        else if (StoppedDialogue.Count != 0)
        {
            Debug.Log(StoppedDialogue);
            DialogueLines = StoppedDialogue;
            //StartDialogue(StoppedDialogue[currentDialogue].character.dialogueInterruptions[Range(0, StoppedDialogue[currentDialogue].character.dialogueInterruptions.Length)].dialogue[0]);
            StoppedDialogue.Clear();
            currentDialogue = lastDialogueStopIndex;
            StartDialogue(DialogueLines[currentDialogue]);
        }
        else
        {
            isPlayingDialogue = false;
            timeSinceLastDialogue = Time.time;
            DialogueLines.Clear();
            sentenceIndex = 0;
            dialogueDisplay.text = null;
        }
    }

    private void SetCharacterEmotion()
    {
        switch (DialogueLines[currentDialogue].dialogLines[sentenceIndex].expressions)
        {
            case dialogueProp.Expressions.Angry:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Angry;
                //Debug.Log("Angry");
                break;
            
            case dialogueProp.Expressions.Sad:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Sad;
                //Debug.Log("Sad");
                break;
            
            case dialogueProp.Expressions.Laughing:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Laughing;
                //Debug.Log("Laughing");
                break;
            
            case dialogueProp.Expressions.Confused:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Confused;
                //Debug.Log("Confused");
                break;
            
            case dialogueProp.Expressions.Neutral:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Neutral;
                //Debug.Log("Neutral");
                break;
        }
    }
}
