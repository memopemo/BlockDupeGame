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
        InvokeRepeating(nameof(CheckExcessClones), 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentlyControlledPlayer == null || currentlyControlledPlayer.currentState is not DefaultPlayerState)
        {
            FindDefaultPlayer();
        }

        if(cameraFocus.target != currentlyControlledPlayer.transform)
        {
            cameraFocus.SetTarget(currentlyControlledPlayer.transform);
        }
    }
    void CheckExcessClones()
    {
        if(!CanCreateClone())
        {
            
            //could play a poof animation here
            Destroy(AllClones.Dequeue().gameObject);
        }
    }

    //Find a Player that is alive
    void FindDefaultPlayer()
    {
        foreach (PlayerStateManager canidate in FindObjectsOfType<PlayerStateManager>())
        {
            if (canidate.currentState is DefaultPlayerState)
            {
                currentlyControlledPlayer = canidate;
            }
        }
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
