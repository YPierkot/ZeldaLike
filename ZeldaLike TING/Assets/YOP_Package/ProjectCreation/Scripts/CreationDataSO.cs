using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ToolsScriptable/AssetCreationSO")]
public class CreationDataSO : ScriptableObject {
    private bool hasCreateAsset = false;
    public bool HasCreateAsset => hasCreateAsset;
    
    [SerializeField] private List<string> folderNameList = new List<string>();
    public List<string> FolderNameList => folderNameList;
    [Space]
    [SerializeField] private List<string> assetNameList = new List<string>();
    public List<string> AssetNameList => assetNameList;

    /// <summary>
    /// Set initialize to false
    /// </summary>
    public void SetInitializeFalse() => hasCreateAsset = true;
    
    /// <summary>
    /// Reinitialize the scriptable
    /// </summary>
    public void ReinitializeAsset() => hasCreateAsset = false;
}
