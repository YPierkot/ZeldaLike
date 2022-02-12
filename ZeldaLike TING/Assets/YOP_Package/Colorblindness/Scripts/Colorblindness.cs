using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Colorblindness : Singleton<Colorblindness> {
    #region Variables
    [SerializeField] private Volume colorblindVolume = null;
    [SerializeField] private ColorblindTypes currentType = ColorblindTypes.normal;
    public ColorblindTypes CurrentType => currentType;
    [SerializeField, Range(0,1)] private float volumeLength = 0;
    
    private VolumeComponent lastFilter;
    
    public static int maxType => (int) System.Enum.GetValues(typeof(ColorblindTypes)).Cast<ColorblindTypes>().Last();
    private int _currentType = 0;
    #endregion Variables
    
    #region Base Methods
    /// <summary>
    /// Called just after making this as an Instance
    /// </summary>
    protected override void Init() {
        DontDestroyOnLoad(this);
        ChangeFilter(-1);
        StartCoroutine(ApplyFilter());
    }

    /// <summary>
    /// Apply a filter to the scene
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApplyFilter() {
        ResourceRequest loadRequest = Resources.LoadAsync<VolumeProfile>($"Colorblind/{(ColorblindTypes) _currentType}");
        yield return loadRequest.isDone;

        VolumeProfile filter = (VolumeProfile) loadRequest.asset;
        if (filter == null) {
            Debug.LogError($"An error has occured while loading the volume profile {(ColorblindTypes) _currentType}! Please, report");
            yield break;
        }

        if (lastFilter != null) colorblindVolume.profile.components.Remove(lastFilter);
        foreach (var component in filter.components) colorblindVolume.profile.components.Add(component);
        lastFilter = filter.components[0];
    }
    #endregion Base methods
    
    #region Called Methods
    /// <summary>
    /// Change the actual colorblindSettings
    /// </summary>
    /// <param name="filterIndex"></param>
    public void ChangeFilter(int filterIndex = -1) {
        filterIndex = filterIndex <= -1 ? PlayerPrefs.GetInt("ColorblindType", 0) : filterIndex;
        
        _currentType = Mathf.Clamp(filterIndex, 0, maxType);
        currentType = (ColorblindTypes) _currentType;
        
        PlayerPrefs.SetInt("ColorblindType", _currentType);
        StartCoroutine(ApplyFilter());
    }

    /// <summary>
    /// Change the actual weight of the volume
    /// </summary>
    /// <param name="length"></param>
    public void ChangeVolumeLenght(float length) {
        volumeLength = length;
        colorblindVolume.weight = volumeLength;
    }
    #endregion Called Method
}

/// <summary>
/// Colorblind enum
/// </summary>
public enum ColorblindTypes {
    normal = 0,
    protanopia,
    protanomaly,
    deuteranopia,
    deuteranomaly,
    tritanopia,
    tritanomaly,
    achromatopsia,
    achromatomaly,
}