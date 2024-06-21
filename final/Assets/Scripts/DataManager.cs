using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header ("Debug Levels")]    
    public bool debugOnWarn = false;
    public bool debugOnInfo = true;

    [Header ("Gameplay Lists")]

    [SerializeField] private string[] possiblePlayerNames = { "Amanda", "Brian", "Christopher", "Danielle", "Eleanor", "Frank", "Gabrielle", "Heather", "Ian", "Jessica", "Kevin", "Lauren", "Matthew", "Nicholas", "Olivia", "Patrick", "Quentin", "Rachel", "Sarah", "Timothy", "Ursula", "Victoria", "William", "Xavier", "Yvonne", "Zachary" };

    public string playerName = "????";

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
    private void Update()
    {
        
    }
}