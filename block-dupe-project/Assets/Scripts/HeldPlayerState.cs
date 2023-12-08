using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldPlayerState : IPlayerState
{
    public const int HELD = 6;
    public void FixedUpdateState(PlayerStateManager manager)
    {
        
    }

    public void OnEnter(PlayerStateManager manager)
    {
        manager.unaliveBox.SetCollisionBox(manager.boxCollider);
        manager.animator2D.SetAnimation(HELD);   
        //Debug.Log(manager.animator2D);
    }

    public void OnExit(PlayerStateManager manager)
    {
        manager.normalBox.SetCollisionBox(manager.boxCollider);
    }

    public void UpdateState(PlayerStateManager manager)
    {
        manager.animator2D.SetAnimation(HELD);   
    }
}
