//Related documentation and sources:
    // Documentation: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/index.html
    // Tutorial: https://www.youtube.com/watch?v=lclDl-NGUMg

//Notes for future me:
    // When you want to implement click and drag: 
        // https://forum.unity.com/threads/new-input-system-mouse-press-and-hold-drag-and-move.1179283/
        // https://forum.unity.com/threads/implement-a-mouse-drag-composite.807906/

//UPDATE INSTRUCTIONS FOR THE BELOW:
    //1. Add a new string for the action name
    //2. Add a new InputAction
    //3. Add a new Input or Trigger to store the value if interaction is used
    //4. Find the Input Action
    //5. Register the input action
    //6. Set Enabling for Action
    //7. Set Disabling for Action
    //8. Set Debug for Action

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [Header ("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;
    
    [Header ("Action Map Name References")]
    [SerializeField] private string actionMapName = "Player";

    //Group Actions in the following ways:
        // Button/Click/Holds (where's a single possible state) actions - bools
        // Hold/Press point actions -- floats
        // Value actions
        // Pass-through actions

    //Making these accessible in the editor
    // ... and making them easily renamable if needed
    
    [Header ("Action Name References")]
    [SerializeField] private string exit = "Exit";
    [SerializeField] private string pause = "Pause";
    // [SerializeField] private string fullscreen = "Fullscreen";
    [SerializeField] private string click = "Click";
    [SerializeField] private string point = "Point";

    // Updating this to handle multiple ActionNameMaps for an Action Name
    private InputAction exitAction;
    private InputAction pauseAction;
    // private InputAction fullscreenAction;
    private InputAction clickAction;
    private InputAction pointAction;

    //FUTURE ME: Remember bool for buttons, Vector2 for directions
    public bool ExitTriggered {get; private set;} 
    public bool PauseTriggered {get; private set;} 
    // public bool FullscreenTriggered {get; private set;} 
    public bool ClickTriggered {get; private set;} 
    public Vector2 PointInput {get; private set;} 

    //References, Instancing and Flags
    public static InputManager Instance {get; private set;}
    private DataManager dataManager;

    //Singleton pattern below
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    
    exitAction = playerControls.FindActionMap(actionMapName).FindAction(exit);
    pauseAction = playerControls.FindActionMap(actionMapName).FindAction(pause);
    // fullscreenAction = playerControls.FindActionMap(actionMapName).FindAction(fullscreen);
    clickAction = playerControls.FindActionMap(actionMapName).FindAction(click);
    pointAction = playerControls.FindActionMap(actionMapName).FindAction(point);
    
    // We need to register that the above actions happened, so...
    RegisterInputActions();
    
    // Instance of Data Manager
    dataManager = DataManager.Instance;

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Input Manager Awake Complete");
        }
    }

    //Registering the various states for an input action and some light debug
    void RegisterInputActions()
    {
        exitAction.performed += context => ExitTriggered = true;
        exitAction.canceled += context => ExitTriggered = false;

        pauseAction.performed += context => PauseTriggered = true;
        pauseAction.canceled += context => PauseTriggered = false;

        // fullscreenAction.performed += context => FullscreenTriggered = true;
        // fullscreenAction.canceled += context => FullscreenTriggered = false;

        clickAction.performed += context => ClickTriggered = true;
        clickAction.canceled += context => ClickTriggered = false;
        // Only applies to the UI action map
        if (actionMapName == "UI")
        {
            pointAction.performed += context => PointInput = context.ReadValue<Vector2>();
            pointAction.canceled += context => PointInput = Vector2.zero;
        }
    }

    // Input System is event based, so we need to Enable/Disable these
    // I am refactoring this, because how i'd have to write it otherwise...
    // is just silly... see helper function below
    void OnEnable()
    {
        exitAction.Enable();
        pauseAction.Enable();
        // fullscreenAction.Enable();
        clickAction.Enable();
                // Only applies to the UI action map
        if (actionMapName == "UI")
            {
            pointAction.Enable();
            }
    }
    void OnDisable()
    {
        exitAction.Disable();
        pauseAction.Disable();
        // fullscreenAction.Disable();
        clickAction.Disable();
        if (actionMapName == "UI")
            {
            pointAction.Disable();
            }
    }

    void Update()
    {
        if (dataManager.debugOnInfo == true)
        {
            if (ExitTriggered == true)
            {
                Debug.Log("Exit performed");
            }
            if (PauseTriggered == true)
            {
                Debug.Log("Pause performed");
            }
            // if (FullscreenTriggered == true)
            // {
            //     Debug.Log("Fullscreen performed");
            // }
            if (ClickTriggered == true)
            {
                Debug.Log("Click performed");
            }
            if (PointInput != Vector2.zero)
            {
                Debug.Log("Point performed at:" + PointInput);
            }
        }
    }
}
