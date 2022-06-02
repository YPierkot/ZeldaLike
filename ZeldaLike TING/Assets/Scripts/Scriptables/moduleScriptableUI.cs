using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module", fileName = "New Module")]
public class moduleScriptableUI : ScriptableObject
{
    public enum moduleType
    {
        Sharpness, LongSword, Thorn, Rock, Swiftness, Toughness
    }

    public Sprite moduleSprite;
    public moduleType _moduleType;
    public int moduleLevel = 1;
}
