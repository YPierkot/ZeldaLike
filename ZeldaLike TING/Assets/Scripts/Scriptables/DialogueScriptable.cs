using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueScriptable")]
public class DialogueScriptable : ScriptableObject
{
    public DialogueLine[] dialogue;
}

[System.Serializable]
public class DialogueLine
{
    public float startingDelay;
    public dialogueProp[] dialogLines;
    public AudioClip voiceLine;
}

[System.Serializable]
public class dialogueProp
{
    public string name;
    [TextArea(4, 10)]
    public string line;
    public float delay;
}
