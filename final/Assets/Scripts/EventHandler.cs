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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    private System.Action<GameObject, PointerEventData> onPointerEnter;
    private System.Action<GameObject, PointerEventData> onPointerExit;
    private System.Action<GameObject, PointerEventData> onPointerUp;
    private DataManager dataManager;

    void Start()
    {
        dataManager = DataManager.Instance;
        
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Event Handler Start Complete");
        }
    }
    public void BindEvents(System.Action<GameObject, PointerEventData> onPointerEnter, System.Action<GameObject, PointerEventData> onPointerExit, System.Action<GameObject, PointerEventData> onPointerUp)
    {
        this.onPointerEnter = onPointerEnter;
        this.onPointerExit = onPointerExit;
        this.onPointerUp = onPointerUp;
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

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke(gameObject, eventData);
        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Pointer Up: " + gameObject.name);
        }
    }
} 
