using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    private AudioSource[] sources;

    public Sounds sounds;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
      sources = GetComponents<AudioSource>();
      sounds = (Sounds)Resources.Load("Sound List");
      sources[sources.Length - 1].clip = sounds.moveGrass;
    }

    private void Update()
    {
       if(Controller.instance.moving && !sources[sources.Length-1].isPlaying)
       {
           sources[sources.Length - 1].pitch = Random.Range(0.8f, 1.2f);
           sources[sources.Length - 1].Play();
       }
       else if(!Controller.instance.moving && sources[sources.Length-1].isPlaying) sources[sources.Length-1].Stop();
    }

    public void PlaySound(AudioClip clip, float pitchCoef = 0)
    {
        //Debug.Log($"Play {clip.name}");
        foreach (var source in sources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                float pitch = Random.Range(1f - pitchCoef, 1f + pitchCoef);
                if (pitch > 3f) pitch = 3f;
                
                source.pitch = pitch;
                source.Play();
                return;
            }
        }
        Debug.LogError("Not Enought Audio Pist");
    }
}
