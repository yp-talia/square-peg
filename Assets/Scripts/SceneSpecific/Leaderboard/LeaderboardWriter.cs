    // ScrollView scroll to position - https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui
    // Tweening library - https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        previousTransform.anchoredPosition = new Vector2(50, -300);
        TMP_Text previousInnerText = previousPositionText.GetComponentInChildren<TextMeshProUGUI>();
            previousInnerText.text = "Your previous hiring rank" + dataManager.previousPosition.ToString();

        GameObject currentPositionText = Instantiate(uiElement, parent);
        RectTransform currentTransform = currentPositionText.GetComponent<RectTransform>();
        currentTransform.anchoredPosition = new Vector2(600, -300);
        TMP_Text currentInnerText = currentPositionText.GetComponentInChildren<TextMeshProUGUI>();
            currentInnerText.text = "Your current hiring rank" + dataManager.currentPosition.ToString();

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Leaderboard Writer Start Complete");
        }
    }
}
