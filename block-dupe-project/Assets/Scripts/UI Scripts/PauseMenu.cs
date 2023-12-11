using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            } 
            else{
                PauseGame();
            } 
        }
    }
    public void PauseGame(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; 
        isPaused = true;
    }
    public void ResumeGame(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; 
        isPaused = false; 
    }
    public void GoToMainMenu(){
        
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void GoToSettingsMenu(){
        SceneManager.LoadScene("SettingsMenu");
    }
    public void QuitGame(){
        Application.Quit();
    }
     private List<string> visitedScenes = new List<string>();
    public void GoBack()// Go back to previous sence used for pause menu
    {
         SceneManager.LoadScene("InGame");
        
    }
}