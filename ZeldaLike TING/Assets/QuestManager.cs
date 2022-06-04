using UnityEngine;
using System.Collections;

public class QuestManager : MonoBehaviour
{
    [Header("Display")]
    
    [SerializeField] private TMPro.TextMeshProUGUI questText;
    [SerializeField] private Animator questTextAnimator;
    [SerializeField] private Animator questFrame;
    
    [Header("Jack")]
    
    [SerializeField] private bool foundArtifact;
    [SerializeField] private string foundArtifactText;
    [SerializeField] private string jackQuestText;
    [SerializeField] private NPCDialogue jack;
    [SerializeField] private DialogueScriptable jackDialogue;
    [SerializeField] private DialogueScriptable jackNotFoundDialogue;
    
    [Header("Adelbert")]
    
    [SerializeField] private bool treasureFound;

    private bool givenQuest;
    [SerializeField] private string adelbertQuestText;
    [SerializeField] private string foundAdelbertTreasure;
    [SerializeField] private NPCDialogue adelbert;
    [SerializeField] private DialogueScriptable adelbertDialogue;
    [SerializeField] private DialogueScriptable adelbertNotFoundDialogue;


    public IEnumerator JackGiveQuest()
    {
        yield return new WaitUntil(() => !DialogueManager.Instance.isPlayingDialogue);
        questText.text = jackQuestText;
        questFrame.SetTrigger("IsOn");
        questTextAnimator.SetTrigger("IsOn");
        yield return new WaitForSeconds(4f);
        jack.dialogues.Add(jackNotFoundDialogue);
        questFrame.ResetTrigger("IsOn");
        questTextAnimator.ResetTrigger("IsOn");
        questText.text = null;
    }
    public IEnumerator FoundArtifact()
    {
        foundArtifact = true;
        questText.text = foundArtifactText;
        questFrame.SetTrigger("IsOn");
        questTextAnimator.SetTrigger("IsOn");
        yield return new WaitForSeconds(4f);
        questFrame.ResetTrigger("IsOn");
        questTextAnimator.ResetTrigger("IsOn");
        questText.text = null;
        jack.dialogues.Insert(1, jackDialogue);
    }

    public IEnumerator AdelbertGiveQuest()
    {
        if (!givenQuest)
        {
            givenQuest = true;
            yield return new WaitUntil(() => !DialogueManager.Instance.isPlayingDialogue);
            questText.text = adelbertQuestText;
            questFrame.SetTrigger("IsOn");
            questTextAnimator.SetTrigger("IsOn");
            yield return new WaitForSeconds(4f);
            adelbert.dialogues.Add(adelbertNotFoundDialogue);
            questFrame.ResetTrigger("IsOn");
            questTextAnimator.ResetTrigger("IsOn");
            questText.text = null;
        }
    }

    public IEnumerator FindingTreasure()
    {
        treasureFound = true;
        questText.text = foundAdelbertTreasure;
        questFrame.SetTrigger("IsOn");
        questTextAnimator.SetTrigger("IsOn");
        yield return new WaitForSeconds(4f);
        adelbert.dialogues.Insert(1, adelbertDialogue);
        questFrame.ResetTrigger("IsOn");
        questTextAnimator.ResetTrigger("IsOn");
        questText.text = null;
    }

    public void JackQuest()
    {
        StartCoroutine(JackGiveQuest());
    }

    public void AdelbertQuest()
    {
        StartCoroutine(AdelbertGiveQuest());
    }

    public void Artifact()
    {
        StartCoroutine(FoundArtifact());
    }

    public void Treasure()
    {
        StartCoroutine(FindingTreasure());
    }
}
