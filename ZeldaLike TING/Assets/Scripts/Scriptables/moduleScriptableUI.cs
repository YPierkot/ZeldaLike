using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Module UI", fileName = "New Module UI")]
public class moduleScriptableUI : MonoBehaviour
{
    public enum moduleType
    {
        Sharpness, LongSword, Thorn, Rock, Swiftness
    }

    public Sprite moduleSprite;
    public moduleType _moduleType;
    private int cost;
}
