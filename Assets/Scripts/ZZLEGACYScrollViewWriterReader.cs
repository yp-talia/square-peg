// !!!! 240625 -- I'm going to completely refactor this file.
// !!!! 240626 -- Content of this file has been commented out to prevent errors.
    // !!!! -- leaving in repo for reference
// This file is now being renamed/classed as ZZLEGACYScrollViewWriterReader

//The GameObjects this script creates needs a parent object to write to...
//I suggest placing it on a panel or similar rectangular GameObject to make...
//positioning easier

// This class is quite dense, so here's the summary:
    // Calling: ScrollViewWriter will create a scrollview with the content provided
    // If you use EventHandler and eventHandler.BindEvents (example below) you can send/receive events based on Inputs
        // EventHandler eventHandler = childScrollViewText.AddComponent<EventHandler>();
        // eventHandler.BindEvents(HandlePointerEnter, HandlePointerExit, HandlePointerUp);
    // There is an event which can be send when the user selects an option

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
// using System;

// public class ZZLEGACYScrollViewWriterReader : MonoBehaviour
// {
//     // private enum ContentTypes{Text, RawImage}; // This relates to contentTypes below.
//     private enum BehaviorTypes{Submit, Select}; // This relates to behaviorTypes below.

//     [Header ("Content Selector")]
//     // [SerializeField] private ContentTypes contentTypes;

//     [Tooltip("Select the prefab for the parent ScrollView Type to create")]
//     [SerializeField] private GameObject scrollViewType;

//     [Tooltip("Select the prefab for the child Content Item to create")]
//     [SerializeField] private GameObject contentItemType;

//     //This the panel/rectangular GameObject I mention up top
//     private Transform grandParent;

//     [Header ("Behaviors")]

//     [Tooltip("1. Select: Highlights the text with selectTextColor, writes data out to the selected Source,  calls onSelect Event \n 2. Submit: Writes data out to the selected Source,  calls onSelect Event")]
//     [SerializeField] private BehaviorTypes behaviorTypes;
//     // [Tooltip("When the player scrolls, and that scroll is on a element in the ScrollView but not the ScrollView itself. Should it scroll?")]
//     // [SerializeField] private bool passScrollEventsToParent = true;
    
//     // private ScrollRect highestScrollableArea;
//     // private bool isDragging = false;
    
//     [Header ("Parent Position Offsets")]
//     [SerializeField] private float scrollViewPositionX = 0;
//     [SerializeField] private float scrollViewPositionY = 0;
    
//     [Header ("Parent Relative Scaling")]
//     [SerializeField] private float scrollViewScaleX = 1f;
//     [SerializeField] private float scrollViewScaleY = 1f;
//     [SerializeField] private float scrollViewScaleZ = 1f;

//     [Header ("Text Styling")]
//     //FUTURE ME: For some reason every time I tried to set a default in code it errored - leave to editor.
//     // TMP FontStyles post: https://forum.unity.com/threads/textmesh-pro-change-bold-underline-at-runtime.515623/
//     [SerializeField] private Color32 defaultTextColor;
//     [SerializeField] private Color32 hoverTextColor;
//     [SerializeField] private Color32 selectTextColor;
//     private Color32 tempTextColor;
    
//     [Tooltip("The percentage increase/decrease in font size when selected (1 = 100%, 0.5 = 50%, 1.5 = 150% etc)")]
//     [SerializeField] private float selectFontSizePercentage = 0.9f;
//     [SerializeField] private int defaultFontSize = 220;
    
//     [Header ("Sources")]

//     [Tooltip("Select where the data is stored to display in this ScrollView")]
//     [SerializeField] private TextFieldOptions scrollViewDataStore;

//     [Header ("Outbound Events")]
//     [SerializeField] public UnityEvent OnSelectEvent = new UnityEvent();
//     [SerializeField] public UnityEvent OnSubmitEvent = new UnityEvent();
    
//     //Data Manager for logging
//     private DataManager dataManager;
//     private DialogueScriptWrapper dialogueScriptWrapper;

//     // private string[] textOptions; // The text options the player can select
//     // private RenderTexture[] rawImageOptions; // The (raw) image options the player can select
//     // private string textSelection; // The text the player has selected
//     // private GameObject gameObjectSelection; // The GameObject selected via the (raw) image option
//     // private Material materialSelection;  // The Material selected via the (raw) image option

//     // This was going to be used to detect which option the player has visible...
//     // I might come back to it, but dropping for now

//     // private int scrollViewCurrentSelected = -1;

//     // !!! This file is getting increasingly hard to work with.
//     // !!! I'm going to take most of the comments out tomorrow.
//     // !!! But for now, I'm going to redefine all types based on the ScriptableObject
//     // !!! that will make everything a lot more readable.

//     void Start()
//     {
    
//     // The main GameObject that everything writes under is the grandParent (because I forgot and had to find...
//     // it above parent)... also Null checking
//     grandParent = transform;

//         if (grandParent == null && dataManager.debugOnWarn == true)
//         {
//             Debug.LogWarning("Grandparent Object is null on Scroll View Writer Reader");
//         }
        
//     // Instance of Data Manager
//     dataManager = DataManager.Instance;

//     dialogueScriptWrapper = DialogueScriptWrapper.Instance;

//     // Getting the data from the ScriptableObject... and null checking
//         if (scrollViewDataStore != null)
//         {
//             options = scrollViewDataStore.options;
//             selection = scrollViewDataStore.selection;
//         }
//         else
//         {
//             if (dataManager.debugOnWarn == true)
//             {
//                 Debug.LogWarning("Scriptable Object is null on Scroll View Writer Reader");
//             }
//         }
        

//     // Call function to create ScrollView and write content
//     ScrollViewWriter();

//         if (dataManager.debugOnInfo == true)
//         {
//             Debug.Log("Scroll View Writer Reader Start Complete");
//         }
//     }

//     // !!!!!!! READ THIS POST AND REMEMBER WHAT A PAIN THE CODE BELOW WAS BEFORE EDITING
//     // !!!!!!! AND IF YOU DO EDIT, COMMIT BEFOREHAND AND TEST FREQUENTLY
//     // !!!!!!! https://forum.unity.com/threads/child-objects-blocking-scrollrect-from-scrolling.311555/ !!!!!!!

//     void ScrollViewWriter()
//     {
//         //Create parent from prebab - scrollViewType in GrandParent GameObject, and check for null
//         GameObject parentScrollView = Instantiate(scrollViewType, grandParent);
//         //Fetch the RectTransform from the GameObject
//         RectTransform parentRectTransform = parentScrollView.GetComponent<RectTransform>();
//             parentRectTransform.anchoredPosition = new Vector2(scrollViewPositionX, scrollViewPositionY);
//             parentScrollView.transform.localScale = new Vector3(scrollViewScaleX, scrollViewScaleY, scrollViewScaleZ);
//             if (parentScrollView == null)
//             {
//                 Debug.LogError("parentScrollView could not be Found");
//                 return;
//             }
//         // Getting the Scroll Rect of the GrandParent to pass scroll events up to.
//         ScrollRect highestScrollableArea = parentScrollView.GetComponent<ScrollRect>();
//             if (dataManager.debugOnInfo == true)
//             {
//                 Debug.Log("highestScrollableArea:" + highestScrollableArea.name);
//             }
//             if (highestScrollableArea == null)
//             {
//                 Debug.LogError("highestScrollableArea in parentScrollView could not be Found");
//                 return;
//             }

//         //Find the Content Object in the parent and check for null
//         Transform contentArea = parentScrollView.transform.Find("Viewport/Content");
//             if (contentArea == null)
//             {
//                 Debug.LogError("parentScrollView contentArea could not be Found");
//                 return;
//             }

//         // Switch statements for each of the content types above (mainly so I can handle Image/RawImage/Text)...
//         // without having to copy paste into another file
//         switch(contentTypes)
//         {
//             case ContentTypes.Text:
//             {
//                 // Now Create a new Text Object in the Content Object, 
//                 // update the text in that Object with the current option from options
//                 // repeat for the length of the array of options
//                 foreach (string option in options)
//                 {
//                     GameObject childScrollViewText = Instantiate(contentItemType, contentArea);

//                     //Adding event handler (outbound), Binding Event Handler Handling (inbound)
//                     EventHandler eventHandler = childScrollViewText.AddComponent<EventHandler>();
//                     // I've modified this method to have optional arguments (i.e. they args with defaults)
//                     // use this structure onPointerClick: HandlePointerClick, 
//                     // for these arguments
//                     eventHandler.BindEvents(onPointerEnter : HandlePointerEnter, 
//                                             onPointerExit : HandlePointerExit,
//                                             onPointerClick: HandlePointerClick);
//                                         //Commented out when moving ScrollViewDragOverride to its own class
//                                             // onBeginDrag : HandleBeginDrag,
//                                             // onDrag : HandleDrag,
//                                             // onEndDrag: HandleEndDrag);
//                                             //Unused Handlers
//                                             // HandlePointerDown, HandlePointerUp
//                     // Adding more event handlers to overwrite from text to parent
//                     ScrollViewDragOverride scrollViewDragOverride = childScrollViewText.AddComponent<ScrollViewDragOverride>();
//                     scrollViewDragOverride.scrollRect = highestScrollableArea;

//                     //Writing the option to the text within childScrollViewText
//                     TMP_Text innerText = childScrollViewText.GetComponentInChildren<TextMeshProUGUI>();
//                     innerText.fontSize = defaultFontSize;
//                     innerText.text = option;

//                     // Logging out the value created and total created if Info Logging enabled
//                     if (dataManager.debugOnInfo == true)
//                     {
//                         Debug.Log("Created parentScrollView.childScrollViewText.innerText.text, with value" + innerText.text + "/n Total Created:" + contentArea.transform.childCount);
//                     }
//                 }
//             break;
//             }
//             case ContentTypes.RawImage:
//             {
//                 foreach (RenderTexture option in options)
//                 {
//                     GameObject childScrollViewRawImage = Instantiate(contentItemType, contentArea);
                    
//                     EventHandler eventHandler = childScrollViewRawImage.AddComponent<EventHandler>();
//                     eventHandler.BindEvents(onPointerEnter : HandlePointerEnter, 
//                                             onPointerExit : HandlePointerExit,
//                                             onPointerClick: HandlePointerClick);

//                     ScrollViewDragOverride scrollViewDragOverride = childScrollViewRawImage.AddComponent<ScrollViewDragOverride>();
//                     scrollViewDragOverride.scrollRect = highestScrollableArea;

//                     RawImage texture = childScrollViewRawImage.GetComponentInChildren<RawImage>();
//                     texture.texture = option;
//                     // Logging out the value created and total created if Info Logging enabled
//                     if (dataManager.debugOnInfo == true)
//                     {
//                         Debug.Log("Created parentScrollView.childScrollViewRawImage.texture.name, with value" + texture.name + "/n Total Created:" + contentArea.transform.childCount);
//                     }
//                 }
//             break;
//             }
//         }
//     }

//     // These function can do a lot more than it is currently, as they have access to the...
//     // entire GameObject sending the event and PointerEventData
//     //https://docs.unity3d.com/Packages/com.unity.ugui@3.0/api/UnityEngine.EventSystems.PointerEventData.html

//     // When the player's mouse enters childScrollViewText, change colour to hoverColor
//     public void HandlePointerEnter(GameObject target, PointerEventData data)
//     {
//         TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
//         tempTextColor = innerText.color;
//         innerText.color = hoverTextColor;
//         if (dataManager.debugOnInfo == true)
//         {
//             Debug.Log("Pointer Enters: " + target.name);
//         }
//     }
//         // When the player's mouse exits childScrollViewText, change colour to default
//     public void HandlePointerExit(GameObject target, PointerEventData data)
//     {
//         TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
//         innerText.color = tempTextColor;
//         if (dataManager.debugOnInfo == true)
//         {
//             Debug.Log("Pointer Exit: " + target.name);
//         }
//     }
    
//     //When the user finishes a click, call WriteTextOut to send data...
//     // and stop the timer

//     // Also : Enums and switch cases :) https://www.w3schools.com/cs/cs_enums.php
//     public void HandlePointerClick(GameObject target, PointerEventData data)
//     {
//         // The current innerText
//         TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
//         // All innerText (for style resets)
//             // We need to go up from the target to the parent
//             // The operators for fontStyles -- https://forum.unity.com/threads/textmesh-pro-change-bold-underline-at-runtime.515623/
//         TMP_Text[] allInnerTexts = target.transform.parent.GetComponentsInChildren<TMP_Text>();
//         switch(behaviorTypes)
//         {
//             case BehaviorTypes.Select:
//                 // For each text component of name
//                     // reset styles
//                 // Selected innerText styles/color/etc set
//                 foreach (TMP_Text allInnerText in allInnerTexts)
//                 {
//                     // FUTURE ME: Bitwise operators below: https://www.alanzucconi.com/2015/07/26/enum-flags-and-bitwise-operators/
//                         // aka this: &= ~ and |= 
//                         // Also useful: https://gist.github.com/NicholasSheehan/0c5b690e246e72b7f5558c64b83c2150
//                     allInnerText.fontStyle &= ~FontStyles.Underline;
//                     allInnerText.fontStyle &= ~FontStyles.Bold;
//                     allInnerText.color = defaultTextColor;
//                     allInnerText.fontSize = defaultFontSize;
//                 }
//                 innerText.color = selectTextColor;
//                 innerText.fontStyle = FontStyles.Underline | FontStyles.Bold;
//                 innerText.fontSize = Mathf.Round(defaultFontSize * selectFontSizePercentage);
//                 WriteTextOut(innerText.text);
                
//                 OnSelectEvent.Invoke(); 

//                 if (dataManager.debugOnInfo == true)
//                 {
//                     Debug.Log("Pointer Clicked: " + target.name + "Selected text" + innerText);
//                 }
//                 break;
//             case BehaviorTypes.Submit:
//                 WriteTextOut(innerText.text);
                
//                 OnSubmitEvent.Invoke(); 

//                 if (dataManager.debugOnInfo == true)
//                 {
//                     Debug.Log("Pointer Clicked: " + target.name + "Submitted text" + innerText);
//                 }
//                 break;
//         }
//         // }   
//     }

//     // Unused currently... but required for HandlePointerUp
//     // public void HandlePointerDown(GameObject target, PointerEventData data)
//     // {}
//     // Unused currently... but required to differentiate between click and drag
//     // public void HandlePointerUp(GameObject target, PointerEventData data)
//     // {}
//     // public void HandleBeginDrag(GameObject target, PointerEventData data)
//     // {
//     //     if (passScrollEventsToParent == true && highestScrollableArea != null)
//     //     {
//     //         highestScrollableArea.OnBeginDrag(data);
//     //         isDragging = true;
//     //         if (dataManager.debugOnInfo == true)
//     //         {
//     //             Debug.Log("Pointer Began Drag: " + target.name);
//     //         }
//     //     }
//     // }
//     // public void HandleDrag(GameObject target, PointerEventData data)
//     // {
//     //     if (passScrollEventsToParent == true && highestScrollableArea != null)
//     //     {
//     //         highestScrollableArea.OnDrag(data);
//     //         if (dataManager.debugOnInfo == true)
//     //         {
//     //             Debug.Log("Pointer Dragging: " + target.name);
//     //         }
//     //     }
//     // }
//     // public void HandleEndDrag(GameObject target, PointerEventData data)
//     // {
//     //     if (passScrollEventsToParent == true && highestScrollableArea != null)
//     //     {
//     //         highestScrollableArea.OnEndDrag(data);
//     //         isDragging = false;
//     //         if (dataManager.debugOnInfo == true)
//     //         {
//     //             Debug.Log("Pointer Ended Drag: " + target.name);
//     //         }
//     //     }
//     // }

//     public void WriteTextOut(string outputText)
//     {
//         // A possible feature extension automatically select the currently visible...
//         //option if none have been selected.. but leaving this for now.
//         scrollViewDataStore.selection = outputText;

//         if (scrollViewDataStore.name == "Player Name")
//         {
//             dialogueScriptWrapper.UpdateCharacterName("PlayerCharacter", scrollViewDataStore.selection);
//         } 
//         Debug.Log("Selected: " + scrollViewDataStore.selection);
//     }
    
// }
