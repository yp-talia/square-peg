using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Object Options")]
public class GameObjectOptions : ScriptableObject
{
    public GameObject[] options;
    public GameObject selection;
}
