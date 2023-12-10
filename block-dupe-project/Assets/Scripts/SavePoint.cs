using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePoint : MonoBehaviour
{
    public void Save()
    {
        SaveManager.Save(SceneManager.GetActiveScene().name);
    }
}
