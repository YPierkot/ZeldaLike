using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    [SerializeField] private GameObject defautButton;
    private GameObject menuOption;

    private AudioMixer musicMixer;
    private AudioMixer EffectsMixer;
    private AudioMixer voicelinesMixer;

    private void Start()
    {
        musicMixer = (AudioMixer)Resources.Load("AudioMix/Music"); 
        EffectsMixer = (AudioMixer)Resources.Load("AudioMix/Effects"); 
        voicelinesMixer = (AudioMixer)Resources.Load("AudioMix/Voiceline");
        if(Instance != this) return;
        if (defautButton != null) EventSystem.current.SetSelectedGameObject(defautButton);
        
    }

    private void Update()
    {
        if(Instance != this) return;
    }

    public void LoadScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void OpenOptions()
    {
        Debug.Log("Load Menu");
        if (menuOption == null) menuOption = Instantiate((GameObject)Resources.Load("UI/Menu Options"));
        menuOption.SetActive(true);
        EventSystem.current.SetSelectedGameObject(menuOption.transform.GetChild(1).gameObject);
    }

    public void CloseOptionsButton()
    {
        gameObject.SetActive(false);
        if(MenuManager.Instance.defautButton != null)EventSystem.current.SetSelectedGameObject(MenuManager.Instance.defautButton);
        
    }

    public void ChangeMusicVolume( Slider slider)
    {
        if(musicMixer != null) musicMixer.SetFloat("Master", slider.value);
    }
    
    public void ChangeEffectsVolume( Slider slider)
    {
        if(EffectsMixer != null)EffectsMixer.SetFloat("Master", slider.value);
    }
    
    public void ChangeVoicelinessVolume( Slider slider)
    {
        if(voicelinesMixer != null)voicelinesMixer.SetFloat("Master", slider.value);
    }
    
    public void QuitGame()
    {
     Application.Quit();   
    }
}
