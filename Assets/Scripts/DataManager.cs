using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header ("Debug Levels")]    
    public bool debugOnWarn = true;
    public bool debugOnInfo = true;
    public bool debugOnInfoPriority = false; // Adding this in because debugOnInfo is really noisy

    [Header ("Gameplay Contstants")]

    [Tooltip ("Minimum number of places that ranking can increase onSuccess")]
    [SerializeField] public int positionsGainableMin = 1;
    
    [Tooltip ("Maximum number of places that ranking can increase onSuccess")]
    [SerializeField] public int positionsGainableMax = 20;
    
    [Tooltip ("Minimum number of places that ranking can decrease onFailure")]
    [SerializeField] public int positionsLosableMin = 5;
    
    [Tooltip ("Maximum number of places that ranking can decrease onFailure")]
    [SerializeField] public int positionsLosableMax = 15;
    
    [Tooltip ("Position at which player gets hired")]
    [SerializeField] public int successThreshold = 1;
    
    [Tooltip ("Position at which player fails the interview")]
    [SerializeField] public int failureThreshold = 120;
    
    [Tooltip ("The number of positions the player starts higher on the leaderboard after losing")]
    [SerializeField] public int startPositionBuff = 20;
    
    [Tooltip ("The lowest position a player can start on the leaderboard")]
    [SerializeField] public int startPositionMin = 30;

    public readonly int  positionDefault = 100;
    public int previousPosition;

    public int currentPosition;
    
    public int loseCount = 0;

    public static DataManager Instance;
    
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
    previousPosition = positionDefault;

    currentPosition = positionDefault;
    }
    
    public void ResetPlayerRank()
    {
        previousPosition = positionDefault;
        currentPosition = positionDefault;
    }
}