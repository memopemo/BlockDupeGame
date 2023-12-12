using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CloneManager : MonoBehaviour
{
    public PlayerStateManager currentlyControlledPlayer;
    private CameraFocus cameraFocus;
    public int AllowedClones;
    public Queue<PlayerStateManager> AllClones;
    bool restarting;
    // Start is called before the first frame update
    void Start()
    {
        cameraFocus = FindFirstObjectByType<CameraFocus>();
        FindDefaultPlayer();
        if(AllowedClones < 3) AllowedClones = 3;
        
        AllClones = new Queue<PlayerStateManager>(AllowedClones);
        AllClones.Enqueue(currentlyControlledPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        AllowedClones = 3 * (SaveManager.NumOfClonePacks  + 1);
        if(currentlyControlledPlayer == null || (currentlyControlledPlayer.currentState is not DefaultPlayerState &&  currentlyControlledPlayer.currentState is not ThrownPlayerState))
        {
            if(!FindDefaultPlayer())
            {
                if(!restarting)
                {
                    Invoke(nameof(RestartAtSavePoint), 2f);
                    restarting = true;
                }
            }
            else
            {
                CancelInvoke();
                restarting = false;
            }
            
        }

        if(cameraFocus.target != currentlyControlledPlayer.transform)
        {
            cameraFocus.SetTarget(currentlyControlledPlayer.transform);
        }
        if(!CanCreateClone())
        {
            //could play a poof animation here
            Destroy(AllClones.Dequeue().gameObject);
        }
    }

    //Find a Player that is alive
    bool FindDefaultPlayer()
    {
        foreach (PlayerStateManager canidate in FindObjectsOfType<PlayerStateManager>())
        {
            if (canidate.currentState is DefaultPlayerState)
            {
                currentlyControlledPlayer = canidate;
                return true;
            }
        }
        return false;
    }
    
    public bool CanCreateClone()
    {
        return AllClones.Count <= AllowedClones;
    }
    public void CreateClone(PlayerStateManager newClone)
    {
        AllClones.Enqueue(newClone);
    }
    public void RestartAtSavePoint()
    {
        StartCoroutine(nameof(FadeOutAndWait));
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
        PersistentExitData.Instance.exitNum = 1;
        Time.timeScale = 1;
        SceneManager.LoadScene(SaveManager.SaveScene);
        
    }
}
