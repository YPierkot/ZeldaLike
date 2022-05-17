using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogues Management")] public static DialogueManager Instance;
    private Queue<string> sentences;
    private int currentDialogue;
    private float defaultDelay = 2f;
    private int sentenceIndex;
    [SerializeField] private List<DialogueLine> DialogueLines = new List<DialogueLine> { };
    public bool isPlayingDialogue;
    [SerializeField] private int dialogueMistKellTimer = 30;
    private float timeSinceLastDialogue;
    [NonSerialized] public string playerLocation;
    private bool skip;

    [Header("Dialogue Display")] [SerializeField]
    private TextMeshProUGUI dialogueDisplay;

    [SerializeField] private UnityEngine.UI.Image characterEmotion;
    [SerializeField] private Animator cinematicMode;
    public Animator mist;
    public bool isCursed;
    public bool isCinematic = false;

    [Header("Enqueued Dialogue Management")]
    
    public List<DialogueLine> EnqueuedDialogue;
    
    [Header("Dialogues")]
    
    [SerializeField] private List<DialogueScriptable> FirstExploration;
    [SerializeField] private List<DialogueScriptable> WindDungeon;
    [SerializeField] private List<DialogueScriptable> SecondExploration;
    [SerializeField] private List<DialogueScriptable> Dungeon;
    [SerializeField] private DialogueScriptable TestDialogue;

    private void Awake()
    {
        Instance = this;
        sentences = new Queue<string>();
        timeSinceLastDialogue = Time.time;
        characterEmotion.gameObject.SetActive(false);
    }

    private void Update()
    {
        SkipDialogue();
        AutomaticDialogues();
    }

    public void AssignDialogue(List<DialogueLine> dialogue)
    {
        isPlayingDialogue = true;
        if (isPlayingDialogue)
        {
            CancelInvoke("DisplayNextSentence");
        }
        currentDialogue = 0;
        DialogueLines = dialogue;
        StartDialogue(DialogueLines[currentDialogue]);
    }
    public void StartDialogue(DialogueLine dialogueLine)
    {
        characterEmotion.gameObject.SetActive(true);
        if(isCursed) mist.SetTrigger("Appear");
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
            SetCharacterEmotion();
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
        else
        {
            characterEmotion.gameObject.SetActive(false);
            if(isCursed) mist.ResetTrigger("Appear");
            isPlayingDialogue = false;
            timeSinceLastDialogue = Time.time;
            DialogueLines.Clear();
            EnqueuedDialogue.Clear();
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

    private void SkipDialogue()
    {
        if (Input.GetKeyDown(KeyCode.T) && isPlayingDialogue)
        {
            skip = true;
            CancelInvoke("DisplayNextSentence");
            SoundManager.Instance.Interrupt();
            characterEmotion.gameObject.SetActive(false);
            if(isCursed) mist.ResetTrigger("Appear");
            isPlayingDialogue = false;
            timeSinceLastDialogue = Time.time;
            DialogueLines.Clear();
            sentenceIndex = 0;
            dialogueDisplay.text = null;
            if (EnqueuedDialogue.Count != 0)
            {
                DialogueLines = EnqueuedDialogue;
                AssignDialogue(DialogueLines);
            }
            if (isCinematic)
            {
                UIManager.Instance.playerLocation.text = null;
                IsCinematic();
                GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
                Controller.instance.FreezePlayer(false);
            }
        }
    }

    public void IsCinematic()
    {
        if (isCinematic)
        {
            UIManager.Instance.gameObject.SetActive(true);
            cinematicMode.ResetTrigger("IsCinematic");
        }
        else if (!isCinematic)
        {   
            UIManager.Instance.gameObject.SetActive(false);
            cinematicMode.SetTrigger("IsCinematic");
        }
        isCinematic = !isCinematic;
    }

    private void AutomaticDialogues()
    {
        if (!isPlayingDialogue && Time.time >= timeSinceLastDialogue + dialogueMistKellTimer && !GameManager.Instance.isTutorial)
        {
            DialogueScriptable dialogueToPlay;
            switch (playerLocation)
            {
                case "Exploration1":
                    dialogueToPlay = FirstExploration[UnityEngine.Random.Range(0, FirstExploration.Count)];
                    AssignDialogue(dialogueToPlay.dialogue.ToList());
                    FirstExploration.Remove(dialogueToPlay);
                    break;
                case "WindDungeon":
                    dialogueToPlay = WindDungeon[UnityEngine.Random.Range(0, WindDungeon.Count)];
                    AssignDialogue(dialogueToPlay.dialogue.ToList());
                    WindDungeon.Remove(dialogueToPlay);
                    break;
                case "Exploration2":
                    dialogueToPlay = SecondExploration[UnityEngine.Random.Range(0, SecondExploration.Count)];
                    AssignDialogue(dialogueToPlay.dialogue.ToList());
                    SecondExploration.Remove(dialogueToPlay);
                    break;
                case "Dungeon":
                    dialogueToPlay = Dungeon[UnityEngine.Random.Range(0, Dungeon.Count)];
                    AssignDialogue(dialogueToPlay.dialogue.ToList());
                    Dungeon.Remove(dialogueToPlay);
                    break;
                
            }
            
        }
    }
    public IEnumerator CinematicWait(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (skip)
        {
            Debug.Log("La cinématique a été skip");
            skip = false;
        }
        else
        {
            Debug.Log("La cinématique continue");
            IsCinematic();
            UIManager.Instance.playerLocation.text = null;
            GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
            Controller.instance.FreezePlayer(false);
        }
    }
}
