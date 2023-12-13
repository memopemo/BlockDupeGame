using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour
{
    public int SaveNumber;
    // Start is called before the first frame update
    void Start()
    {
        PersistentExitData _ = new GameObject("ExitData").AddComponent<PersistentExitData>();
        PersistentExitData.Instance.exitNum = 1;
        SaveManager.LoadSaveFromFile(SaveNumber);
        PowerupStatus.LoadPowerups();
        SceneManager.LoadScene(SaveManager.SaveScene);
    }
}
