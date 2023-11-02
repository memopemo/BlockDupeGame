using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadScene("Test");
    }
    
    public void QuitGame(){
        Application.Quit();
    }

    public void GoToSettingsMenu(){
        SceneManager.LoadScene("SettingsMenu");

    }

    public void GoToMainMenu(){
        SceneManager.LoadScene("MainMenu");
        
    }

    public GameObject options;
        void start(){
            options.SetActive(false);
        }
}
