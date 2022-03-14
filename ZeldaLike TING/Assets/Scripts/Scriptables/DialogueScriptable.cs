using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueScriptable")]
public class DialogueScriptable : ScriptableObject
{
    public DialogueLine[] dialogue;
}

[System.Serializable]
public class DialogueLine
{
    public CharacterScriptable character;
    public float startingDelay;
    public dialogueProp[] dialogLines;
    public AudioClip voiceLine;
}

[System.Serializable]
public class dialogueProp
{
    public enum Expressions
    {
        Neutral, Sad, Confused, Angry, Laughing
    }

    public Expressions expressions;
    public string name;
    [TextArea(4, 10)]
    public string line;
    
    public float delay;
}
