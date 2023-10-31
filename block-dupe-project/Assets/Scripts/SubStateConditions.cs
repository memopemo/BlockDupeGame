using UnityEngine;

//Library of conditions that many states check with
public static class SubstateConditions
{
    public static bool IsDucking(float yinput, PlayerStateManager manager)
    {
        return manager.IsGrounded() && yinput < 0;
    }
    public static bool IsLookingUp(float yinput, DefaultPlayerState.MovementState movementState)
    {
        return yinput > 0 && movementState == DefaultPlayerState.MovementState.Idle;
    }

    public static bool IsRunningAgainstWall(float xinput, PlayerStateManager manager)
    {
        return xinput != 0 && IsAgainstWall(manager);
    }

    //returns if the player can un-duck from ducking (so they dont clip into 1-block gaps)
    public static bool CanUnDuck(float yinput, PlayerStateManager manager)
    {
        return yinput == 0 && !manager.IsTouchingCeiling();
    }

    //returns if we are touching a wall, and we are facing that wall.
    public static bool IsAgainstWall(PlayerStateManager manager)
    {
        return (manager.IsTouchingRightWall() && manager.direction) || (manager.IsTouchingLeftWall() && !manager.direction);
    }

}