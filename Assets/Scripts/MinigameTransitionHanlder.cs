using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Notes mainly for my benefit to understand what this script needs to know about.

// A GameObject needs to inform this class of one of three states
// Success - met the requirement for success
// Failure - in a state which means it can not be successful
// NotSuccess - not yet met the requirement for success / previously met success requirement, but no longer

// This script needs to track the overall number of success/failure
// Then it can take the required action

// To do this, this script only needs to be told the changes i.e. Success+1, Failure+1, Success-1
// We can simplify this to a Success counter and a failure counter.

public class MinigameTransitionHanlder : MonoBehaviour
{
    private DataManager dataManager;
    private SceneManagerCustom sceneManagerCustom;

    [Tooltip("The number of onSuccess events received in order to have achieved success on this game")]
    [SerializeField] int requiredSuccessCount = 1;
    private int currentSuccessCount = 0;
    [SerializeField] int requiredFailureCount = 1;
    private int currentFailureCount = 0;
    private bool completed = false;

    [Header("Outbound Events")]
    [SerializeField] public UnityEvent OnGameSuccessEvent = new UnityEvent();
    [SerializeField] public UnityEvent OnGameFailureEvent = new UnityEvent();

    void Start()
    {
        dataManager = DataManager.Instance;
        // sceneManagerCustom = SceneManagerCustom.Instance;

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Minigame TransitionManager Start Complete");
        }
    }

    public void Success()
    {
        currentSuccessCount += 1;
        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Success count increased for current Minigame= " + currentSuccessCount + " Target: " + requiredSuccessCount);
        }
        if (currentSuccessCount == requiredSuccessCount)
        {
            DisplayLeaderboard("success");
            OnGameSuccessEvent.Invoke();
        }
    }
    public void NotSuccess()
    {
        currentSuccessCount -= 1;
        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Success count increased for current Minigame= " + currentSuccessCount + " Target: " + requiredSuccessCount);
        }
    }
    public void Failure()
    {
        currentFailureCount += 1;
        if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        {
            Debug.Log("Failure count for current Minigame= " + currentFailureCount + "Loss at:" + requiredFailureCount);
        }
        if (currentFailureCount == requiredFailureCount)
        {
            DisplayLeaderboard("failure");
            OnGameFailureEvent.Invoke();
        }
    }

    private void DisplayLeaderboard(string result)
    {
        if (result == "success" && completed != true)
        {
            dataManager.previousPosition = dataManager.currentPosition;
            dataManager.currentPosition -= Random.Range(dataManager.positionsGainableMin, dataManager.positionsGainableMax);

            if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
            {
                Debug.Log("Player successful. Previous leaderboard position: " + dataManager.previousPosition + ". New Position: " + dataManager.currentPosition);
            }
            completed = true;
        }
        else if (result == "failure" && completed != true)
        {
            dataManager.previousPosition = dataManager.currentPosition;
            dataManager.currentPosition += Random.Range(dataManager.positionsGainableMin, dataManager.positionsGainableMax);

            if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
            {
                Debug.Log("Player successful. Previous leaderboard position: " + dataManager.previousPosition + ". New Position: " + dataManager.currentPosition);
            }
            completed = true;
        }
    }
}

