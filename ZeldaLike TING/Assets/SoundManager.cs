using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayVoiceline(AudioClip voiceline)
    {
        _audioSource.clip = voiceline;
        _audioSource.Play();
    }
}
