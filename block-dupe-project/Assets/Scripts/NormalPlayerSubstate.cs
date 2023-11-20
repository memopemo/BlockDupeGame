using UnityEngine;
public class NormalPlayerSubstate : IPlayerSubstate
{
    public void EnterSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
    }

    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
        Vector2 joyInput = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //ducking check
        if (SubstateConditions.IsDucking(joyInput.y, manager))
        {
            if(manager.carryingObj)
            {
                manager.carryDuckBox?.SetCollisionBox(manager.boxCollider);
            }
            else
            {
                manager.duckBox?.SetCollisionBox(manager.boxCollider);
            }
            substateManager.ChangeSubstate(substateManager.duckPlayerSubstate);
        }

        //wall check
        if (SubstateConditions.IsRunningAgainstWall(joyInput.x, manager))
        {    
            substateManager.ChangeSubstate(substateManager.wallPlayerSubstate);
        }

        //looking up check
        if (SubstateConditions.IsLookingUp(joyInput.y, substateManager.movementState))
        {
            substateManager.ChangeSubstate(substateManager.lookUpPlayerSubstate);
        }

        substateManager.SetAnimation(PlayerStateManager.Animations.Idle, PlayerStateManager.Animations.CarryIdle, manager);

    }
}
