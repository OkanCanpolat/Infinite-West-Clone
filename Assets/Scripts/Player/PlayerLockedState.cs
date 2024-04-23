using System;
using Zenject;

public class PlayerLockedState : IPlayerState,  IDisposable
{
    private SignalBus signalBus;
    private Player player;
    [Inject(Id = "Idle")] private IPlayerState idleState;
    public PlayerLockedState(SignalBus signalBus, Player player)
    {
        this.signalBus = signalBus;
        this.player = player;
        signalBus.Subscribe<PlayerTurnStartSignal>(ChangeToIdleStete);
    }
    public void Dispose()
    {
        signalBus.Unsubscribe<EnemyTurnEndSignal>(ChangeToIdleStete);
    }
    public void OnEnemyClickDown(Enemy enemy)
    {
    }
    public void OnEnter()
    {
    }
    public void OnExit()
    {
    }
    public void OnPlayerClickDown()
    {
    }
    public void OnPlayerClickUp()
    {
    }
    private void ChangeToIdleStete()
    {
        player.stateMachine.ChangeStete(idleState);
    }
}
