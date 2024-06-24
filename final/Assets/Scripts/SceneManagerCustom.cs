//Trying to keep Scene Management neat...
// Three functions:
    // Load - which also unloads the other scenes
    // Load Additive (adding another scene to the current Scene(s))
    // Unload Async - unloading the additive scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    //DataManager for Debug options
    private DataManager dataManager;
    
    private void Start()
    {
    // Instance of Data Manager
    dataManager = DataManager.Instance;

    if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Scene Manager Custom Start Complete");
        }
    }

    public void LoadScene(string sceneName)
    {
            SceneManager.LoadScene(sceneName);
            if (dataManager.debugOnInfo == true)
            {
                Debug.Log("Loaded: " + sceneName);
            }
    }
    public void LoadSceneAdditive(string sceneName)
    {
            SceneManager.LoadScene(sceneName,  LoadSceneMode.Additive);
            if (dataManager.debugOnInfo == true)
            {
                Debug.Log("Loaded: " + sceneName);
            }
    }
    public void UnloadSceneAdditive(string sceneName)
    {
            SceneManager.UnloadSceneAsync(sceneName);
            // Quick fix for cursor visibility bug
            Cursor.visible = false;
            if (dataManager.debugOnInfo == true)
            {
                Debug.Log("Unloaded: " + sceneName);
            }
    }
}

