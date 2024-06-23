using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header ("Debug Levels")]    
    public bool debugOnWarn = false;
    public bool debugOnInfo = true;

    // [Header ("Gameplay Lists")]
    
    //Old Player Name options moving to ScriptableObject
    // [SerializeField] private string[] possiblePlayerNames = { "Amanda", "Brian", "Christopher", "Danielle", "Eleanor", "Frank", "Gabrielle", "Heather", "Ian", "Jessica", "Kevin", "Lauren", "Matthew", "Nicholas", "Olivia", "Patrick", "Quentin", "Rachel", "Sarah", "Timothy", "Ursula", "Victoria", "William", "Xavier", "Yvonne", "Zachary" };

    //Old Player Name before moving to ScriptableObject
    // public string playerName = "????";

    public static DataManager Instance {get; private set;}
    
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
            Debug.LogWarning("Data Manager Destroyed");
        }

        if (debugOnInfo == true)
        {
            Debug.Log("Data Manager Awake Complete");
        }
    }
    // TODO: Write public setter function for private data
}