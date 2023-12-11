using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Premenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        string savePath = Application.persistentDataPath + Path.DirectorySeparatorChar;
        if (
            File.Exists(savePath + "1" + ".sav") ||
            File.Exists(savePath + "2" + ".sav") ||
            File.Exists(savePath + "3" + ".sav")
        )
        {
            SceneManager.LoadScene("ContinueMenuTest");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
