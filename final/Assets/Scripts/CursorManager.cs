using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Attach this script to a GameObject with a Collider, then mouse over the object to see your cursor change.
public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private Vector2 cursorsHotSpot;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, cursorsHotSpot, cursorMode);
    }

    public void Hide()
    {
        Cursor.visible = false;
    }
}