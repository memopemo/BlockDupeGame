public class WallPlayerSubstate : IPlayerSubstate
{
    public void EnterSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
        manager.playerSounds.PlayGeneric();
    }

    public void UpdateSubstate(PlayerStateManager manager, DefaultPlayerState substateManager)
    {
        DefaultPlayerState.MovementState movementState = substateManager.movementState;

        if (!SubstateConditions.IsAgainstWall(manager) || movementState == DefaultPlayerState.MovementState.Idle )
        {
            substateManager.ChangeSubstate(substateManager.normalPlayerSubstate, manager);
        }

        substateManager.SetAnimation(PlayerStateManager.Animations.Wall, PlayerStateManager.Animations.CarryWall, manager);
    }
}
