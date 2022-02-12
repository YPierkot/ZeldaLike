using System;
using System.IO;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public static class ProjectCreationEditor {
    static ProjectCreationEditor() => AssemblyReloadEvents.afterAssemblyReload += UpdateProjectDataWait;

    /// <summary>
    /// Call the function to load folders and assets when assembly definition stop to compile scripts
    /// </summary>
    private static void UpdateProjectDataWait() {
        AssetDatabase.Refresh();
        UpdateProjectData();
        AssemblyReloadEvents.afterAssemblyReload -= UpdateProjectDataWait;
    }

    /// <summary>
    /// Update the project data
    /// </summary>
    public static void UpdateProjectData(bool forceUpload = false) {
        CreationDataSO SO = AssetDatabase.LoadAssetAtPath<CreationDataSO>("Assets/YOP_Package/ProjectCreation/Asset Data.asset");
        if (SO != null && (!SO.HasCreateAsset || forceUpload)) CreateBaseFolder(SO);
    }
    
    /// <summary>
    /// Create all the basics folder for a project
    /// </summary>
    private static void CreateBaseFolder(CreationDataSO SO) {
        #region FOLDER CREATION
        foreach (string folderName in SO.FolderNameList) {
            if (AssetDatabase.IsValidFolder($"Assets/{folderName}")) continue;
            
            string[] nameSplit = folderName.Split(new string[] {"/"}, StringSplitOptions.None);
            switch (nameSplit.Length) {
                case 1:
                    AssetDatabase.CreateFolder("Assets", folderName);
                    break;
                
                case >=2:
                    string folderPath = "";
                    for (int i = 0; i < nameSplit.Length - 1; i++) {
                        folderPath += "/" + nameSplit[i];
                    }
                    AssetDatabase.CreateFolder($"Assets{folderPath}", nameSplit[^1]);
                    break;
            }
        }
        #endregion FOLDER CREATION

        #region ASSET CREATION
        foreach (string assetName in SO.AssetNameList) {
            string[] nameSplit = assetName.Split(new string[] {"/"}, StringSplitOptions.None);
            nameSplit[^1] = nameSplit[^1].Split(".")[0];
            string assetPath = $"Assets/" + assetName;

            if (!File.Exists(assetPath)) {
                using StreamWriter outfile = new StreamWriter(assetPath);
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                outfile.WriteLine("public class "+nameSplit[^1]+" : MonoBehaviour {");
                outfile.WriteLine(" ");
                outfile.WriteLine("}");
                AssetDatabase.Refresh();
            }
        }
        #endregion ASSET CREATION
        
        #region DELETE TUTORIAL INFO
        if (AssetDatabase.IsValidFolder("Assets/TutorialInfo")) AssetDatabase.DeleteAsset("Assets/TutorialInfo");
        #endregion DELETE TUTORIAL INFO
        
        SO.SetInitializeFalse();
        AssetDatabase.Refresh();
    }
}
#endif