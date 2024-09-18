// ! Remember to call LoadScene or UnloadSceneAdditive as the last call on any trigger

//Trying to keep Scene Management neat...
// Six functions:
// Generic:
// Load - which also unloads the other scenes
// Load Additive (adding another scene to the current Scene(s))
// Unload Async - unloading the additive scene
// Minigame specific:
// SceneOptionUpdate - for the list of Minigames at Resources/ScriptableObjects/Minigames...
// ... mark the scene provided as used/played
// SceneOptionReset - when all scenes have been played, reset the list
// LoadSceneAdditiveFromOptions - Load a scene from the list of Minigames which are unplayed

using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Loaded: " + sceneName);
        }
    }
    public void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Loaded: " + sceneName);
        }
    }
    public void UnloadSceneAdditive(string scene)
    {
        // Having big problems here with scene no unloading, unloading wrong scene, unloading out of order.

        // A few sources: 
        // https://damiandabrowski.medium.com/scene-management-in-unity-a-comprehensive-guide-to-loading-scenes-sync-and-async-845ae1e129be
        // https://forum.unity.com/threads/unloadsceneasync-scenemanager-doesnt-seem-to-be-asynchronous.789506/
        // https://forum.unity.com/threads/scenemanager-unloadsceneasync-not-coroutine-friendly.465269/
        // https://gist.github.com/TerryBeyak/51b8020d214384dd8480205246c05df1 
        // https://medium.com/womenintechnology/getting-started-with-coroutines-f0e2ef1410a6 

        Cursor.visible = false;  // Quick fix for cursor visibility bug

        StartCoroutine(UnloadSceneAdditiveCoroutine(scene));
    }

    private IEnumerator UnloadSceneAdditiveCoroutine(string scene)
    {
        yield return StartCoroutine(SceneOptionUpdate(scene));

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);
        while (asyncUnload.isDone == false)
        {
            yield return null;
        }

        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Unloaded: " + scene);
        }
    }

    private IEnumerator SceneOptionUpdate(string scene)
    // filtered arrays in c# 
    // Using LINQ https://learn.microsoft.com/en-us/dotnet/api/system.linq?view=net-8.0
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.toarray?view=net-8.0
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where?view=net-8.0
    {
        bool allScenesUsed = true; // If all minigames played = true, assuming true changing to false.

        bool sceneMatched = false;

        // Get SceneOptions
        Scenes[] sceneOptions = Resources.Load<SceneOptions>("ScriptableObjects/Minigames").scenes;
        if (sceneOptions == null)
        {
            Debug.LogError("Minigame Options Scriptable Object not found");
            yield break;
        }
        else if (dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Marking scene: " + scene + " as played");
        }


        // For scenes in SceneOptions. Match scene name in SceneOptions. Set sceneUsed = true
        for (int i = 0; i < sceneOptions.Length; i++)
        {
            if (sceneOptions[i].sceneName == scene)
            {
                sceneOptions[i].sceneUsed = true;
                sceneMatched = true;
                if (dataManager.debugOnInfo == true)
                {
                    Debug.Log("Scene:" + sceneOptions[i].sceneName + " - Scene used= " + sceneOptions[i].sceneUsed);
                }
            }

            // Setting all scenes used flag to false, if one scene's flag is false.
            if (sceneOptions[i].sceneUsed == false)
            {
                allScenesUsed = false;
                if (dataManager.debugOnInfo == true)
                {
                    Debug.Log("At least " + sceneOptions[i].sceneUsed + " not used");
                }
            }

        }

        if (sceneMatched == false)
        {
            Debug.LogWarning("No Scene found with matching name" + scene);
        }

        // If all scenes are sceneUsed = true, reset
        if (allScenesUsed == true)
        {
            SceneOptionReset();
            if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
            {
                Debug.Log("There are no unused scenes remaining. Resetting");
            }
        }
        yield return null;
    }
    public void SceneOptionReset()
    {
        // Get SceneOptions
        Scenes[] sceneOptions = Resources.Load<SceneOptions>("ScriptableObjects/Minigames").scenes;
        if (sceneOptions == null)
        {
            Debug.LogError("Minigame Options Scriptable Object not found");
        }
        // For scenes in SceneOptions
        // Set sceneUsed = false
        // Logging
        for (int i = 0; i < sceneOptions.Length; i++)
        {
            sceneOptions[i].sceneUsed = false;

            if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
            {
                Debug.Log("Scene usage reset - all Minigames in Minigames now selectable");
            }
        }
    }
    public void LoadSceneAdditiveFromOptions()
    {
        // Handling for if the player meets the threshold for winning or losing ... otherwise, go to else and load minigame
        if (dataManager.currentPosition <= dataManager.successThreshold)
        {
            SceneManager.LoadScene("Win", LoadSceneMode.Single);
        }
        else if (dataManager.currentPosition >= dataManager.failureThreshold)
        {
            dataManager.loseCount +=1;
            dataManager.currentPosition = Mathf.Max(dataManager.positionDefault - (dataManager.loseCount * dataManager.startPositionBuff),dataManager.startPositionMin);
            SceneManager.LoadScene("Lose", LoadSceneMode.Single);
        }
        else
        {
            // Get SceneOptions
            Scenes[] sceneOptions = Resources.Load<SceneOptions>("ScriptableObjects/Minigames").scenes;
            if (sceneOptions == null)
            {
                Debug.LogError("Minigame Options Scriptable Object not found at ScriptableObjects/Minigames");
                return; // return if not found
            }

            // Array of SceneOptions where sceneUsed is false
            Scenes[] scenes = sceneOptions.Where(scene => scene.sceneUsed == false).ToArray();
            // Get Length of Array
            int length = scenes.Length;

            // If no scenes returned, reset, then try again
            if (length == 0)
            {
                if (dataManager.debugOnWarn == true)
                {
                    Debug.LogWarning("No unplayed Minigames: Resetting");
                }

                // coroutine call see end of method.
                StartCoroutine(ResetScenesRetryLoadSceneAdditiveFromOptions());
                return;

            }
            
            // If length is 1, load scene 1
            if (length == 1)
            {
                SceneManager.LoadSceneAsync(scenes[0].sceneName, LoadSceneMode.Additive);
                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                {
                    Debug.Log("Loading: " + scenes[0].sceneName);
                }

            }
            // If length > 1, load random scene between 1 and length
            else if (length > 1)
            {
                int r = Random.Range(0, length);
                // Logging and checking I underhand how LINQ works
                for (int i = 1; i < length; i++)
                {
                    if (dataManager.debugOnInfo == true)
                    {
                        Debug.Log(scenes[i].sceneName);
                    }
                }
                SceneManager.LoadSceneAsync(scenes[r].sceneName, LoadSceneMode.Additive);
                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                {
                    Debug.Log("Loading: " + scenes[r].sceneName);
                }
            }
            // if called, reset sceneUsed on Minigames: SceneOptions,
            // reattempt to load minigames
            IEnumerator ResetScenesRetryLoadSceneAdditiveFromOptions()
            {
                SceneOptionReset();
                yield return null;
                LoadSceneAdditiveFromOptions();
            }
        }
    }
}

