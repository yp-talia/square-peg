using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Raw Image Field Options To GameObject Selection")]
public class RenderTextureToGameObjectFieldOptions : ScriptableObject
{
    // Player selects the RawImage and the corresponding GameObject is stored
    public RenderTexture[] options;
    public GameObject[] pairedOptions;
    public GameObject selection;
}
