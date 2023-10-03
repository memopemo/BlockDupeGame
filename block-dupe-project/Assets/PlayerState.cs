public interface IPlayerState
{
    public void OnEnter(PlayerStateManager manager);
    public void UpdateState(PlayerStateManager manager);
    public void FixedUpdateState(PlayerStateManager manager);
    public void OnExit(PlayerStateManager manager);
}
