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
        manager.normalBox.SetCollisionBox(manager.boxCollider);
    }

    public void OnExit(PlayerStateManager manager)
    {
        
    }

    public void UpdateState(PlayerStateManager manager)
    {
        manager.animator2D.SetAnimation(HELD);   
    }
}
