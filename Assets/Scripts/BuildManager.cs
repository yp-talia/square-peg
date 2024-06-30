// Everything that is build specific goes in here

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    private DataManager dataManager;
    private InputManager inputManager;
    private void Start()
    {
        // Singleton of BuildManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //Instance of Data Manager
        dataManager = DataManager.Instance;
        // Instance of Input Manager
        inputManager = InputManager.Instance;

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Build Manager Start Complete");
        }

    }
    void Update()
    {
#if !UNITY_WEBGL
        if (inputManager.ExitTriggered == true)
        {
            if (dataManager.debugOnInfoPriority == true)
            {
                Debug.Log("Player attempted to Quit the game");
            }
            Application.Quit();
        }
#endif

#if UNITY_EDITOR
        if (inputManager.ExitTriggered == true)
        {
            if (dataManager.debugOnInfoPriority == true)
            {
                Debug.Log("Player attempted to Quit the game");
            }
            UnityEditor.EditorApplication.isPlaying = false;
        }
#endif
    }
}
