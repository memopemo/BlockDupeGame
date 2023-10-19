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
        manager.rigidBody.bodyType = RigidbodyType2D.Static;
    }

    public void OnExit(PlayerStateManager manager)
    {
        manager.normalBox.SetCollisionBox(manager.boxCollider);
        manager.rigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public void UpdateState(PlayerStateManager manager)
    {
        manager.animator2D.SetAnimation(HELD);   
    }
}
