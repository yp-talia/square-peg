//I'm going to try to keep all UI event outbounds in one place, I think this file might just...
//end up being forwarding for all the basic iPointer* , but let's see how it goes.

//This file is a mix of loads of sources, there doesn't appear to be a...
//single best way to do this:
    // - https://ryanjmccoach.medium.com/unity-ui-scroll-view-1a38758ebc03
    // - https://forum.unity.com/threads/scrollview-event.1046960/
    // - https://docs.unity3d.com/Packages/com.unity.ugui@3.0/manual/MessagingSystem.html
    // - https://www.youtube.com/watch?time_continue=100&v=p7bQ5sMqpqg&embeds_referring_euri=https://www.google.com/search?q=unity+ui+evens&rlz=1C5CHFA_enGB1033GB1033&oq=unity+ui+evens&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIHCAEQ&source_ve_path=MTM5MTE3LDI4NjY2&feature=emb_logo
    // - https://docs.unity3d.com/ScriptReference/Events.UnityEvent.html
    // - https://docs.unity3d.com/Packages/com.unity.ugui@3.0/api/UnityEngine.EventSystems.html
    // - https://www.youtube.com/watch?v=7_dyDmF0Ktw
    // - https://www.youtube.com/watch?time_continue=212&v=Y8A5z0FmmS8&embeds_referring_euri=https://www.google.com/search?q=unity+events+mouse+up+mouse+down+mouse+enter&sca_esv=8b154fb423ea49e7&rlz=1C5CHFA_enGB1033GB1033&source_ve_path=MTM5MTE3LDEzOTExNywyODY2Ng&feature=emb_logo

//UPDATE INSTRUCTIONS FOR THE BELOW:
    //1. Add a new Interface https://docs.unity3d.com/Packages/com.unity.ugui@3.0/api/UnityEngine.EventSystems.html
    //2. Add a private System Action
    //3. Add this System Action to BindEvents params
    //4. Reference the relevant Method. e.g. https://docs.unity3d.com/Packages/com.unity.ugui@3.0/api/UnityEngine.EventSystems.IPointerDownHandler.html 
    //4.1 Refer to the gameObject broadcasting the event with this....
    //5. Setup public function...
    //5.1 call private function
    //5.2 include logging

// Pointer Events implemented: 
// -- Enter/Exit UI Element with Binding
// -- Click on, Start click, End click (Down/Up)
// -- Starting Drag, Dragging, Finished Drag

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
                            IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, 
                            IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private System.Action<GameObject, PointerEventData> onPointerEnter;
    private System.Action<GameObject, PointerEventData> onPointerExit;
    private System.Action<GameObject, PointerEventData> onPointerDown;
    private System.Action<GameObject, PointerEventData> onPointerUp;
    private System.Action<GameObject, PointerEventData> onPointerClick;
    private System.Action<GameObject, PointerEventData> onBeginDrag;
    private System.Action<GameObject, PointerEventData> onDrag;
    private System.Action<GameObject, PointerEventData> onEndDrag;
    private DataManager dataManager;

    void Start()
    {
        dataManager = DataManager.Instance;
        
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Event Handler Start Complete");
        }
    }
    // I'm setting all of these to null by default (see Optional Arguments) so I don't have to...
    // call all of the events unless I need to. Partially to tidy up the code, mainly to fix a bug where...
    // clickable GameObjects are not scrollable and visa versa 
    // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/named-and-optional-arguments

    // FUTURE ME: I can't believe that Unity would implement a system where click events block drag events...
    // but I've already spent most of a day on this, and nothing else appears to work
    public void BindEvents(System.Action<GameObject, PointerEventData> onPointerEnter = null, 
                            System.Action<GameObject, PointerEventData> onPointerExit = null, 
                            System.Action<GameObject, PointerEventData> onPointerDown = null, 
                            System.Action<GameObject, PointerEventData> onPointerUp = null, 
                            System.Action<GameObject, PointerEventData> onPointerClick = null, 
                            System.Action<GameObject, PointerEventData> onBeginDrag = null, 
                            System.Action<GameObject, PointerEventData> onDrag = null, 
                            System.Action<GameObject, PointerEventData> onEndDrag = null)
    {
        this.onPointerEnter = onPointerEnter;
        this.onPointerExit = onPointerExit;
        this.onPointerDown = onPointerDown;
        this.onPointerClick = onPointerClick;
        this.onPointerUp = onPointerUp;
        this.onBeginDrag = onBeginDrag;
        this.onDrag = onDrag;
        this.onEndDrag = onEndDrag;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Enter: " + gameObject.name);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Exit: " + gameObject.name);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Down: " + gameObject.name);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Up: " + gameObject.name);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onPointerClick?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Click: " + gameObject.name);
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Began Drag: " + gameObject.name);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Dragging: " + gameObject.name);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Ended Drag: " + gameObject.name);
        }
    }
} 
