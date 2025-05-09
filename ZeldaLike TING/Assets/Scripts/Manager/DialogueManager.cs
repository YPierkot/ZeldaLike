using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public bool playerCanMove;
    [SerializeField] private UnityEngine.UI.Image characterEmotion;
    [SerializeField] private UnityEngine.UI.Image frame;
    [SerializeField] private Sprite[] frames;
    [SerializeField] private Animator maskAnimator;
    [SerializeField] private Animator cinematicMode;
    private Animator textAnimator;
    private CharacterScriptable lastCharacter;
    public Animator mist;
    public bool isCursed;
    public bool isCinematic = false;
    public bool dialogueMoved;
    [SerializeField] private Image gray;

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
        textAnimator = characterEmotion.GetComponent<Animator>();
    }

    private void Update()
    {
        SkipDialogue();
        if (!dialogueMoved && !isCinematic)
        {
            if (isPlayingDialogue)
            {
                gray.enabled = true;
                
            }
            else
            {
                gray.enabled = false;
            }
            dialogueMoved = true;
            characterEmotion.rectTransform.anchoredPosition = new Vector2(-1808, -417);
            maskAnimator.GetComponent<RectTransform>().anchoredPosition = new Vector2(-180, -385);
            dialogueDisplay.alignment = TextAlignmentOptions.Left;
            dialogueDisplay.margin = new Vector4(-467, 0, -456, -60);
        }

        if (isCinematic)
        {
            gray.enabled = false;
            characterEmotion.rectTransform.anchoredPosition = new Vector2(-954, -338);
            maskAnimator.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -411);
            dialogueDisplay.alignment = TextAlignmentOptions.Midline;
            dialogueDisplay.margin = new Vector4(-638, 0, -648, -60);
            dialogueMoved = false;
        }
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
        if (dialogueMoved)
        {
            gray.enabled = true;
        }
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
        maskAnimator.Play("DialogueTextMask");
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
            if (isCinematic && !GameManager.Instance.isTutorial)
            {
                IsCinematic(false);
            }

            gray.enabled = false;
            Controller.instance.FreezePlayer(false);
        }
    }

    private void SetCharacterEmotion()
    {
        if (lastCharacter != DialogueLines[currentDialogue].character)
        {
            textAnimator.Play("DialogueTextAppear");
        }
        
        switch (DialogueLines[currentDialogue].character.frame)
        {
            case CharacterScriptable.Frames.defaultFrame :
                frame.sprite = frames[0];
                break;
            case CharacterScriptable.Frames.Ithar : 
                frame.sprite = frames[1];
                break;
            case CharacterScriptable.Frames.mainCharacter :
                frame.sprite = frames[2];
                break;
        }
        switch (DialogueLines[currentDialogue].dialogLines[sentenceIndex].expressions)
        {
            case dialogueProp.Expressions.Angry:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Angry;
                break;
            
            case dialogueProp.Expressions.Sad:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Sad;
                break;
            
            case dialogueProp.Expressions.Laughing:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Laughing;
                break;
            
            case dialogueProp.Expressions.Confused:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Confused;
                break;
            
            case dialogueProp.Expressions.Neutral:
                characterEmotion.sprite = DialogueLines[currentDialogue].character.Neutral;
                break;
        }

        lastCharacter = DialogueLines[currentDialogue].character;
    }

    private void SkipDialogue()
    {
        if (Input.GetKeyDown(KeyCode.T) && isPlayingDialogue && !GameManager.Instance.isDungeonFinished)
        {
            skip = true;
            CancelInvoke("DisplayNextSentence");
            gray.enabled = false;
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
                IsCinematic(false);
                GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
                Controller.instance.FreezePlayer(false);
            }
        }
    }

    public void IsCinematic(bool cinematic)
    {
        if (!cinematic)
        {
            UIManager.Instance.gameObject.SetActive(true);
            cinematicMode.ResetTrigger("IsCinematic");
            isCinematic = false;
            Controller.instance.animatorPlayerHand.gameObject.SetActive(true);
        }
        else
        {   
            Controller.instance.animatorPlayer.Play("idle");
            UIManager.Instance.gameObject.SetActive(false);
            cinematicMode.SetTrigger("IsCinematic");
            isCinematic = true;
        }
    }

    private void AutomaticDialogues()
    {
        if (!isPlayingDialogue && Time.time >= timeSinceLastDialogue + dialogueMistKellTimer && !GameManager.Instance.isTutorial && !GameManager.Instance.isDungeonFinished)
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
    public IEnumerator CinematicWait()
    {
        yield return new WaitWhile(()=> isPlayingDialogue);
        if (skip)
        {
            skip = false;
        }
        else
        {
            if (isCinematic && !GameManager.Instance.isTutorial)
            {
                IsCinematic(false);
            }
            UIManager.Instance.playerLocation.text = null;
            GameManager.Instance.cameraController.ChangePoint(Controller.instance.PlayerCameraPoint, true);
            Controller.instance.FreezePlayer(false);
        }
    }
}
