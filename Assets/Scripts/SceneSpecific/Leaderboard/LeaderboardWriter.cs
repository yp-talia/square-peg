    // ScrollView scroll to position - https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui
    // Tweening library - https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676

// using System;
// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using TMPro;
// using UnityEngine.EventSystems;
// using UnityEngine.Events;
// using System.Runtime.CompilerServices;
// using UnityEngine.UIElements;


public class LeaderboardWriter : MonoBehaviour
{
    private DataManager dataManager;
    [SerializeField] private GameObject uiElement;
        void Start()
    {
        dataManager = DataManager.Instance;
        Transform parent = transform;

        GameObject previousPositionText = Instantiate(uiElement, parent);
        RectTransform previousTransform = previousPositionText.GetComponent<RectTransform>();
        previousTransform.anchoredPosition = new Vector2(50, -250);
        TMP_Text previousInnerText = previousPositionText.GetComponentInChildren<TextMeshProUGUI>();
            previousInnerText.fontSize = 60;
            previousInnerText.text = "Previous rank: " + dataManager.previousPosition.ToString();

        GameObject currentPositionText = Instantiate(uiElement, parent);
        RectTransform currentTransform = currentPositionText.GetComponent<RectTransform>();
        currentTransform.anchoredPosition = new Vector2(400, -325);
        TMP_Text currentInnerText = currentPositionText.GetComponentInChildren<TextMeshProUGUI>();
            currentInnerText.fontSize = 60;
            currentInnerText.text = "Current rank: " + dataManager.currentPosition.ToString();
        
        GameObject hiringCountText = Instantiate(uiElement, parent);
        RectTransform hiringCountTransform = hiringCountText.GetComponent<RectTransform>();
        hiringCountTransform.anchoredPosition = new Vector2(50, -425);
        TMP_Text hiringCountInnerText = hiringCountText.GetComponentInChildren<TextMeshProUGUI>();
            hiringCountInnerText.fontSize = 60;
            hiringCountInnerText.text = "Roles available:" + dataManager.successThreshold.ToString() + " role today";
        
        GameObject totalInterestedText = Instantiate(uiElement, parent);
        RectTransform totalInterestedTextTransform = totalInterestedText.GetComponent<RectTransform>();
        totalInterestedTextTransform.anchoredPosition = new Vector2(400, -500);
        TMP_Text totalInterestedInnerText = totalInterestedText.GetComponentInChildren<TextMeshProUGUI>();
            totalInterestedInnerText.fontSize = 60;
            totalInterestedInnerText.text = "Top " + dataManager.failureThreshold.ToString() + " being interviewed";

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Leaderboard Writer Start Complete");
        }
    }
}

    // [Header("Content Selector")]
    // [Tooltip("Select the ScriptableObject containing the data for the leaderboard")]
    // private LeaderboardOrdered leaderboardData;
    // [SerializeField] private ScriptableObject playerNameData;
    // public ScriptableObject playerPreviousRank;
    // public ScriptableObject playerCurrentRank;

    // [Tooltip("Select the prefab for the parent ScrollView Type to create")]
    // [SerializeField] private GameObject scrollViewType;

    // [Tooltip("Select the prefab for the child Content Item to create")]
    // [SerializeField] private GameObject contentItemType1;
    // [SerializeField] private GameObject contentItemDivider;

    // private Transform grandParent;


    // // ! There is a bug which occurs when this script is the first in the scene to load... if it becomes a necessity i'll fit it
    // void Start()
    // {
    //     dataManager = DataManager.Instance;
    //     grandParent = transform;

    //     if (grandParent == null && dataManager.debugOnWarn == true)
    //     {
    //         Debug.LogWarning("Grandparent Object is null on Scroll View Writer Reader");
    //     }

    // LeaderboardOrdered leaderboardData  = Resources.Load<LeaderboardOrdered>("ScriptableObjects/HiringLeaderboard");

    // }

    // void ScrollViewWriter()
    // {
    //     GameObject parentScrollView = Instantiate(scrollViewType, grandParent);
    //     RectTransform parentRectTransform = parentScrollView.GetComponent<RectTransform>();

    //     if (parentScrollView == null)
    //     {
    //         Debug.LogError("parentScrollView could not be Found");
    //         return;
    //     }
    //     grandParent = transform;
    //     ScrollRect highestScrollableArea = parentScrollView.GetComponent<ScrollRect>();
    //     if (highestScrollableArea == null)
    //     {
    //         Debug.LogError("highestScrollableArea in parentScrollView could not be Found");
    //         return;
    //     }

    //     Transform contentArea = parentScrollView.transform.Find("Viewport/Content");
    //     if (contentArea == null)
    //     {
    //         Debug.LogError("parentScrollView contentArea could not be Found");
    //         return;
    //     }
        
    //     string[] leaderboardWorkingData;

    //         if (leaderboardData != null)
    //     {
    //     foreach (Positions position in leaderboardData.position)
    //     {

    //     }

    //     }

    //     foreach (string leaderboardPositions.name in leaderboardPositions)
    //     {
    //         GameObject childScrollViewText = Instantiate(contentItemType, content);
    //         EventHandler eventHandler = childScrollViewText.AddComponent<EventHandler>();
    //         eventHandler.BindEvents(onPointerEnter: HandlePointerEnter, onPointerExit: HandlePointerExit, onPointerClick: HandlePointerClick);

    //         ScrollViewDragOverride scrollViewDragOverride = childScrollViewText.AddComponent<ScrollViewDragOverride>();
    //         scrollViewDragOverride.scrollRect = scrollRect;

    //         TMP_Text innerText = childScrollViewText.GetComponentInChildren<TextMeshProUGUI>();
    //         innerText.fontSize = defaultFontSize;
    //         innerText.text = option;

    //         if (dataManager.debugOnInfo == true)
    //         {
    //             Debug.Log("Created parentScrollView.childScrollViewText.innerText.text, with value" + innerText.text + "\n Total Created:" + content.transform.childCount);
    //         }
    //     }
    // }
    // // Single Function to clear associated ScriptableOption.selection field
    // public void ResetSelection()
    // {
    //     // If we're handling an array of strings to a single string
    //     if (scrollViewDataStore is TextFieldOptions textFieldOptions)
    //     {
    //         textFieldOptions.selection = null;
    //     }
    //     // If we're handling an array of Render Textures to a single Material
    //     else if (scrollViewDataStore is RenderTextureToMaterialFieldOptions renderTextureToMaterialFieldOptions)
    //     {
    //         renderTextureToMaterialFieldOptions.selection = null;
    //     }
    //     // If we're handling an array of Render Textures to a single GameObject
    //     else if (scrollViewDataStore is RenderTextureToGameObjectFieldOptions renderTextureToGameObjectFieldOptions)
    //     {
    //         renderTextureToGameObjectFieldOptions.selection = null;
    //     }
    //             // I should probably rename this, they're not Textures they're Sprites. But oh well...
    //     else if (scrollViewDataStore is TextureTagged textureTagged)
    //     {
    //         textureTagged.selection = null;
    //         textureTagged.selectionName = null;
    //         textureTagged.selectionTagName = null;
    //     }
    //     else if (scrollViewDataStore == null)
    //     {
    //         if (dataManager.debugOnWarn == true)
    //         {
    //         Debug.LogWarning("ResetSelection - ScriptableObject is not assigned in editor");
    //         }
    //     }
    //     else
    //     {
    //         if (dataManager.debugOnWarn == true)
    //         {
    //         Debug.LogWarning("ResetSelection - Other ScriptableObject type than expected");
    //         }
    //     }
    // }
// }


