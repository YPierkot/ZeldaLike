using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThievesLairNPCList : MonoBehaviour
{
    public NPCDialogue[] NPCs;
    public DialogueScriptable[] windDungeonDialogues;

    public void AddWindDialogues()
    {
        for (int i = 0; i < NPCs.Length; i++)
        {
            NPCs[i].dialogues.Add(windDungeonDialogues[i]);
        }
    }
}
