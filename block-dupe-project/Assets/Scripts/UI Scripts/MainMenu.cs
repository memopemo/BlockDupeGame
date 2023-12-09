using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        SaveManager.LoadSaveFromFile(1); //change this to load from different saves later
        PersistentExitData _ = new GameObject("ExitData").AddComponent<PersistentExitData>();
        PersistentExitData.Instance.exitNum = 1;
        SceneManager.LoadScene(SaveManager.SaveScene);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
