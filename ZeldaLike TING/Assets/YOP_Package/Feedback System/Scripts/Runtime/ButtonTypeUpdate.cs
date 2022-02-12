using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTypeUpdate : MonoBehaviour {
    [SerializeField] private Button feedbackBtn = null;
    [SerializeField] private Button bugReportBtn = null;
    [SerializeField] private Button otherBtn = null;
    [SerializeField] private Image feedbackBtnImg = null;
    [SerializeField] private Image bugReportBtnImg = null;
    [SerializeField] private Image otherBtnImg = null;

    [Space]
    [SerializeField] private FeedbackType activeFeedbackType = FeedbackType.feedback;
    public FeedbackType ActiveFeedbackType => activeFeedbackType;
    
    [Space]
    [SerializeField] private Color activColor = new Color();
    [SerializeField] private Color normalColor = new Color();


    [SerializeField] private TextMeshProUGUI feedbackTitleTxt = null;
    [SerializeField] private TextMeshProUGUI feedbackDescriptionTxt = null;
    
#if UNITY_EDITOR    
    private void OnValidate() {
        UpdateButtonColor();
        UpdateTextTitle();
    }
#endif
    
    /// <summary>
    /// Update the color of the button
    /// </summary>
    private void UpdateButtonColor() {
        switch (activeFeedbackType) {
            case FeedbackType.feedback:
                if(feedbackBtn != null) feedbackBtn.interactable = false;
                if(feedbackBtnImg != null) feedbackBtnImg.color = activColor;
                
                if(bugReportBtn != null) bugReportBtn.interactable = true;
                if(bugReportBtnImg != null) bugReportBtnImg.color = normalColor;
                
                if(otherBtn != null) otherBtn.interactable = true;
                if(otherBtnImg != null) otherBtnImg.color = normalColor;
                break;
            
            case FeedbackType.bugReport:
                if(feedbackBtn != null) feedbackBtn.interactable = true;
                if(feedbackBtnImg != null) feedbackBtnImg.color = normalColor;
                
                if(bugReportBtn != null) bugReportBtn.interactable = false;
                if(bugReportBtnImg != null) bugReportBtnImg.color = activColor;
                
                if(otherBtn != null) otherBtn.interactable = true;
                if(otherBtnImg != null) otherBtnImg.color = normalColor;
                break;
            
            case FeedbackType.other:
                if(feedbackBtn != null) feedbackBtn.interactable = true;
                if(feedbackBtnImg != null) feedbackBtnImg.color = normalColor;
                
                if(bugReportBtn != null) bugReportBtn.interactable = true;
                if(bugReportBtnImg != null) bugReportBtnImg.color = normalColor;
                
                if(otherBtn != null) otherBtn.interactable = false;
                if(otherBtnImg != null) otherBtnImg.color = activColor;
                break;
        }
    }

    /// <summary>
    /// Update the title of each text
    /// </summary>
    private void UpdateTextTitle() {
        switch (activeFeedbackType) {
            case FeedbackType.feedback:
                if(feedbackTitleTxt != null) feedbackTitleTxt.text = "Feedback Title";
                if(feedbackDescriptionTxt != null) feedbackDescriptionTxt.text = "Feedback Description";
                break;
            case FeedbackType.bugReport:
                if(feedbackTitleTxt != null) feedbackTitleTxt.text = "Bug Report Title";
                if(feedbackDescriptionTxt != null) feedbackDescriptionTxt.text = "Bug Report Description";
                break;
            case FeedbackType.other:
                if(feedbackTitleTxt != null) feedbackTitleTxt.text = "Other Feedback Title";
                if(feedbackDescriptionTxt != null) feedbackDescriptionTxt.text = "Other Feedback Description";
                break;
        }
    }

    /// <summary>
    /// When a button is pressed update the color of all buttons and the active feedback type
    /// </summary>
    /// <param name="id"></param>
    public void ChangeActivType(int id) {
        activeFeedbackType = id switch {
            0 => FeedbackType.feedback,
            1 => FeedbackType.bugReport,
            2 => FeedbackType.other,
            _ => activeFeedbackType
        };
        
        UpdateButtonColor();
        UpdateTextTitle();
    }
}