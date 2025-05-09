using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public static SoundManager Instance;
    public AudioSource ambianceSource;
    public AudioSource musicSource;
    public AudioClip dungeonMusic;
    public AudioClip exploMusic;
    public AudioClip bossMusic; 
    

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

    public void SetAmbiance(AudioClip ambiance)
    {
        ambianceSource.clip = ambiance;
        ambianceSource.Play();
    }

    public void Interrupt()
    {
        _audioSource.Stop();
    }
}
