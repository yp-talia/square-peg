using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEditor;

// Notes about what each Method/Function does:
    // 1. ScrollViewWriter instatiates the ScrollView and depending on which ScriptableObject is loaded...
    // populates the content in the correct type of GameObject
        //1.1 CreateTextOptions - creates text values
        //1.2 CreateRenderTextureOptions - creates Raw Images
        //1.2 CreateTextureOptions - creates Images (Sprites)
    // 2. HandlePointer* sends events to EventHandler
        //2.1 HandlePointerClick is more complex, because it calls WriteOut* functions
    // 3. WriteOutText, WriteOutImage, WriteOutRawImage write back selected value to the ScriptableObjects as needed
        // These functions also call SuccessText and SuccessProcessedImage functions, which decide whether the user has completed the goal for that scrollview
    // 4. SuccessText and SuccessProcessedImage use successMatchCriteria and GameplayStates to inform...
    // MinigameTransitionHandler when a success/failure state has been reached for a ScrollView
        // Their logic is inherently related to MinigameTransitionHandler...
        //  probably best to have both scripts open when working on these functions
    //5. ResetSelection is sometimes called by functions in this script, sometimes by events to reset the values of fields...
    // in the associated ScriptableObject

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

    [SerializeField] private string[] successMatchCriteria;
    [SerializeField] private string[] failureMatchCriteria;
    
    private enum GameplayStates { isSuccess, isNotSuccess, isFailure }; // This relates to behaviorTypes below.

    private GameplayStates gameplayStates = GameplayStates.isNotSuccess;

    // Removing to move to a state based system build on GameplayStates
    // private bool successful = false;

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
    [SerializeField] public UnityEvent OnSuccessEvent = new UnityEvent();
    [SerializeField] public UnityEvent OnNotSuccessEvent = new UnityEvent();
    [SerializeField] public UnityEvent OnFailureEvent = new UnityEvent();

    private DataManager dataManager;
    private DialogueScriptWrapper dialogueScriptWrapper;

    // ! There is a bug which occurs when this script is in the first scene to load.
        // ! If that becomes a requirement, you'll need to fix it.
    void Start()
    {
        dataManager = DataManager.Instance;
        grandParent = transform;

        if (grandParent == null && dataManager.debugOnWarn == true)
        {
            Debug.LogWarning("Grandparent Object is null on Scroll View Writer Reader");
        }

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
                // I should probably rename this, they're not Textures they're Sprites. But oh well...
        else if (scrollViewDataStore is TextureTagged textureTagged)
        {
            CreateTextureOptions(textureTagged.options, contentArea, highestScrollableArea);
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

    void CreateTextureOptions(Sprite[] options, Transform content, ScrollRect scrollRect)
    {
        foreach (Sprite option in options)
        {
            GameObject childScrollViewProcessedImage = Instantiate(contentItemType, content);
            EventHandler eventHandler = childScrollViewProcessedImage.AddComponent<EventHandler>();
            eventHandler.BindEvents(onPointerEnter: HandlePointerEnter, onPointerExit: HandlePointerExit, onPointerClick: HandlePointerClick);

            ScrollViewDragOverride scrollViewDragOverride = childScrollViewProcessedImage.AddComponent<ScrollViewDragOverride>();
            scrollViewDragOverride.scrollRect = scrollRect;

            Image processedImage = childScrollViewProcessedImage.GetComponentInChildren<Image>();
            processedImage.color = defaultImageColor;
            processedImage.sprite = option;

            if (dataManager.debugOnInfo == true)
            {
                Debug.Log("Created parentScrollView.childScrollViewRawImage.texture.name, with value" + processedImage.name + "\n Total Created:" + content.transform.childCount);
            }
        }
    }

    //TODO: Updated HandlePointerEnter, HandlePointerExit
    public void HandlePointerEnter(GameObject target, PointerEventData data)
    {
        TMP_Text innerText = target.GetComponentInChildren<TMP_Text>();
        RawImage rawImage = target.GetComponentInChildren<RawImage>();
        Image processedImage = target.GetComponentInChildren<Image>();
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
        else if (processedImage != null)
        {
            if (target != selectedOption)
            {
                tempImageColor = processedImage.color;
                processedImage.color = hoverImageColor;
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
        Image processedImage = target.GetComponentInChildren<Image>();
        if (innerText != null && target != selectedOption)
        {
            innerText.color = tempTextColor;
        }
        else if (rawImage != null && target != selectedOption)
        {
            rawImage.color = tempImageColor;
        }
        else if (processedImage != null && target != selectedOption)
        {
            processedImage.color = tempImageColor;
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
        Image processedImage = target.GetComponentInChildren<Image>();
        Image[] allProcessedImages = target.transform.parent.GetComponentsInChildren<Image>();
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
                        // Removing to move to a state based system
                        // successful = false; // If the user has selected a new option, then we don't know if the option is successful yet, so reset successful
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
                
                if (allRawImages != null) // These needs to be kept as independent ifs
                {
                    foreach (RawImage allRawImage in allRawImages)
                    {
                        allRawImage.color = defaultImageColor;
                        allRawImage.rectTransform.localScale = Vector3.one;
                    }

                    if (rawImage != null)
                    {
                        // Removing to move to a state based system
                        // successful = false; // If the user has selected a new option, then we don't know if the option is successful yet, so reset successful
                        selectedOption = target;
                        rawImage.rectTransform.localScale = localScaleChange;
                        rawImage.color = selectImageColor;
                        WriteOutRawImage(rawImage);
                        if (dataManager.debugOnInfo == true)
                        {
                            Debug.Log("Pointer Clicked: " + target.name + "\nSelected image " + rawImage.name);
                        }
                    }
                }
                if (allProcessedImages != null) // These needs to be kept as independent ifs
                {
                    foreach (Image allProcessedImage in allProcessedImages)
                    {
                        allProcessedImage.color = defaultImageColor;
                        allProcessedImage.rectTransform.localScale = Vector3.one;
                    }

                    if (processedImage != null)
                    {
                        // Removing to move to a state based system
                        // successful = false; // If the user has selected a new option, then we don't know if the option is successful yet, so reset successful
                        selectedOption = target;
                        processedImage.rectTransform.localScale = localScaleChange;
                        processedImage.color = selectImageColor;
                        WriteOutImage(processedImage);
                        
                        if (dataManager.debugOnInfo == true)
                        {
                            Debug.Log("Pointer Clicked: " + target.name + "\nSelected image " + processedImage.name);
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

                if (processedImage != null) //replace this section
                {
                    // Define logic
                    if (dataManager.debugOnInfo == true)
                    {
                        Debug.Log("Pointer Clicked: " + target.name + "\nSubmitted image " + processedImage.name);
                    }
                }
                OnSubmitEvent.Invoke();

                break;
            }
        }
    }
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
            if (successMatchCriteria != null || failureMatchCriteria != null)
            {
                SuccessText(textFieldOptions.selection);
            }
        }
    }
    public void WriteOutRawImage(RawImage outputRawImage)
    {
        RenderTexture renderTexture;
        renderTexture = (RenderTexture)outputRawImage.texture;

    if (scrollViewDataStore is RenderTextureToMaterialFieldOptions renderTextureToMaterialFieldOptions)
        {
            for (int i = 0; i < renderTextureToMaterialFieldOptions.options.Length; i++)
            {
                if (renderTextureToMaterialFieldOptions.options[i] == renderTexture)
                {
                    renderTextureToMaterialFieldOptions.selection = renderTextureToMaterialFieldOptions.pairedOptions[i];
                }
            }
        }
    else if (scrollViewDataStore is RenderTextureToGameObjectFieldOptions renderTextureToGameObjectFieldOptions)
        {
            for (int i = 0; i < renderTextureToGameObjectFieldOptions.options.Length; i++)
            {
                if (renderTextureToGameObjectFieldOptions.options[i] == renderTexture)
                {
                    renderTextureToGameObjectFieldOptions.selection = renderTextureToGameObjectFieldOptions.pairedOptions[i];

                }
            }
        }
    }
    public void WriteOutImage(Image processedImage)
    {
        Sprite sprite;
        sprite = (Sprite)processedImage.sprite;

        if (scrollViewDataStore is TextureTagged textureTagged)
        {
            for (int i = 0; i < textureTagged.options.Length; i++)
            {
                if (textureTagged.options[i] == sprite)
                {
                    textureTagged.selection = textureTagged.options[i];
                    textureTagged.selectionName = textureTagged.options[i].name;
                    textureTagged.selectionTagName = textureTagged.pairedOptionTag[i].ToString();
                    if (successMatchCriteria != null || failureMatchCriteria != null)
                    {
                        SuccessProcessedImage(textureTagged.selection, textureTagged.selectionName, textureTagged.selectionTagName);
                    }
                }
            }
        }
    }
    

    void SuccessText(String inputText)
    {
        switch (gameplayStates)
        {
            case GameplayStates.isNotSuccess:
                {
                    if (successMatchCriteria != null)
                    {
                        foreach (String successMatchCriterion in successMatchCriteria)
                        {
                            if (inputText == successMatchCriterion)
                            {
                                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                                {
                                    Debug.Log("Moving to State: isSuccess. Matched " + successMatchCriterion + " on " + inputText);
                                }
                                OnSuccessEvent.Invoke();
                                gameplayStates = GameplayStates.isSuccess;
                                ResetSelection();
                                break;
                            }
                        }
                    }
                    if (failureMatchCriteria != null)
                    {
                        foreach (String failureMatchCriterion in failureMatchCriteria)
                        {
                            if (inputText == failureMatchCriterion)
                            {
                                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                                {
                                    Debug.Log("Moving to State: isFailure. Matched " + failureMatchCriterion + " on " + inputText);
                                }
                                OnFailureEvent.Invoke();
                                gameplayStates = GameplayStates.isFailure;
                                break;
                            }
                        }
                    }
                    break;
                }
            case GameplayStates.isSuccess:
                {
                    if (successMatchCriteria != null)
                    {
                        foreach (String successMatchCriterion in successMatchCriteria)
                        {
                            if (inputText == successMatchCriterion)
                            {
                                if (dataManager.debugOnInfo == true)
                                {
                                    Debug.Log("Staying in State: isSuccess. Matched " + successMatchCriterion + " on " + inputText );
                                }
                                ResetSelection(); 
                                break;
                            }
                        }
                    }
                    if (failureMatchCriteria != null)
                    {
                        foreach (String failureMatchCriterion in failureMatchCriteria)
                        {
                            if (inputText == failureMatchCriterion || inputText == failureMatchCriterion)
                            {
                                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                                {
                                    Debug.Log("Moving to State: isFailure. Matched " + failureMatchCriterion + " on " + inputText);
                                }
                                OnNotSuccessEvent.Invoke();
                                OnFailureEvent.Invoke();
                                gameplayStates = GameplayStates.isFailure;
                                break;
                            }
                        }
                    }
                    if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                    {
                        Debug.Log("Moving to State: isNotSuccess. No Match on " + inputText);
                    }
                    OnNotSuccessEvent.Invoke();
                    gameplayStates = GameplayStates.isNotSuccess;
                    break;
                }
            case GameplayStates.isFailure:
            {
                // put logic in here when you define this state
                break;
            }
        }

        // if (successful == false)
        // {
        //     foreach (String successMatchCriterion in successMatchCriteria)
        //     {
        //         if (inputText == successMatchCriterion)
        //         {
        //             if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        //             {
        //                 Debug.Log("Matched" + successMatchCriterion + " on " + inputText);
        //             }
        //             OnSuccessEvent.Invoke();
        //             successful = true;
        //             ResetSelection(); // Testing out hypothesis below
        //         }
        //     }
        // return true;
        // }
        // 

        // I need to test this, but I think if I call reset on success, then I don't need this block 
        // if (successful == true)
        // {
        //     foreach (String successMatchCriterion in successMatchCriteria)
        //     {
        //         if (inputText == successMatchCriterion)
        //         {
        //             return true;
        //         }
        //         else 
        //             OnFailureEvent.Invoke();
        //             successful = false;
        //             return false;
        //     }
        // }
    }
    // inputImage not used currently
    void SuccessProcessedImage(Sprite inputImage, String inputImageName, String inputImageTagName)
    {
        switch (gameplayStates)
        {
            case GameplayStates.isNotSuccess:
                {
                    if (successMatchCriteria != null)
                    {
                        foreach (String successMatchCriterion in successMatchCriteria)
                        {
                            if (inputImageName == successMatchCriterion || inputImageTagName == successMatchCriterion)
                            {
                                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                                {
                                    Debug.Log("Moving to State: isSuccess. Matched" + successMatchCriterion + " on " + inputImageName + " or " + inputImageTagName);
                                }
                                OnSuccessEvent.Invoke();
                                gameplayStates = GameplayStates.isSuccess;
                                ResetSelection(); // Testing out hypothesis below
                            }
                        }
                    }
                    if (failureMatchCriteria != null)
                    {
                        foreach (String failureMatchCriterion in failureMatchCriteria)
                        {
                            if (inputImageName == failureMatchCriterion || inputImageTagName == failureMatchCriterion)
                            {
                                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                                {
                                    Debug.Log("Moving to State: isFailure. Matched" + failureMatchCriterion + " on " + inputImageName + " or " + inputImageTagName);
                                }
                                OnFailureEvent.Invoke();
                                gameplayStates = GameplayStates.isFailure;
                            }
                        }
                    }
                    break;
                }
            case GameplayStates.isSuccess:
                {
                    if (successMatchCriteria != null)
                    {
                        foreach (String successMatchCriterion in successMatchCriteria)
                        {
                            if (inputImageName == successMatchCriterion || inputImageTagName == successMatchCriterion)
                            {
                                if (dataManager.debugOnInfo == true)
                                {
                                    Debug.Log("Staying in State: isSuccess. Matched" + successMatchCriterion + " on " + inputImageName + " or " + inputImageTagName);
                                }
                                ResetSelection(); // Testing out hypothesis below
                            }
                        }
                    }
                    if (failureMatchCriteria != null)
                    {
                        foreach (String failureMatchCriterion in failureMatchCriteria)
                        {
                            if (inputImageName == failureMatchCriterion || inputImageTagName == failureMatchCriterion)
                            {
                                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                                {
                                    Debug.Log("Moving to State: isFailure. Matched" + failureMatchCriterion + " on " + inputImageName + " or " + inputImageTagName);
                                }
                                OnFailureEvent.Invoke();
                                gameplayStates = GameplayStates.isFailure;
                            }
                        }
                    }
                    if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                    {
                        Debug.Log("Moving to State: isNotSuccess. No Match on " + inputImageName + " or " + inputImageTagName);
                    }
                    OnNotSuccessEvent.Invoke();
                    gameplayStates = GameplayStates.isNotSuccess;
                    break;
                }
            case GameplayStates.isFailure:
            {
                // put logic in here when you define this state
                break;
            }
        }
        // if (successful == false)
        // {
        //     foreach (String successMatchCriterion in successMatchCriteria)
        //     {
        //         if (inputImageName == successMatchCriterion || inputImageTagName == successMatchCriterion)
        //         {
        //             if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
        //             {
        //                 Debug.Log("Matched" + successMatchCriterion + " on " + inputImageName + " or " + inputImageTagName);
        //             }
        //             OnSuccessEvent.Invoke();
        //             // successful = true;
        //             ResetSelection(); // Testing out hypothesis below
        //         }
        //     }
        // return true;
        // }
        // // I need to test this, but I think if I call reset on success, then I don't need this block 

        // else if (successful == true)
        // {
        //     foreach (String successMatchCriterion in successMatchCriteria)
        //     {
        //         if (inputImageName == successMatchCriterion || inputImageTagName == successMatchCriterion)
        //         {
        //             return true;
        //         }
        //         else
        //         {
        //             OnFailureEvent.Invoke();
        //             successful = false;
        //             return false;
        //         }
        //     }
        // }
    }

    // Single Function to clear associated ScriptableOption.selection field
    public void ResetSelection()
    {
        // If we're handling an array of strings to a single string
        if (scrollViewDataStore is TextFieldOptions textFieldOptions)
        {
            textFieldOptions.selection = null;
        }
        // If we're handling an array of Render Textures to a single Material
        else if (scrollViewDataStore is RenderTextureToMaterialFieldOptions renderTextureToMaterialFieldOptions)
        {
            renderTextureToMaterialFieldOptions.selection = null;
        }
        // If we're handling an array of Render Textures to a single GameObject
        else if (scrollViewDataStore is RenderTextureToGameObjectFieldOptions renderTextureToGameObjectFieldOptions)
        {
            renderTextureToGameObjectFieldOptions.selection = null;
        }
                // I should probably rename this, they're not Textures they're Sprites. But oh well...
        else if (scrollViewDataStore is TextureTagged textureTagged)
        {
            textureTagged.selection = null;
            textureTagged.selectionName = null;
            textureTagged.selectionTagName = null;
        }
        else if (scrollViewDataStore == null)
        {
            if (dataManager.debugOnWarn == true)
            {
            Debug.LogWarning("ResetSelection - ScriptableObject is not assigned in editor");
            }
        }
        else
        {
            if (dataManager.debugOnWarn == true)
            {
            Debug.LogWarning("ResetSelection - Other ScriptableObject type than expected");
            }
        }
    }
}