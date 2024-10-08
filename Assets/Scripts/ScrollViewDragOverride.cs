// Adapted version of: https://forum.unity.com/threads/child-objects-blocking-scrollrect-from-scrolling.311555/#post-2092347
// Fixing the Text Drag Scroll Event being captured but ScrollView not acting on Scroll
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewDragOverride : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private DataManager dataManager;

    public ScrollRect scrollRect;

    void Start()
    {
        dataManager = DataManager.Instance;

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("ScrollView Drag Override Start Complete");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnBeginDrag(eventData);
            if (dataManager.debugOnInfo == true)
            {
            Debug.Log("ScrollViewDragOverride - OnBeginDrag");
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnDrag(eventData);
            if (dataManager.debugOnInfo == true)
            {
            Debug.Log("ScrollViewDragOverride - OnDrag");
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnEndDrag(eventData);
            if (dataManager.debugOnInfo == true)
            {
            Debug.Log("ScrollViewDragOverride - OnEndDrag");
            }
        }
    }
}