// I'm going to modify the behaviour/values of the Dialogue system here
// This file is going to be smothered in comments ...
// Because I'm getting my head around how ScriptableObjects work...
// and it all feels very counter-intuitive. Sorry FUTURE ME

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Reference to the Dialogue System 
using HeneGames.DialogueSystem;

public class DialogueScriptWrapper : MonoBehaviour
{
// Defining Variables
    //Name of character field
    private string prevCharacterName;

    // Instance of Data Manager which is were I'm storing player Data.
    // In hind-sight, this probably should be a scriptable object...
    // Let's see how today goes, I might replace it ... but I'm also running out of time (21/06/24)
    private DataManager dataManager;
    // On Start not Awake as want to run this after everything has loaded
    private void Start()
    {
        // Instance of Data Manager
        dataManager = DataManager.Instance;

        // Calling Update Character Name with the Player's Character
        UpdateCharacterName("PlayerCharacter");

        if (dataManager.debugOnInfo == true)
        {
            Debug.Log("Dialogue Script Wrapper Start Complete");
        }
    }

    //Function that I can call whenever the data on a character's name changes...
    // I've written this to work on Start as well as be called elsewhere whenever a name updates
    void UpdateCharacterName(string characterName)
    {
        // Variable for constructed path to the character's file
        string characterPath = "Dialogue/" + characterName;
        // In order to be editable at runtime, a file needs to be Resources -- https://docs.unity3d.com/ScriptReference/Resources.Load.html
        // 
        DialogueCharacter character = Resources.Load<DialogueCharacter>(characterPath);

        // From reading through the Unity forums, Scriptable Objects don't always report being null. So checking myself...
        // with Error logging in the else
        if (character != null)
        {
            // Checking to see whether the name has changed
            if (dataManager.playerName != prevCharacterName)
            {
                //If it has changed, then update character name
                character.characterName = dataManager.playerName;
                prevCharacterName = character.characterName;
                // If we've got debug logging on, write message to console
                if (dataManager.debugOnInfo == true)
                {
                    Debug.Log("DialogueCharacter PlayerCharacter playerName set to:" + character.characterName);
                }
            }
        }
        else
            Debug.LogError("DialogueCharacter PlayerCharacter does not exist");

    }

}

