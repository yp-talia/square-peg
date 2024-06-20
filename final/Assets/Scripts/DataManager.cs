using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [Header ("Debug Levels")]    
    public bool debugOnWarn = false;
    public bool debugOnInfo = false;

    [Header ("Gameplay Lists")]

    public string[] possiblePlayerNames = { "Amanda", "Brian", "Christopher", "Danielle", "Eleanor", "Frank", "Gabrielle", "Heather", "Ian", "Jessica", "Kevin", "Lauren", "Matthew", "Nicholas", "Olivia", "Patrick", "Quentin", "Rachel", "Sarah", "Timothy", "Ursula", "Victoria", "William", "Xavier", "Yvonne", "Zachary" };

    public string playerName = "????";
    
    //Singleton pattern below
    public static DataManager Instance {get; private set;}
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
    }
}