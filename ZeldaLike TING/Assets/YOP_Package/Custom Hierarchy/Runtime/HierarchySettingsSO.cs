#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomizeEditor.HierarchySO {
    [System.Serializable]
    public class HierarchySettingsSO : ScriptableObject {
        [Tooltip("Use a custom Hierarchy to have a better visualisation of the Scene Objects")]
        [SerializeField] private bool useCustomHierarchy = true;

        [SerializeField] private List<SceneAsset> activScene = new List<SceneAsset>();

        //BACKGROUND
        [SerializeField] private Color darkerBackground = new Color(0, 0, 0, 22 / 256f);
        [SerializeField] private bool useDarkerBackground = true;
        [SerializeField] private Color backgroundGroupA =  new Color(255 / 256f, 253 / 256f, 101 / 256f, 25 / 256f);
        [SerializeField] private Color backgroundGroupB = new Color(230 / 256f, 106 / 256f, 231 / 256f, 25 / 256f);
        [SerializeField] private bool useColorFullBackground = true;
        
        //TREE BRANCH
        [SerializeField] private List<Color> treeColorListA = new List<Color>() {
            new Color(255 / 256f, 253 / 256f, 101 / 256f, 1),
            new Color(255 / 256f, 210 / 256f, 0 / 256f, 1),
            new Color(255 / 256f, 150 / 256f, 0 / 256f, 1),
            new Color(255 / 256f, 60 / 256f, 0 / 256f, 1)
            
        }; 
        [SerializeField] private List<Color> treeColorListB = new List<Color>(){
            new Color(230 / 256f, 106 / 256f, 231 / 256f, 1),
            new Color(145 / 256f, 79 / 256f, 255 / 256f, 1),
            new Color(116 / 256f, 198 / 256f, 255 / 256f, 1),
            new Color(55 / 256f, 133 / 256f, 199 / 256f, 1)
        }; 
        [SerializeField] private bool useComplexColor = true;
        [SerializeField] private bool useTreeBranch = true;
        [SerializeField] private int treeSize = 2;
        
        //ICONS GAMEOBJECT
        [SerializeField] private List<string> ignoreIconsName = new List<string>() {"d_GameObject Icon"};
        [SerializeField] private bool useIconGam = true;

        //ICONS COMPONENT
        [SerializeField] private List<string> componentIcontype = new List<string>() {
            "UnityEngine.BoxCollider", "UnityEngine.BoxCollider2D", "UnityEngine.MeshCollider", "UnityEngine.SphereCollider",
            "UnityEngine.Rigidbody", "UnityEngine.Rigidbody2D",
            "UnityEngine.Camera",
            "UnityEngine.Light",
            "UnityEngine.Animator",
            "UnityEngine.Canvas"
        };
        [SerializeField] private bool drawMonobehaviourScriptIcon = true;
        [SerializeField] private bool useIconComponent = true;
        
        //SEPARATOR
        [SerializeField] private string separatorTag = "EditorOnly";
        [SerializeField] private Color separatorColor = new Color(45 / 256f, 45 / 256f, 45 / 256f, 1);
        [SerializeField] private bool useSeparator = true;
        [SerializeField] private int separatorLineSize = 5;
        
        /// <summary>
        /// When data changed
        /// </summary>
        private void OnValidate() {
            CustomHierarchy.ReloadData();
            AssetDatabase.Refresh();
        }
        
        #region PublicVariables
        //Hierarchy
        public bool UseCustomHierarchy => useCustomHierarchy;
        public List<SceneAsset> ActivScene => activScene;
        public Color DarkerBackground => darkerBackground;
        public bool UseDarkerBackground => useDarkerBackground;

        //Background
        public Color BackgroundGroupA => backgroundGroupA;
        public Color BackgroundGroupB => backgroundGroupB;
        public bool UseColorFullBackground => useColorFullBackground;

        //TreeBranch
        public List<Color> TreeColorListA => treeColorListA;
        public List<Color> TreeColorListB => treeColorListB;
        public bool UseComplexColor => useComplexColor;
        public bool UseTreeBranch => useTreeBranch;
        public int TreeSize => treeSize;

        //Icon Object
        public List<string> IgnoreIconsName => ignoreIconsName;
        public bool UseIconGam => useIconGam;
        
        //Icon Component
        public List<string> ComponentIcontype => componentIcontype;
        public bool DrawMonobehaviourScriptIcon => drawMonobehaviourScriptIcon;
        public bool UseIconComponent => useIconComponent;

        //Separator
        public string SeparatorTag => separatorTag;
        public Color SeparatorColor => separatorColor;
        public bool UseSeparator => useSeparator;
        public int SeparatorLineSize => separatorLineSize;

        #endregion PublicVariables

        public void ResetVariables() {
            useCustomHierarchy = true;
            
            //Scene List
            activScene.Clear();
            
            //Background
            useDarkerBackground = true;
            useColorFullBackground = true;
            backgroundGroupA =  new Color(255 / 256f, 253 / 256f, 101 / 256f, 25 / 256f);
            backgroundGroupB = new Color(230 / 256f, 106 / 256f, 231 / 256f, 25 / 256f);
            darkerBackground = new Color(0, 0, 0, 15 / 256f);
            
            //Tree
            useComplexColor = true;
            useTreeBranch = true;
            treeSize = 2;
            treeColorListB = new List<Color>(){new Color(230 / 256f, 106 / 256f, 231 / 256f, 1), new Color(145 / 256f, 79 / 256f, 255 / 256f, 1), new Color(116 / 256f, 198 / 256f, 255 / 256f, 1), new Color(55 / 256f, 133 / 256f, 199 / 256f, 1)}; 
            treeColorListA = new List<Color>() {new Color(255 / 256f, 253 / 256f, 101 / 256f, 1), new Color(255 / 256f, 210 / 256f, 0 / 256f, 1), new Color(255 / 256f, 150 / 256f, 0 / 256f, 1), new Color(255 / 256f, 60 / 256f, 0 / 256f, 1)}; 
            
            //Icons Gam
            ignoreIconsName = new List<string>() {"d_GameObject Icon"};
            useIconGam = true;
            
            //Icons Component
            componentIcontype = new List<string>() {"UnityEngine.BoxCollider", "UnityEngine.BoxCollider2D",  "UnityEngine.MeshCollider", "UnityEngine.SphereCollider","UnityEngine.Rigidbody", "UnityEngine.Rigidbody2D", "UnityEngine.Camera", "UnityEngine.Light", "UnityEngine.Animator", "UnityEngine.Canvas"};
            drawMonobehaviourScriptIcon = true;
            useIconComponent = true;
            
            //Separator
            separatorTag = "EditorOnly";
            separatorColor = new Color(45 / 256f, 45 / 256f, 45 / 256f, 1);
            useSeparator = true;
            separatorLineSize = 5;
        }
    }
}
#endif