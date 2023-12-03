using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StayTrigger : MonoBehaviour
{
    int framesTouching = 0;
    bool hasActivated = false;
    bool isTouching = false;
    public int framesUntilActivate;
    public UnityEvent OnActivate;
    public UnityEvent OnStart;
    public UnityEvent OnLeave;

    // Update is called once per frame
    void Update()
    {
        if(isTouching)
        {
            if (framesTouching == 0)
            {
                OnStart.Invoke();
            }
            framesTouching++;
        }
        else if(framesTouching != 0)
        {
            OnLeave.Invoke();
            hasActivated = false;
            framesTouching = 0;
        }
        if(framesTouching >= framesUntilActivate && !hasActivated)
        {
            OnActivate.Invoke();
            hasActivated = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsPlayer(collider))
        {
            isTouching = true;
        }
        else print("false");
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (IsPlayer(collider))
        {
            isTouching = false;
        }
        else print("false");

    }
    bool IsPlayer(Collider2D collider)
    {
        //Conditions:
        // Must be a player
        // Must be an alive player
        // Must be idle.
        return collider.TryGetComponent(out PlayerStateManager p) && 
            p.currentState == p.defaultPlayerState;
    }
    public void debugPrintActivated()
    {
        print($"StayTrigger {gameObject.name} Activated!");
    }
    public void debugPrintEnter()
    {
        print($"StayTrigger {gameObject.name} Entered!");
    }
    public void debugPrintLeave()
    {
        print($"StayTrigger {gameObject.name} Left!");
    }
}
