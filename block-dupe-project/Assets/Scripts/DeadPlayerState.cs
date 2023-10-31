using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPlayerState : IPlayerState
{


    public void FixedUpdateState(PlayerStateManager manager)
    {
    }

    public void OnEnter(PlayerStateManager manager)
    {
        manager.unaliveBox.SetCollisionBox(manager.boxCollider);
        manager.animator2D.SetAnimation(6);
        manager.gameObject.layer = 6;
    }

    public void OnExit(PlayerStateManager manager)
    {
    }

    public void UpdateState(PlayerStateManager manager)
    {
        manager.unaliveBox.SetCollisionBox(manager.boxCollider);
        manager.animator2D.SetAnimation(6);
    }
}
