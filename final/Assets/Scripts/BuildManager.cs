// Everything that is build specific goes in here unless...
// it has a really good reason to be on a GameObject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance {get; private set;}
    private InputManager inputManager;
    // Start is called before the first frame update
    private void Awake()
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
    // Instance of Input Manager
    inputManager = InputManager.Instance;
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
