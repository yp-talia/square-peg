using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Attach this script to a GameObject with a Collider, then mouse over the object to see your cursor change.
public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture1;
    [SerializeField] private Texture2D cursorTexture2;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 cursorsHotSpot;

    private DataManager dataManager;

    void Start()
    {
        dataManager = DataManager.Instance;

        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture1, cursorsHotSpot, cursorMode);

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Cursor Manager Start Complete");
        }
    }

    public void Hide()
    {
        Cursor.visible = false;
    }
    public void SetTexture1()
    {
        Cursor.SetCursor(cursorTexture1, cursorsHotSpot, cursorMode);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Cursor changed to texture 1");
        }
    }
    public void SetTexture2()
    {
        Cursor.SetCursor(cursorTexture2, cursorsHotSpot, cursorMode);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Cursor changed to texture 2");
        }
    }
}