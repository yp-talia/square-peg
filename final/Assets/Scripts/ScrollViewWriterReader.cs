//The GameObjects this script creates needs a parent object to write to...
//I suggest placing it on a panel or similar rectangular GameObject to make...
//positioning easier

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        
    // [Header ("Parent Position Scaler")]
    // [SerializeField] private float scrollViewScalerX = 0;
    // [SerializeField] private float scrollViewScalerY = 0;
    
    [Header ("Data Source")]

    [Tooltip("Select where the data is stored to display in this ScrollView")]
    [SerializeField] private TextFieldOptions scrollViewDataStore;

    // Place to store the possible text options for the childScrollViewText
    private string[] options;
    
    // Place to store the selected options from the childScrollViewText
    private string selection;

    //Data Manager for logging
    private DataManager dataManager;

    // Place to store which item is currently selected
    private int scrollViewCurrentSelected = -1;

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
        // Now Create a new Text Object in the Content Object with the text from the Store...
        // repeat for the length of the array of options
        foreach (string option in options)
        {
            GameObject childScrollViewText = Instantiate(contentItemType, contentArea);
            childScrollViewText.transform.localPosition = new Vector3(scrollViewPositionX, scrollViewPositionY, scrollViewPositionZ);

            TMP_Text innerText = childScrollViewText.GetComponentInChildren<TextMeshProUGUI>();

            innerText.text = option;
            // Logging out the value created and total created if Info Logging enabled
            if (dataManager.debugOnInfo == true)
            {
                Debug.Log("Created parentScrollView.childScrollViewText.innerText.text, with value" + innerText.text + "/n Total Created:" + contentArea.transform.childCount);
            }
        }

    }


}
