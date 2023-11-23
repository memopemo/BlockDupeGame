using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneManager : MonoBehaviour
{
    public PlayerStateManager currentlyControlledPlayer;
    private CameraFocus cameraFocus;
    public int AllowedClones;
    public Queue<PlayerStateManager> AllClones;
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
        if(currentlyControlledPlayer == null || currentlyControlledPlayer.currentState is not DefaultPlayerState or ThrownPlayerState)
        {
            if(!FindDefaultPlayer())
            {
                //restart at save point
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
}
