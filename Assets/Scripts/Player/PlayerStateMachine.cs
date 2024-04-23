using Zenject;
public class PlayerStateMachine
{
    [Inject(Id = "Idle")] public IPlayerState CurrentState;

    public void ChangeStete(IPlayerState currentState)
    {
        CurrentState.OnExit();
        CurrentState = currentState;
        CurrentState.OnEnter();
    }
}
