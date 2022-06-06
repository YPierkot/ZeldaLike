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
    private GameObject openMenu;
    private bool inMenu;

    private AudioMixer musicMixer;
    private AudioMixer EffectsMixer;
    private AudioMixer voicelinesMixer;

    private void Start()
    {
        musicMixer = (AudioMixer)Resources.Load("AudioMix/Music"); 
        EffectsMixer = (AudioMixer)Resources.Load("AudioMix/Effects"); 
        voicelinesMixer = (AudioMixer)Resources.Load("AudioMix/Voiceline");
        
        // --- ONLY INSTANCE
        if(Instance != this) return;
        
        if (defautButton != null) EventSystem.current.SetSelectedGameObject(defautButton);
        if(Controller.instance != null) Controller.instance.pause += Back;

    }

    private void Update()
    {
        if(Instance != this) return;
    }

    public void LoadScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void Back()
    {
        if (MenuManager.Instance != this)
        {
            MenuManager.Instance.Back();
            return;
        }
        Debug.Log("Menuuuu");
        if (!inMenu)
        {
            OpenOptions();
            
        }
        else
        {
           CloseMenu();
        }
    }

    public void OpenMenu(GameObject menu)
    {
        if (this != Instance)
        {
            MenuManager.Instance.OpenMenu(menu);
            return;
        }
        menu.SetActive(true);
        openMenu = menu;
        inMenu = true;
        EventSystem.current.SetSelectedGameObject(menu.transform.GetChild(1).gameObject);
        Debug.Log("Time Stop");
        Time.timeScale = 0f;
    }

    public void CloseMenu()
    {
        if (openMenu == menuOption)
        {
            CloseOptionsButton();
        }
        else
        {
            Time.timeScale = 1f;
            Debug.Log("Time Play");
            openMenu.SetActive(false);
            inMenu = false; 
        }
    }

    public void OpenShop()
    {
        MenuManager.Instance.OpenMenu(UIManager.Instance.ShopCanvas.gameObject);
    }
    
    public void OpenOptions()
    {
        if (menuOption == null) menuOption = Instantiate((GameObject)Resources.Load("UI/Menu Options"), transform);
        menuOption.SetActive(true);
        openMenu = menuOption;
        inMenu = true;
        Debug.Log(menuOption);
        Debug.Log(menuOption.transform.GetChild(1));
        Debug.Log(menuOption.transform.GetChild(1).gameObject);
        Debug.Log(EventSystem.current);
        EventSystem.current.SetSelectedGameObject(menuOption.transform.GetChild(1).gameObject);
        Time.timeScale = 0f;
    }

    public void CloseOptionsButton()
    {
        Time.timeScale = 1f;
        Debug.Log("Time Play");
        menuOption.SetActive(false);
        inMenu = false;
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
