public class WallPlayerSubstate : IPlayerSubstate
{
    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
        DefaultPlayerState.MovementState movementState = substateManager.movementState;

        if (!SubstateConditions.IsAgainstWall(manager) || movementState == DefaultPlayerState.MovementState.Idle )
        {
            substateManager.ChangeSubstate(substateManager.normalPlayerSubstate);
        }

        substateManager.SetAnimation(PlayerStateManager.Animations.Wall, PlayerStateManager.Animations.CarryWall, manager);
    }
}
