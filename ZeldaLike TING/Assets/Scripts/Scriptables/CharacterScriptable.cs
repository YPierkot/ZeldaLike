using UnityEngine;
[CreateAssetMenu(fileName = "Character", menuName = "CharacterScriptable")]
public class CharacterScriptable : ScriptableObject
{
    public string characterName;
    public Sprite Neutral;
    public Sprite Angry;
    public Sprite Sad;
    public Sprite Laughing;
    public Sprite Confused;

    public DialogueScriptable[] dialogueInterruptions = new DialogueScriptable[]{};
}
