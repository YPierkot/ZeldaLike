using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Sprites")]
public class CardUIScriptable : ScriptableObject
{
    [Header("---Fire")]
    public Sprite fireRecto;
    public Sprite fireVerso;
    [Header("---Ice")]
    public Sprite iceRecto;
    public Sprite iceVerso;
    [Header("---Ground")]
    public Sprite groundRecto;
    public Sprite groundVerso;
    [Header("---Wind")]
    public Sprite windRecto;
    public Sprite windVerso;
}
