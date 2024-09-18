using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Material Options")]
public class MaterialOptions : ScriptableObject
{
    public Material[] options;
    public Material selection;
}
