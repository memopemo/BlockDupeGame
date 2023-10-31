using UnityEngine;
public class LookUpPlayerSubstate : IPlayerSubstate
{
    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
        Vector2 input = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        DefaultPlayerState.MovementState movementState = substateManager.movementState;

        if (!SubstateConditions.IsLookingUp(input.y, movementState))
        {
            substateManager.ChangeSubstate(substateManager.normalPlayerSubstate);
        }
        substateManager.SetAnimation(PlayerStateManager.Animations.LookUpIdle, PlayerStateManager.Animations.CarryLookUpIdle, manager);
    }
}
