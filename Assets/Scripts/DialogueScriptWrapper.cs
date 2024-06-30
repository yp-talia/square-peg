// 240621_I'm going to modify the behaviour/values of the Dialogue system here
// This file is going to be smothered in comments ...
// Because I'm getting my head around how ScriptableObjects work...
// and it all feels very counter-intuitive. Sorry FUTURE ME

// 240623_Looping back to this file, I've made the player name data stored...
// in it's own ScriptableObject, so refactoring

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Reference to the Dialogue System 
using HeneGames.DialogueSystem;

public class DialogueScriptWrapper : MonoBehaviour
{

    public static DialogueScriptWrapper Instance;
// Defining Variables
    //Name of character field
    private string prevCharacterName;
    private string curCharacterName;

    // Instance on Data Manager for logging
    private DataManager dataManager;

    [SerializeField] private TextFieldOptions characterNameStore;

    // On Start not Awake as want to run this after everything has loaded
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Dialogue Script Wrapper Destroyed");
        }

        // Instance of Data Manager
        dataManager = DataManager.Instance;

        //Pulling out the character name field
        curCharacterName = characterNameStore.selection;

        // Calling Update Character Name with the Player's Character (which will be used to...
        // lookup the Dialogue Character ScriptableObject)
        UpdateCharacterName("PlayerCharacter", curCharacterName);

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Dialogue Script Wrapper Start Complete");
        }
    }

    //Function that I can call whenever the data on a character's name changes...
    // I've written this to work on Start as well as be called elsewhere whenever a name updates
    public void UpdateCharacterName(string characterType, string characterName )
    {
        // Making the received characterName the currCharacterName if it came from external source
        curCharacterName = characterName;
        // Variable for constructed path to the character's file
        string characterPath = "Dialogue/" + characterType;
        // In order to be editable at runtime, a file needs to be Resources -- https://docs.unity3d.com/ScriptReference/Resources.Load.html

        DialogueCharacter character = Resources.Load<DialogueCharacter>(characterPath);

        if (character != null)
        {
            // Checking to see whether the name has changed
            if (curCharacterName != prevCharacterName)
            {
                //If it has changed, then update character name
                character.characterName = curCharacterName;
                prevCharacterName = character.characterName;
                // If we've got debug logging on, write message to console
                if (dataManager.debugOnInfo == true || dataManager.debugOnInfoPriority == true)
                {
                    Debug.Log("DialogueCharacter PlayerCharacter playerName set to:" + character.characterName);
                }
            }
        }
        else
            Debug.LogError("DialogueCharacter PlayerCharacter does not exist");

    }

}

