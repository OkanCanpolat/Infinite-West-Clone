using System;
using UnityEngine;
using Zenject;

public class EnemyInstaller : MonoInstaller
{
    [SerializeField] private Enemy enemy;
    public override void InstallBindings()
    {
        Container.Bind<IEnemyState>().WithId("EnemyMovement").To<EnemyMovementState>().AsSingle();
        Container.Bind<IEnemyState>().WithId("EnemyAttack").To<EnemyAttackState>().AsSingle();
        Container.Bind(typeof(IEnemyState), typeof(IDisposable)).WithId("EnemyIdle").To<EnemyIdleState>().AsSingle();
        Container.Bind<EnemyStateMachine>().AsSingle();
        Container.Bind<IAttack>().FromMethod(GetEnemyAttack).AsSingle();
        Container.Bind<IDamageable>().FromMethod(GetHealth).AsSingle();
        Container.Bind<Enemy>().FromInstance(enemy).AsSingle();
    }

    private IAttack GetEnemyAttack()
    {
        return enemy.GetComponent<IAttack>();
    }
    private IDamageable GetHealth()
    {
        return enemy.GetComponent<IDamageable>();
    }
}