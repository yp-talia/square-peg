using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Handling whe the player wins or loses a mimigame
    // Success Method: called by event
        // Increase count of success by 1
        // If you reach success th

public class MinigameTransitionHanlder : MonoBehaviour
{
    private DataManager dataManager;
    
    [Tooltip ("The number of onSuccess events received in order to have achieved success on this game")]
    [SerializeField] int requiredSuccessCount = 1;
    private int currentSuccessCount = 0;

    [Header("Outbound Events")]
    [SerializeField] public UnityEvent OnGameSuccessEvent = new UnityEvent();
    [SerializeField] public UnityEvent OnGameFailureEvent = new UnityEvent();

    void Start()
    {
        dataManager = DataManager.Instance;

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Minigame TransitionManager Start Complete");
        }
    }

    public void Success()
    {
        currentSuccessCount +=1;
        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Success count for current Minigame= " +  currentSuccessCount);
        }
        if (currentSuccessCount == requiredSuccessCount)
        {
            OnGameSuccessEvent.Invoke();
        }
    }
    public void Failure()
    {
        Mathf.Max(currentSuccessCount -=1,0);
        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Success count for current Minigame= " +  currentSuccessCount);
        }
    }

    // public void UpdateLeaderboardPlacement(result)
    // {
    //     // if success 
    //         // 
    //     // if failure
    //         //
    // }
}
