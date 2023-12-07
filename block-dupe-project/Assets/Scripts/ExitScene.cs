using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScene : MonoBehaviour
{
    public string sceneToLoad;
    public int exitNum;
    
    //Todo: make it only allow the currently controlled player and share code with the "Trigger" Object.
    private void OnTriggerEnter2D(Collider2D collider){
        UIFading fader = FindFirstObjectByType<UIFading>();
        if(fader)
        {
            StartCoroutine(nameof(FadeOutAndWait)); 
        }
        else
        {
            print("No Fadeout Found, not fading out.");
            SwitchSceneWhenFadeOutDone();
        }
        
    }

    private IEnumerator FadeOutAndWait()
    {
        UIFading fader = FindFirstObjectByType<UIFading>();
        Time.timeScale = 0;
        fader.StartCoroutine(nameof(fader.FadeOut));
        while(!fader.done)
        {
            yield return null;
        }
        SwitchSceneWhenFadeOutDone();
    }
    private void SwitchSceneWhenFadeOutDone()
    {
        PersistentExitData _ = new GameObject("ExitData").AddComponent<PersistentExitData>();
        //exitdata.Awake();
        PersistentExitData.Instance.exitNum = exitNum;
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneToLoad);
        
    }
}