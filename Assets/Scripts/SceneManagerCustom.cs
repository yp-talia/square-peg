// ! A note from prior me, remember to always call LoadScene or UnloadSceneAdditive...
// ! after you've called all other events on that trigger, because otherwise...
// ! you'll be surprised when they don't fire and you'll be confusedly debugging for 1/2 an hour

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
    // public static SceneManagerCustom Instance;

    private string previousHighestScene;
    private string currentHighestScene;
    private void Start()
    {
        // Instance of Data Manager
        dataManager = DataManager.Instance;
        // if (Instance == null)
        // {
        //     Instance = this;
        //     DontDestroyOnLoad(gameObject);
        // }
        // else
        // {
        //     Destroy(gameObject);
        //     Debug.LogWarning("Scene Manager Custom Destroyed");
        // }

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Scene Manager Custom Start Complete");
        }

        // // Register the delegate to the sceneLoaded event
        // SceneManager.sceneLoaded += OnSceneLoaded;

    }
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == sceneToSetActive)
    //     {
    //         SceneManager.SetActiveScene(scene);
    //         if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
    //         {
    //             Debug.Log("Currently active scene is: " + scene.name + " Previously active scene was: " + previouslyActiveScene);
    //         }
    //     }
    // }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Loaded: " + sceneName);
        }
    }
    public void LoadSceneAdditive(string sceneName)
    {
        previousHighestScene = SceneManager.GetActiveScene().name;
        currentHighestScene = sceneName;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Loaded: " + sceneName);
        }
    }
    public void UnloadSceneAdditive(string scene)
    {
        // Quick fix for cursor visibility bug
        Cursor.visible = false;

        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(previousHighestScene));
        // if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        // {
        //     Debug.Log("Currently active scene is: " + previousHighestScene + "Previously: " + sceneName);
        // }

        SceneManager.UnloadSceneAsync(scene);
        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Unloaded: " + scene);
        }
    }
    public void UnloadHighestScene()
    {
        int totalScenes = SceneManager.sceneCount;
        if (totalScenes > 1)
        {
            Scene scene = SceneManager.GetSceneAt(totalScenes);
            SceneManager.UnloadSceneAsync(scene.name);
            if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
            {
                Debug.Log("Unloaded: " + scene.name);
            }
        }
        else if (dataManager.debugOnWarn == true)
        {
            Debug.LogWarning("Only one scene, did not unload");
        }
    }
}

