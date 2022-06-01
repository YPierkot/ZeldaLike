using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound List", fileName = "new Sound List")]
public class Sounds : ScriptableObject
{
    [Header("--- CARDS")]
    public AudioClip fireBall;
    public AudioClip fireExplosion;
    public AudioClip iceRecto;
    public AudioClip iceVerso;
    public AudioClip WindThrow;
    public AudioClip windAttract;
    public AudioClip groundPeak;
    public AudioClip groundWall;
    
    [Header("--- PLAYER")]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip dash;
    public AudioClip moveGrass;
    public AudioClip moveStone;
    
    [Header("--- ENEMY")]
    public AudioClip swingerAttack;
    public AudioClip boomerExplosion;
    public AudioClip mageShoot;
    public AudioClip mageRegen;
    public AudioClip bossTP;
    public AudioClip bossLaserCast;
    public AudioClip bossLaser;
    public AudioClip bossProjectilImpact;
    public AudioClip bossProjectilShoot;

    [Header("--- UI")]
    public AudioClip click;
    public AudioClip buy;

    
    [Header("--- AMBIANCE")]
    public AudioClip dungeon;
    public AudioClip forest;
    public AudioClip windPlain;
    public AudioClip town;

    [Header("---ENVIRO")] 
    public AudioClip burnVine;
    public AudioClip runeActivation;
    public AudioClip monolythActivation;
    public AudioClip portalAppear;
    public AudioClip blockFinish;
    public AudioClip lever;
    
    [Header("--- MISC.")]
    public AudioClip kellMind;
    public AudioClip magicEffect;
    public AudioClip gainCard;
}
