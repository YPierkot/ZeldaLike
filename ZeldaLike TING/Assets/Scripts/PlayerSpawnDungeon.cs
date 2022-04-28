using System;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSpawnDungeon : MonoBehaviour
{
    [SerializeField] private AudioClip dungeonAmbiance;
    [SerializeField] private VolumeProfile dungeonVolume;

    private void Start()
    {
        SoundManager.Instance.SetAmbiance(dungeonAmbiance);
        GameManager.Instance.volumeManager.profile = dungeonVolume;
    }
}
