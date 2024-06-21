using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCustom : MonoBehaviour
{
    // private string sceneName;

    // Start is called before the first frame update
    public void LoadScene(string sceneName)
    {
            SceneManager.LoadScene(sceneName);
    }
        public void LoadSceneAdditive(string sceneName)
    {
            SceneManager.LoadScene(sceneName,  LoadSceneMode.Additive);
    }
}

