// Everything that is build specific goes in here unless...
// it has a really good reason to be on a GameObject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance {get; private set;}
    private DataManager dataManager;
    private InputManager inputManager;

    // Start -- before games starts, but after Awake
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

    // Update is called once per frame
    void Update()
    {
        #if !UNITY_WEBGL
            if (inputManager.ExitTriggered == true)
            {
                Application.Quit();
            }
            if (inputManager.FullscreenTriggered == true)
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        #endif 
    }
}
