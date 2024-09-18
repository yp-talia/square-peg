using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Raw Image Field Options To Material Selection")]
public class RenderTextureToMaterialFieldOptions : ScriptableObject
{
    // Player selects the RawImage and the corresponding GameObject is stored
    public RenderTexture[] options;
    public Material[] pairedOptions;
    public Material selection;
}
