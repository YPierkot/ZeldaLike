using System;
using System.Collections;
using System.IO;
using Codice.CM.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TrelloRuntime : MonoBehaviour {
    //MAPPING
    private TrelloManagerInput mapping = null;

    [Header("TRELLO DATA")]
    [SerializeField] private TrelloSO trelloData = null;
    
    #region DATA VARIABLES
    private Camera activeCam = null;
    [Header("BASIC DATA")]
    [SerializeField] private GameObject reportPanel = null;
    [SerializeField] private Button sendReportBtn = null;
    [Space] 
    [SerializeField] private TMP_InputField usernameInputfield = null;
    [SerializeField] private TMP_InputField feedbackTitleInputfield = null;
    [SerializeField] private TMP_InputField feedbackDescriptionInputfield = null;
    [Space]
    [SerializeField] private ButtonTypeUpdate buttonType = null;
    [SerializeField] private Toggle screenshotToggle = null;
    [Space]
    [SerializeField] private Image screenshotImg = null;
    #endregion

    private Texture2D screenShot = null;
    private byte[] screenshotBytes;

    /// <summary>
    /// Called when the object is enabled
    /// </summary>
    private void OnEnable() {
        activeCam = Camera.main;
        
        mapping = new TrelloManagerInput();
        mapping.Interface.Enable();
        mapping.Interface.OpenClosePanel.started += ReportFeedbackInputPerformed;
        
        if(reportPanel != null) reportPanel.SetActive(false);
    }

    /// <summary>
    /// Enable or Disable the BugReport Panel
    /// </summary>
    /// <param name="obj"></param>
    private void ReportFeedbackInputPerformed(InputAction.CallbackContext obj) => StartCoroutine(TakeScreenshotThenOpenPanel());

    /// <summary>
    /// Enable or Disable the panel
    /// </summary>
    private void ChangePanelState() => reportPanel.SetActive(!reportPanel.activeSelf);

    /// <summary>
    /// Take a screenshot then open the panel
    /// </summary>
    /// <returns></returns>
    private IEnumerator TakeScreenshotThenOpenPanel() {
        yield return new WaitForEndOfFrame();

        int resWidth = 1920;
        int resHeight = 1080;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        
        if (activeCam != null) {
            activeCam.targetTexture = rt;
            screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            activeCam.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            screenShot.Apply();
            
            activeCam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            screenshotBytes = screenShot.EncodeToPNG();
            screenshotImg.sprite = Sprite.Create(screenShot, new Rect(0,0,resWidth, resHeight), new Vector2(.5f,.5f), 10);
        }

        yield return new WaitForEndOfFrame();
        
        if (reportPanel != null) {
            ChangePanelState();
            if (usernameInputfield != null) usernameInputfield.text = PlayerPrefs.GetString("LastUsernameFeedback", "");
        }

        if (sendReportBtn != null) CheckReportChanges();
    }
    
    /// <summary>
    /// When the player make an update. Check if he can press the send button
    /// </summary>
    public void CheckReportChanges() {
        bool canSendReport = usernameInputfield.text != "" && feedbackTitleInputfield.text != "" && feedbackDescriptionInputfield.text != "";
        sendReportBtn.interactable = canSendReport;
    }
    
    /// <summary>
    /// Send the feedback to the trello
    /// </summary>
    public void SendReport() {
        //Update the last username
        StartCoroutine(CreateCard(usernameInputfield.text, feedbackTitleInputfield.text, feedbackDescriptionInputfield.text, buttonType.ActiveFeedbackType));
        usernameInputfield.text = "";
        feedbackTitleInputfield.text = "";
        feedbackDescriptionInputfield.text = "";
        buttonType.ChangeActivType(0);
        screenshotToggle.isOn = true;
        ChangePanelState();
    }

    /// <summary>
    /// Create a card based on the input field
    /// </summary>
    /// <param name="username"></param>
    /// <param name="title"></param>
    /// <param name="desc"></param>
    /// <returns></returns>
    private IEnumerator CreateCard(string username, string title, string desc, FeedbackType type) {
        PlayerPrefs.SetString("LastUsernameFeedback", username);

        WWWForm form = new WWWForm();
        form.AddField("name", title);
        foreach (ListIdClass list in trelloData.ListOfListID) {
            if (list.listType == type) {
                form.AddField("idList", list.listID);
                form.AddField("idLabels", list.labelID);
            }
        }
        form.AddField("desc", $" #{title}\n*from : {username}*\n{desc}");
        if(screenshotToggle.isOn) form.AddBinaryData("fileSource", screenshotBytes);
        
        UnityWebRequest download = UnityWebRequest.Post($"https://api.trello.com/1/cards/?key={trelloData.TrelloKey}&token={trelloData.UserToken}", form);
        
        yield return download.SendWebRequest();

        if (download.result != UnityWebRequest.Result.Success) Debug.LogError(download.error);
        else {
            DiscordWebHook.SendDiscordMessage($"**:warning: *{username}* JUST SEND A NEW FEEDBACK! :warning:** \n\n**TITLE :** {title.ToUpper()}\n**DESCRIPTION :** {desc}\n**LIST :** {type}\n\n*Go see it by yourself with the following link : https://trello.com/invite/b/MvAF5F5Q/f8d0108041cd63f90d0412e744dbd0f5/zelda-like-bug-report*", "Feedback Report Bot", "https://discord.com/api/webhooks/937761532727689336/FuJ-ztAamRLFq8Lxp-mpcZJms897AgzzEX_tYvbtmcqkZG7NSreMMV4BZi3d257RQM3g");
            Debug.Log("card send");
        }
    }
}

public enum FeedbackType{ feedback, bugReport, other}

