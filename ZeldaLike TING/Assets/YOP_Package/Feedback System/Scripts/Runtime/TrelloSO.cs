using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "ToolsScriptable/Trello Data")]
public class TrelloSO : ScriptableObject {
    [SerializeField] private string trelloKey = "";
    public string TrelloKey => trelloKey;
    [SerializeField] private string userToken = "";
    public string UserToken => userToken;
    [SerializeField] private string boardId = "";
    public string BoardID => boardId;
    [SerializeField] private string username = "";

    public string trelloKeyLink => "https://trello.com/app-key";
    public string userTokenLink => $"https://trello.com/1/authorize?key={trelloKey}&scope=read%2Cwrite&expiration=never&response_type=token";
    public string boardIDLink => $"https://api.trello.com/1/members/{username}/boards?key={trelloKey}&token={userToken}";

    [SerializeField] private List<ListIdClass> listOfListID = new List<ListIdClass>();
    public List<ListIdClass> ListOfListID => listOfListID;


    #region METHOD
    
    /// <summary>
    /// Update the active list from the board
    /// </summary>
    public void UpdateListId() {
        string json = UnityWebRequest.Get($"https://api.trello.com/1/boards/{boardId}/lists").downloadHandler.text;
        Debug.Log(json);
    }

    /// <summary>
    /// Open the trelloKey URL
    /// </summary>
    public void OpenTrelloKeyURL() => Application.OpenURL(trelloKeyLink);

    /// <summary>
    /// Open the user token URL
    /// </summary>
    public void OpenUserTokenURL() => Application.OpenURL(userTokenLink);

    /// <summary>
    /// Get all the board available
    /// </summary>
    public void GetAllBoardAvailable() => Application.OpenURL(boardIDLink);

    #endregion
}

[System.Serializable]
public class ListIdClass {
    public string listName = "";
    public string listID = "";
    public string labelID = "";
    public FeedbackType listType = FeedbackType.feedback;
}
