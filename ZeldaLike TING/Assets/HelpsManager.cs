using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HelpsManager : MonoBehaviour
{
    [Header("Helps Display")]
    
    [SerializeField] private TextMeshProUGUI helpTextDisplay;
    [SerializeField] private GameObject[] checks = new GameObject[4];
    [SerializeField] private List<helpTexts> helps;
    private Queue<helpTexts> helpsQueue;
    private helpTexts currentHelp;
    private bool done;


    [System.Serializable]
    public class helpTexts
    {
        public int helpIndex;
        public int numberOfLines;
        [TextArea(4, 10)] public string keyBoardHelp;
        [TextArea(4, 10)] public string PSHelp;
        [TextArea(4, 10)] public string XboxHelp;
    }

    private void Awake()
    {
        helpsQueue = new Queue<helpTexts>();
        foreach (var help in helps)
        {
            helpsQueue.Enqueue(help);
        }
    }

    private void Update()
    {
        if (currentHelp != null && !done)
        {
            switch (currentHelp.helpIndex)
            {
                case 1:
                    if (GameManager.Instance.currentContorller == GameManager.controller.Keybord)
                    {
                        if (Input.GetKey(KeyCode.Z))
                        {
                            CheckLine(1);
                        }

                        if (Input.GetKey(KeyCode.Q))
                        {
                            CheckLine(2);
                        }

                        if (Input.GetKey(KeyCode.S))
                        {
                            CheckLine(3);
                        }

                        if (Input.GetKey(KeyCode.D))
                        {
                            CheckLine(4);
                        }
                    }
                    else
                    {
                        if (Controller.instance.moving)
                        {
                            CheckLine(1);
                            CheckLine(2);
                            CheckLine(3);
                            CheckLine(4);
                        }
                    }
                    break;
                case 2:
                    if (Controller.instance.dashing)
                    {
                        CheckLine(1);
                    }
                    break;
                case 3:
                    if (Controller.instance.inAttack)
                    {
                        CheckLine(1);
                    }
                    break;
                case 4:
                    if (CardsController.instance.fireRectoUse)
                    {
                        CheckLine(1);
                    }

                    break;
                case 5:
                    if (!CardsController.instance.fireRectoUse)
                    {
                        CheckLine(1);
                    }

                    break;
                case 6 :
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        CheckLine(1);
                    }
                    break;
            }
        }

    }

    public void DisplayHelp()
    {
        done = false;
        helpTextDisplay.gameObject.SetActive(true);
        currentHelp = helpsQueue.Dequeue();
        Debug.Log(currentHelp.keyBoardHelp);
        switch (GameManager.Instance.currentContorller)
        {
            case GameManager.controller.ps:
                helpTextDisplay.text = currentHelp.PSHelp;
                break;
            case GameManager.controller.Keybord:
                helpTextDisplay.text = currentHelp.keyBoardHelp;
                break;
            case GameManager.controller.Xbox:
                helpTextDisplay.text = currentHelp.XboxHelp;
                break;
        }
    }

    private void CheckIfHelpFinished()
    {
        bool finished = true;
        for (int i = 0; i < currentHelp.numberOfLines-1; i++)
        {
            if (!checks[i].activeSelf)
            {
                finished = false;
            }
        }
        if (finished)
        {
            done = true;
            Invoke("ResetHelpText", 1.5f);;
        }
    }

    private void CheckLine(int line)
    {
        checks[line - 1].SetActive(true);
        Debug.Log("Je check la condition " + currentHelp.helpIndex);
        CheckIfHelpFinished();
    }

    private void ResetHelpText()
    {
        
        foreach (var check in checks)
        {
            check.SetActive(false);
        }
        if (currentHelp.helpIndex == 1)
        {
            DisplayHelp();
        }
        else if (currentHelp.helpIndex == 6)
        {
            enabled = false;
        }
        else
        {
            helpTextDisplay.text = "";
        }
    }
}
