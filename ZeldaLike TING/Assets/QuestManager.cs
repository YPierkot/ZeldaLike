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

    [SerializeField] private bool jackGaveQuest;
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

    [Header("Quest Rewards")] 
    
    [SerializeField] private float coinsNumber;
    [SerializeField] private float throwForce = 1;
    [SerializeField] private float throwRangeDistance = 4f;
    [SerializeField] private GameObject chestFX;

    public IEnumerator JackGiveQuest()
    {
        if (!jackGaveQuest)
        {
            yield return new WaitUntil(() => !DialogueManager.Instance.isPlayingDialogue);
            questText.text = jackQuestText;
            questFrame.SetTrigger("IsOn");
            questTextAnimator.SetTrigger("IsOn");
            yield return new WaitForSeconds(4f);
            questFrame.ResetTrigger("IsOn");
            questTextAnimator.ResetTrigger("IsOn");
            questText.text = null;
        }
        else if(foundArtifact)
        {
            yield return new WaitUntil(() => !DialogueManager.Instance.isPlayingDialogue);
            SpawnObject(adelbert.transform.position);
        }
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
        jack.dialogues.Add(jackDialogue);
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
            questFrame.ResetTrigger("IsOn");
            questTextAnimator.ResetTrigger("IsOn");
            questText.text = null;
        }
        else if (treasureFound)
        {
            yield return new WaitUntil(() => !DialogueManager.Instance.isPlayingDialogue);
            SpawnObject(adelbert.transform.position);
        }
    }

    public IEnumerator FindingTreasure()
    {
        treasureFound = true;
        questText.text = foundAdelbertTreasure;
        questFrame.SetTrigger("IsOn");
        questTextAnimator.SetTrigger("IsOn");
        yield return new WaitForSeconds(4f);
        adelbert.dialogues.Add(adelbertDialogue);
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
    
    public void SpawnObject(Vector3 position)
    {
        SpawnFX(position);
        Dropper dropper = GetComponent<Dropper>();
        for (int i = 0; i < coinsNumber; i++) {
            GameObject ressource = Instantiate(dropper.Loots[0].Item, position, Quaternion.identity);
            //if(dropper != null) dropper.Loot();
            if (ressource.GetComponent<Rigidbody>() == null) return;
            ressource.GetComponent<Rigidbody>().AddForce(Vector3.up * throwForce + GetPointOnCircle(Random.Range(0,360), Random.Range(0,360)), ForceMode.Impulse);
        }
    }
    private Vector3 GetPointOnCircle(float rotationX, float rotationY) => new Vector3(throwRangeDistance * Mathf.Sin(rotationX), 0, throwRangeDistance * Mathf.Cos(rotationX));
    public void SpawnFX(Vector3 position) => Instantiate(chestFX, position, Quaternion.identity);
}
