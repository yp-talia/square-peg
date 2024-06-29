using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// Note to self, this is a much better way of constructing ScriptableObjects

public class Scenes
{
public string sceneName;
public bool sceneUsed = false;

}

[CreateAssetMenu(fileName = "Scene Options", menuName = "Scene Options")]
public class SceneOptions : ScriptableObject
{
    public Scenes[] scenes;
}
