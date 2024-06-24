//Related documentation and sources:
    // Tutorial: https://youtu.be/Cmx76-Q11tM

// NOTE on above -- I deviate a little bit just to implement count up and count down...
// as well as integrate DataManager for logging

//TODO: Replace these two and their calls with Audio Manager (on event)
    //[SerializeField] private AudioSource timerOnSound = null; //Component for audio while timer active
    //[SerializeField] private AudioSource timerEndSound = null; //Component for audio while timer end

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerHelper : MonoBehaviour
{
    [Header ("Timer Duration")]
    
    [SerializeField] private bool countDown = true; // Are we counting down? Or up?
    [SerializeField] private float timerDuration = 5.00f; //How long we want the timer to run
    
    //How long we want "1 second" to be. Used used in timeRemaining = timerIntervalBase/timerIntervalVariable
    [SerializeField] private float timerIntervalVariable = 1f; 
    private const float timerIntervalBase = 1f; 
    
    [Header ("Timer UI")]
    [SerializeField] private TMP_Text timerText; //Component rendering text
    [SerializeField] private AudioSource timerOnSound = null; //Component for audio while timer active
    [SerializeField] private AudioSource timerEndSound = null; //Component for audio while timer end

    
    [Header ("Timer Events")]
    [SerializeField] private UnityEvent OnStartEvent = null; //Event to trigger on start
    [SerializeField] private UnityEvent OnEndEvent = null; //Event to trigger on end
    
    private float timeRemaining; //Place to store the reminaing time for this Timer

    private DataManager dataManager; // Referencing Data Manager Single

    // Start is called before the first frame update
    void Start()
    {
        //Data manager instance
        dataManager = DataManager.Instance;

        OnStartEvent.Invoke(); // Outbound of OnStartEvent
        // Trigger playing of timer audio, if not null
        if (timerOnSound != null)
        {
        timerOnSound.Play(); // Trigger playing of timer audio
            if (dataManager.debugOnInfo == true)
            {
            Debug.Log("timerOnSound played");
            }
        }
            // If we're counting down, start from the timerDuration, else reverse
            if (countDown == true)
            {
                timeRemaining = timerDuration; 
            }
            else
            {
                timeRemaining = 0;
            }
        UpdateTimerText(); // Update timer text before starting Coroutine
        StartCoroutine(CountdownCoroutine());

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Timer Start Complete. Timer duration: " + timerDuration);
        }
    }

    IEnumerator CountdownCoroutine()
    {
        // Giving the option to have a second be more or less than 1 second
        // Then counting down on (countDown == true) and the reverse on false
        float waitTime = timerIntervalBase/timerIntervalVariable;
        if (countDown == true)
        {
            while (timeRemaining > 0)
            {
            yield return new WaitForSeconds(waitTime);
                timeRemaining -= timerIntervalBase;    
                UpdateTimerText();
            }
        }
        else
        {
            while (timeRemaining < timerDuration)
            {
            yield return new WaitForSeconds(waitTime);
                timeRemaining += timerIntervalBase;    
                UpdateTimerText();
            }
        }
        
        CountdownEnd();
    }

    // Update text if there is one and debug
    void UpdateTimerText()
    {
        timerText.text = timeRemaining.ToString("0");

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Timer CountdownCoroutine. Time remaining: " + timeRemaining.ToString("0"));
        }
    }

    // Update text if there is one and debug
    public void ForceTimerEnd()
    {
    if (countDown == true)
        {
            timeRemaining = 0;
        }
    else
        {
            timeRemaining = timerDuration;
        }
        
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("ForceTimerEnd Called: Timer Forced to end");
        }
        
    StopAllCoroutines();
    CountdownEnd();
    }

    // When the timer ends, outbound on End Event, play end sound, debug
    void CountdownEnd()
    {
        OnEndEvent.Invoke();
        // Trigger playing of timer audio, if not null
        if (timerEndSound != null)
        {
        timerEndSound.Play();
            if (dataManager.debugOnInfo == true)
            {
            Debug.Log("timerEndSound played");
            }
        }

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Timer CountdownEnd");
        }
    }
}
