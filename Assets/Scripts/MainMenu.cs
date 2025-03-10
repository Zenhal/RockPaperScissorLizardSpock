using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int gameSceneIndex = 1;
    
    //Called from the Button Object in Main scene.
    public  void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

}
