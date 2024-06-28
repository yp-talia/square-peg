using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header ("Debug Levels")]    
    public bool debugOnWarn = true;
    public bool debugOnInfo = true;
    public bool debugOnInfoPriority = true; // Adding this in because debugOnInfo is really noisy

    [Header ("Gameplay Constants")]

    public int positionsGainableMin = 1;
    public int positionsGainableMax = 20;
    public int positionsLosableMin = 5;
    public int positionsLosableMax = 15;

    public static DataManager Instance {get; private set;}
    
    //Singleton pattern below
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Data Manager Destroyed");
        }

        if (debugOnInfo == true)
        {
            Debug.Log("Data Manager Awake Complete");
        }
    }
    // TODO: Write public setter function for private data
}