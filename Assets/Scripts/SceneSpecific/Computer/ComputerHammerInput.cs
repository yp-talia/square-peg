// I'm going to use this as the basis for many of the minigames going forward

// Documentation:
    //https://docs.unity3d.com/ScriptReference/RectTransform.html
    //https://docs.unity3d.com/ScriptReference/PolygonCollider2D.html
    //https://docs.unity3d.com/ScriptReference/CanvasGroup.html
    // https://stackoverflow.com/questions/37244471/click-and-drag-a-gameobject-in-a-overlay-canvas-in-unity
    // https://medium.com/medialesson/drag-drop-for-ui-elements-in-unity-the-simple-ish-way-9efcb4617648

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(CanvasGroup))]
public class ComputerHammerInput : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private PolygonCollider2D polygonCollider2D;
    // private Vector2 originalPosition;
    private DataManager dataManager;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        canvas = GetComponentInParent<Canvas>();
        dataManager = DataManager.Instance;

        EventHandler eventHandler = gameObject.AddComponent<EventHandler>();
        eventHandler.BindEvents(onBeginDrag: HandleBeginDrag,
                                onDrag: HandleDrag,
                                onEndDrag: HandleEndDrag);
        if (dataManager.debugOnInfo)
        {
            Debug.Log("Computer Hammer Input Awake");
        }
    }

    private void HandleBeginDrag(GameObject target, PointerEventData eventData)
    {
        if (IsPointerOverCollider(target, eventData))
        {
            canvasGroup.alpha = 0.8f;

            if (dataManager.debugOnInfo)
            {
                Debug.Log("Local: Pointer Down: " + target.name + "at" + eventData.position.x + "," + eventData.position.y);
            }
        }
    }

    private void HandleDrag(GameObject target, PointerEventData eventData)
    {
        if (IsPointerOverCollider(target, eventData))
        {
            // https://docs.unity3d.com/ScriptReference/RectTransformUtility.ScreenPointToLocalPointInRectangle.html
            // public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint);
            // When ScreenPointToLocalPointInRectangle is used from within an event handler that provides a PointerEventData object, 
            // ... the correct camera can be obtained by using PointerEventData.enterEventData (for hover functionality) or 
            // ... PointerEventData.pressEventCamera (for click functionality). This will automatically use the correct camera (or null) for the given event.
            
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

            if (dataManager.debugOnInfo)
            {
                Debug.Log("Local: Dragging: " + target.name + "at" + eventData.position.x + "," + eventData.position.y);
            }
        }
    }

    private void HandleEndDrag(GameObject target, PointerEventData eventData)
    {
        if (IsPointerOverCollider(target, eventData))
        {
            canvasGroup.alpha = 1.0f;

            if (dataManager.debugOnInfo)
            {
                Debug.Log("Local: Pointer Up: " + target.name + "at" + eventData.position.x + "," + eventData.position.y);
            }
        }
    }
    // So glad I stumbled across this -- https://docs.unity3d.com/ScriptReference/Transform.InverseTransformPoint.html
    private bool IsPointerOverCollider(GameObject target, PointerEventData eventData)
    {
        // Get position of the mouse
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(eventData.position);
        // is position of the mouse over the polygonCollider2D
        bool isOver = polygonCollider2D.OverlapPoint(localMousePosition);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Local: Pointer Over Collider: " + target.name + "at" + eventData.position.x + "," + eventData.position.y);
        }
        // Return whether mouse is over collider
        return isOver;
    }

}