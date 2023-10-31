using UnityEngine;
using System;

public class DuckPlayerSubstate : IPlayerSubstate
{
    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
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
            
            substateManager.ChangeSubstate(substateManager.normalPlayerSubstate);
        }

        //play depending on if we are carrying or running (4 possibilities)
        substateManager.SetAnimation(PlayerStateManager.Animations.DuckIdle, PlayerStateManager.Animations.CarryDuckIdle, manager);
    }
}