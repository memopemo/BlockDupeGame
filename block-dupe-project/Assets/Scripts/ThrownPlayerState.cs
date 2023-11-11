using System.Collections;
using Unity;
using UnityEngine;
public class ThrownPlayerState : IPlayerState
{
    public void FixedUpdateState(PlayerStateManager manager)
    {
    }

    public void OnEnter(PlayerStateManager manager)
    {
        
        if(!manager.GetComponent<Liftable>().IsStraightThrown())
        {
            manager.StartCoroutine(BecomeAlive(manager, 0.2f));
        }
        else
        {
            manager.StartCoroutine(BecomeAlive(manager, 5f));
        }
    }
    IEnumerator BecomeAlive(PlayerStateManager manager, float time)
    {
        yield return new WaitForSeconds(time);
        manager.ChangeState(manager.defaultPlayerState);
    }

    public void OnExit(PlayerStateManager manager)
    {
    }

    public void UpdateState(PlayerStateManager manager)
    {
    }
}