using UnityEngine;
using Unity;
using System;

public class DuckPlayerSubstate : IPlayerSubstate
{
    int BeginDuckAnimationFrames;
    public void EnterSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
        BeginDuckAnimationFrames = 10;
    }

    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {   if(BeginDuckAnimationFrames != 0 && substateManager.movementState == DefaultPlayerState.MovementState.Idle)
        {
            Debug.Log(BeginDuckAnimationFrames);
            manager.animator2D.SetAnimation((!manager.carryingObj?PlayerStateManager.Animations.DuckIdle:PlayerStateManager.Animations.CarryDuckIdle)+5);
            BeginDuckAnimationFrames--;
            return;
        }
        Vector2 input = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (SubstateConditions.CanUnDuck(input.y, manager))
        {
            if(manager.carryingObj)
            {
                manager.carryBox.SetCollisionBox(manager.boxCollider);
            }
            else
            {
                manager.normalBox.SetCollisionBox(manager.boxCollider);
            }
            
            substateManager.ChangeSubstate(substateManager.normalPlayerSubstate, manager);
        }

        //play depending on if we are carrying or running (4 possibilities)
        substateManager.SetAnimation(PlayerStateManager.Animations.DuckIdle, PlayerStateManager.Animations.CarryDuckIdle, manager);
    }
}