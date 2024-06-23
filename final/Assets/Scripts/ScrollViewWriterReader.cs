//The GameObjects this script creates needs a parent object to write to...
//I suggest placing it on a panel or similar rectangular GameObject to make...
//positioning easier

// This class is quite dense, so here's the summary:
    // Calling: ScrollViewWriter will create a scrollview with the content provided
    // If you use EventHandler and eventHandler.BindEvents (example below) you can send/receive events based on Inputs
        // EventHandler eventHandler = childScrollViewText.AddComponent<EventHandler>();
        // eventHandler.BindEvents(HandlePointerEnter, HandlePointerExit, HandlePointerUp);
    // There is an event which can be send when the user selects an option

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class ScrollViewWriterReader : MonoBehaviour
{
    [Header ("Content Selector")]
    [Tooltip("Select the prefab for the parent ScrollView Type to create")]
    [SerializeField] private GameObject scrollViewType;
    [Tooltip("Select the prefab for the child Content Item to create")]
    [SerializeField] private GameObject contentItemType;

    //This the panel/rectangular GameObject I mention up top
    private Transform grandParent;
    
    [Header ("Parent Position Offsets")]
    [SerializeField] private float scrollViewPositionX = 0;
    [SerializeField] private float scrollViewPositionY = 0;
    [SerializeField] private float scrollViewPositionZ = 0;

    [Header ("Text Styling")]
    //FUTURE ME: For some reason every time I tried to set a default in code it errored - leave to editor.
    [SerializeField] private Color32 defaultTextColor;
    [SerializeField] private Color32 hoverTextColor;
    
    [Header ("Sources")]

    [Tooltip("Select where the data is stored to display in this ScrollView")]
    [SerializeField] private TextFieldOptions scrollViewDataStore;

    [Header ("Outbound Events")]
    
    [Tooltip("Select where the data is stored to display in this ScrollView")]
    [SerializeField] public UnityEvent OnSelectEvent = new UnityEvent();

    // Place to store the possible text options for the childScrollViewText
    private string[] options;
    
    // Place to store the selected options from the childScrollViewText
    private string selection;

    //Data Manager for logging
    private DataManager dataManager;

    // This was going to be used to detect which option the player has visible...
    // I might come back to it, but dropping for now
    
    // private int scrollViewCurrentSelected = -1;

    void Start()
    {
    
    // The main GameObject that everything writes under is the grandParent (because I forgot and had to find...
    // it above parent)... also Null checking
    grandParent = transform;

        if (grandParent == null && dataManager.debugOnWarn == true)
        {
            Debug.LogWarning("Grandparent Object is null on Scroll View Writer Reader");
        }
        
    // Instance of Data Manager
    dataManager = DataManager.Instance;

    // Getting the data from the ScriptableObject... and null checking
        if (scrollViewDataStore != null)
        {
            options = scrollViewDataStore.textOptions;
            selection = scrollViewDataStore.selection;
        }
        else
        {
            if (dataManager.debugOnWarn == true)
            {
                Debug.LogWarning("Scriptable Object is null on Scroll View Writer Reader");
            }
        }

    // Call function to create ScrollView and write content
    ScrollViewWriter();

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Scroll View Writer Reader Start Complete");
        }
    }

    void ScrollViewWriter()
    {
        //Create parent from prebab - scrollViewType in GrandParent GameObject, and check for null
        GameObject parentScrollView = Instantiate(scrollViewType, grandParent);
            if (parentScrollView == null)
            {
                Debug.LogError("parentScrollView could not be Found");
                return;
            }
        //Find the Content Object in the parent and check for null
        Transform contentArea = parentScrollView.transform.Find("Viewport/Content");
            if (contentArea == null)
            {
                Debug.LogError("parentScrollView contentArea could not be Found");
                return;
            }
        // Now Create a new Text Object in the Content Object, 
        // update the text in that Object with the current option from options
        // repeat for the length of the array of options
        foreach (string option in options)
        {
            GameObject childScrollViewText = Instantiate(contentItemType, contentArea);
            childScrollViewText.transform.localPosition = new Vector3(scrollViewPositionX, scrollViewPositionY, scrollViewPositionZ);

            //Adding event handler (outbound), Binding Event Handler Handling (inbound)
            EventHandler eventHandler = childScrollViewText.AddComponent<EventHandler>();
            eventHandler.BindEvents(HandlePointerEnter, HandlePointerExit, HandlePointerUp);

            //Writing the option to the text within childScrollViewText
            TMP_Text innerText = childScrollViewText.GetComponentInChildren<TextMeshProUGUI>();
            innerText.text = option;

            // Logging out the value created and total created if Info Logging enabled
            if (dataManager.debugOnInfo == true)
            {
                Debug.Log("Created parentScrollView.childScrollViewText.innerText.text, with value" + innerText.text + "/n Total Created:" + contentArea.transform.childCount);
            }
        }

    }

    // These function can do a lot more than it is currently, as they have access to the...
    // entire GameObject sending the event and PointerEventData
    //https://docs.unity3d.com/Packages/com.unity.ugui@3.0/api/UnityEngine.EventSystems.PointerEventData.html

    // When the player's mouse enters childScrollViewText, change colour to hoverColor
    public void HandlePointerEnter(GameObject target, PointerEventData data)
    {
        TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
        innerText.color = hoverTextColor;
        Debug.Log("Pointer Enters: " + target);
    }
        // When the player's mouse exits childScrollViewText, change colour to default
    public void HandlePointerExit(GameObject target, PointerEventData data)
    {
        TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
        innerText.color = defaultTextColor;
        Debug.Log("Pointer Exit: " + target);
    }
    //When the user finishes a click, call WriteTextOut to send data...
    // and stop the timer
    public void HandlePointerUp(GameObject target, PointerEventData data)
    {
        TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
        WriteTextOut(innerText.text);
            OnSelectEvent.Invoke();
        Debug.Log("Point Up: " + target);
    }

    public void WriteTextOut(string outputText)
    {
        // A possible feature extension automatically select the currently visible...
        //option if none have been selected.. but leaving this for now.
        scrollViewDataStore.selection = outputText;
        Debug.Log("Selected: " + scrollViewDataStore.selection);
    }
}
