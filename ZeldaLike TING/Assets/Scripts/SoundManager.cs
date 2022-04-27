using UnityEngine;

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

    public void Interrupt()
    {
        _audioSource.Stop();
    }
}
