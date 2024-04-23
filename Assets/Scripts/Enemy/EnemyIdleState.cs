using System;
using Zenject;

public class EnemyIdleState : IEnemyState, IDisposable
{
    private SignalBus signalBus;
    private IAttack attack;
    private IDamageable health;
    private Enemy enemy;
    [Inject(Id = "EnemyMovement")] private IEnemyState movementState;
    [Inject(Id = "EnemyAttack")] private IEnemyState attackState;

    public EnemyIdleState(SignalBus signalBus, Enemy enemy, IAttack attack, IDamageable health)
    {
        this.signalBus = signalBus;
        this.enemy = enemy;
        this.attack = attack;
        this.health = health;
        signalBus.Subscribe<PlayerTurnEndSignal>(ControlState);
    }

    public void Dispose()
    {
        signalBus.Unsubscribe<PlayerTurnEndSignal>(ControlState);
    }

    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }
    private void ControlState()
    {
        if (!health.IsAlive()) return;

        if (attack.IsEnemyInRange())
        {
            enemy.stateMachine.ChangeState(attackState);
        }
        else
        {
            enemy.stateMachine.ChangeState(movementState);
        }
    }

}
