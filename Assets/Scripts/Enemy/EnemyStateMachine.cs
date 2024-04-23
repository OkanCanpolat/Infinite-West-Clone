public class EnemyStateMachine 
{
    public IEnemyState currentState;

    public void ChangeState(IEnemyState state)
    {
        currentState?.OnExit();
        currentState = state;
        currentState.OnEnter();
    }
}
