using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ScrollViewWriterReader : MonoBehaviour
{
    private enum BehaviorTypes { Submit, Select }; // This relates to behaviorTypes below.

    [Header("Content Selector")]
    [Tooltip("Select the ScriptableObject containing the data for the ScrollView")]
    [SerializeField] private ScriptableObject scrollViewDataStore;

    [Tooltip("Select the prefab for the parent ScrollView Type to create")]
    [SerializeField] private GameObject scrollViewType;

    [Tooltip("Select the prefab for the child Content Item to create")]
    [SerializeField] private GameObject contentItemType;

    private GameObject selectedOption; // A flag visual treatment on selected/clicked option

    private Transform grandParent;

    [Header("Behaviors")]
    [Tooltip("1. Select: Highlights the text with selectTextColor, writes data out to the selected Source, calls onSelect Event \n 2. Submit: Writes data out to the selected Source, calls onSelect Event")]
    [SerializeField] private BehaviorTypes behaviorTypes;

    [Header("Parent Position Offsets")]
    [SerializeField] private float scrollViewPositionX = 0;
    [SerializeField] private float scrollViewPositionY = 0;

    [Header("Parent Relative Scaling")]
    [SerializeField] private float scrollViewScaleX = 1f;
    [SerializeField] private float scrollViewScaleY = 1f;
    [SerializeField] private float scrollViewScaleZ = 1f;

    [Header("Text Styling")]
    [SerializeField] private Color32 defaultTextColor= new Color32(19,15,15,130);
    [SerializeField] private Color32 hoverTextColor= new Color32(19,15,15,190);
    [SerializeField] private Color32 selectTextColor= new Color32(19,15,15,255);
    private Color32 tempTextColor;

    [Tooltip("The percentage increase/decrease in font size when selected (1 = 100%, 0.5 = 50%, 1.5 = 150% etc)")]
    [SerializeField] private float selectFontSizePercentage = 0.9f;
    [SerializeField] private int defaultFontSize = 220;

    [Header("Image Styling")]
    [SerializeField] private Color32 defaultImageColor = new Color32(255,255,255,155);
    [SerializeField] private Color32 hoverImageColor = new Color32(255,255,255,205);
    [SerializeField] private Color32 selectImageColor = new Color32(255,255,255,255);
    [SerializeField] private float selectImageSizePercentage = 1.1f;
    
    private Vector3 localScaleChange;
    private Color32 tempImageColor;

    [Header("Outbound Events")]
    [SerializeField] public UnityEvent OnSelectEvent = new UnityEvent();
    [SerializeField] public UnityEvent OnSubmitEvent = new UnityEvent();

    private DataManager dataManager;
    private DialogueScriptWrapper dialogueScriptWrapper;

    // ! There is a bug which occurs when this script is in the first scene to load.
        // ! If that becomes a requirement, you'll need to fix it.
    void Start()
    {
        grandParent = transform;

        if (grandParent == null && dataManager.debugOnWarn == true)
        {
            Debug.LogWarning("Grandparent Object is null on Scroll View Writer Reader");
        }

        dataManager = DataManager.Instance;
        dialogueScriptWrapper = DialogueScriptWrapper.Instance;
        ScrollViewWriter();

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Scroll View Writer Reader Start Complete");
        }
    }

    void ScrollViewWriter()
    {
        GameObject parentScrollView = Instantiate(scrollViewType, grandParent);
        RectTransform parentRectTransform = parentScrollView.GetComponent<RectTransform>();
        parentRectTransform.anchoredPosition = new Vector2(scrollViewPositionX, scrollViewPositionY);
        parentScrollView.transform.localScale = new Vector3(scrollViewScaleX, scrollViewScaleY, scrollViewScaleZ);

        if (parentScrollView == null)
        {
            Debug.LogError("parentScrollView could not be Found");
            return;
        }
        grandParent = transform;
        ScrollRect highestScrollableArea = parentScrollView.GetComponent<ScrollRect>();
        if (highestScrollableArea == null)
        {
            Debug.LogError("highestScrollableArea in parentScrollView could not be Found");
            return;
        }

        Transform contentArea = parentScrollView.transform.Find("Viewport/Content");
        if (contentArea == null)
        {
            Debug.LogError("parentScrollView contentArea could not be Found");
            return;
        }
        
        // If we're handling an array of strings to a single string
        if (scrollViewDataStore is TextFieldOptions textFieldOptions)
        {
            CreateTextOptions(textFieldOptions.options, contentArea, highestScrollableArea);
        }
        // If we're handling an array of Render Textures to a single Material
        else if (scrollViewDataStore is RenderTextureToMaterialFieldOptions renderTextureToMaterialFieldOptions)
        {
            CreateRenderTextureOptions(renderTextureToMaterialFieldOptions.options, contentArea, highestScrollableArea);
        }
        // If we're handling an array of Render Textures to a single GameObject
        else if (scrollViewDataStore is RenderTextureToGameObjectFieldOptions renderTextureToGameObjectFieldOptions)
        {
            CreateRenderTextureOptions(renderTextureToGameObjectFieldOptions.options, contentArea, highestScrollableArea);
        }
        else if (scrollViewDataStore == null)
        {
            Debug.LogError("ScriptableObject is not assigned in editor");
        }
        else
        {
            Debug.LogError("Other ScriptableObject type than expected");
        }
    }

    void CreateTextOptions(string[] options, Transform content, ScrollRect scrollRect)
    {
        foreach (string option in options)
        {
            GameObject childScrollViewText = Instantiate(contentItemType, content);
            EventHandler eventHandler = childScrollViewText.AddComponent<EventHandler>();
            eventHandler.BindEvents(onPointerEnter: HandlePointerEnter, onPointerExit: HandlePointerExit, onPointerClick: HandlePointerClick);

            ScrollViewDragOverride scrollViewDragOverride = childScrollViewText.AddComponent<ScrollViewDragOverride>();
            scrollViewDragOverride.scrollRect = scrollRect;

            TMP_Text innerText = childScrollViewText.GetComponentInChildren<TextMeshProUGUI>();
            innerText.fontSize = defaultFontSize;
            innerText.text = option;

            if (dataManager.debugOnInfo == true)
            {
                Debug.Log("Created parentScrollView.childScrollViewText.innerText.text, with value" + innerText.text + "\n Total Created:" + content.transform.childCount);
            }
        }
    }

    void CreateRenderTextureOptions(RenderTexture[] options, Transform content, ScrollRect scrollRect)
    {
        foreach (RenderTexture option in options)
        {
            GameObject childScrollViewRawImage = Instantiate(contentItemType, content);
            EventHandler eventHandler = childScrollViewRawImage.AddComponent<EventHandler>();
            eventHandler.BindEvents(onPointerEnter: HandlePointerEnter, onPointerExit: HandlePointerExit, onPointerClick: HandlePointerClick);

            ScrollViewDragOverride scrollViewDragOverride = childScrollViewRawImage.AddComponent<ScrollViewDragOverride>();
            scrollViewDragOverride.scrollRect = scrollRect;

            RawImage rawImage = childScrollViewRawImage.GetComponentInChildren<RawImage>();
            rawImage.color = defaultImageColor;
            rawImage.texture = option;

            if (dataManager.debugOnInfo == true)
            {
                Debug.Log("Created parentScrollView.childScrollViewRawImage.texture.name, with value" + rawImage.name + "\n Total Created:" + content.transform.childCount);
            }
        }
    }

    // TODO: 2406026 : There is a bug here which means that when the player selects an option,
    // the temp color is only applied when the player returns to onEnter on the option.
    // I know how to fix it, but it'll have to wait until I see how we progress with the 
    // rest of the game
    public void HandlePointerEnter(GameObject target, PointerEventData data)
    {
        TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
        RawImage rawImage = target.GetComponentInChildren<RawImage>();
        if (innerText != null)
        {
            if (target != selectedOption)
            {
                tempTextColor = innerText.color;
                innerText.color = hoverTextColor;
            }
        }
        else if (rawImage != null)
        {
            if (target != selectedOption)
            {
                tempImageColor = rawImage.color;
                rawImage.color = hoverImageColor;
            }
        }
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Enters: " + target.name);
        }
    }

    public void HandlePointerExit(GameObject target, PointerEventData data)
    {
        TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
        RawImage rawImage = target.GetComponentInChildren<RawImage>();
        if (innerText != null && target != selectedOption)
        {
            innerText.color = tempTextColor;
        }
        else if (rawImage != null && target != selectedOption)
        {
            rawImage.color = tempImageColor;
        }
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Exit: " + target.name);
        }
    }

    public void HandlePointerClick(GameObject target, PointerEventData data)
    {
        TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
        TMP_Text[] allInnerTexts = target.transform.parent.GetComponentsInChildren<TMP_Text>();
        RawImage rawImage = target.GetComponentInChildren<RawImage>();
        RawImage[] allRawImages = target.transform.parent.GetComponentsInChildren<RawImage>();
        localScaleChange = new Vector3(selectImageSizePercentage,selectImageSizePercentage,selectImageSizePercentage);

        switch (behaviorTypes)
        {
            case BehaviorTypes.Select:
                {
                    if (allInnerTexts != null)
                    {
                        foreach (TMP_Text allInnerText in allInnerTexts)
                        {
                            allInnerText.fontStyle &= ~FontStyles.Underline;
                            allInnerText.fontStyle &= ~FontStyles.Bold;
                            allInnerText.color = defaultTextColor;
                            allInnerText.fontSize = defaultFontSize;
                        }

                        if (innerText != null)
                        {
                            selectedOption = target;
                            innerText.color = selectTextColor;
                            innerText.fontStyle = FontStyles.Underline | FontStyles.Bold;
                            innerText.fontSize = Mathf.RoundToInt(defaultFontSize * selectFontSizePercentage);
                            WriteOutText(innerText.text);
                            if (dataManager.debugOnInfo == true)
                            {
                                Debug.Log("Pointer Clicked: " + target.name + " Selected text " + innerText.text);
                            }
                        }
                    }
                    
                    if (allRawImages != null) // These need to be kept as two independent ifs
                    {
                        foreach (RawImage allRawImage in allRawImages)
                        {
                            allRawImage.color = defaultImageColor;
                            allRawImage.rectTransform.localScale = Vector3.one;
                        }

                        if (rawImage != null)
                        {
                            selectedOption = target;
                            rawImage.rectTransform.localScale = localScaleChange;
                            rawImage.color = selectImageColor;
                            // WriteOut(innerText.text); -- Replace this line
                            if (dataManager.debugOnInfo == true)
                            {
                                Debug.Log("Pointer Clicked: " + target.name + "\nSelected image " + rawImage.name);
                            }
                        }
                    }
                    OnSelectEvent.Invoke();

                    break;
                }

            case BehaviorTypes.Submit:
            {
                if (innerText != null)
                {
                    WriteOutText(innerText.text);

                    if (dataManager.debugOnInfo == true)
                    {
                        Debug.Log("Pointer Clicked: " + target.name + "\nSubmitted text " + innerText.text);
                    }
                }
                
                if (rawImage != null) //replace this section
                {
                    // Define logic
                    if (dataManager.debugOnInfo == true)
                    {
                        Debug.Log("Pointer Clicked: " + target.name + "\nSubmitted image " + rawImage.name);
                    }
                }
                OnSubmitEvent.Invoke();

                break;
            }
        }
    }

// TODO: Replace with three functions?
    public void WriteOutText(string outputText)
    {
        if (scrollViewDataStore is TextFieldOptions textFieldOptions)
        {
            textFieldOptions.selection = outputText;

            if (textFieldOptions.name == "Player Name")
            {
                dialogueScriptWrapper.UpdateCharacterName("PlayerCharacter", textFieldOptions.selection);
            } 
            Debug.Log("Selected: " + textFieldOptions.selection);
        }
    }

    //     if (scrollViewDataStore is TextFieldOptions textFieldOptions)
    //     {
    //         textFieldOptions.selection = outputText;
    //     }
    //     else if (scrollViewDataStore is RenderTextureToMaterialFieldOptions renderTextureToMaterialFieldOptions)
    //     {
    //         // Define logic
    //     }
    //     else if (scrollViewDataStore is RenderTextureToGameObjectFieldOptions renderTextureToGameObjectFieldOptions)
    //     {
    //         // Define logic
    //     }

    //     if (scrollViewDataStore.name == "Player Name")
    //     {
    //         dialogueScriptWrapper.UpdateCharacterName("PlayerCharacter", outputText);
    //     }

    //     Debug.Log("Selected: " + outputText);
    // }
}