using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScene : MonoBehaviour
{
    public SceneAsset sceneToLoad;
    public int exitNum;
    
    //Todo: make it only allow the currently controlled player and share code with the "Trigger" Object.
    private void OnTriggerEnter2D(Collider2D collider){
        PersistentExitData exitdata = new GameObject("ExitData").AddComponent<PersistentExitData>();
        //exitdata.Awake();
        PersistentExitData.Instance.exitNum = exitNum;
        SceneManager.LoadScene(sceneToLoad.name);
}
}