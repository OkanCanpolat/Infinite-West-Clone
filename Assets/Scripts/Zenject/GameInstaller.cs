using System;
using UnityEngine;
using Zenject;
public class GameInstaller : MonoInstaller
{
    public Directions PistolDirections;
    public Directions DashDirections;
    public GameObject shotEffect;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<PlayerTurnEndSignal>();
        Container.DeclareSignal<EnemyTurnEndSignal>();
        Container.DeclareSignal<PlayerTurnStartSignal>();
        Container.DeclareSignal<EnemyDiedSignal>();
        Container.DeclareSignal<LevelFinishedSignal>();
        Container.DeclareSignal<EnterPistolStateSignal>();
        Container.DeclareSignal<ExitPistolStateSignal>();
        Container.DeclareSignal<NextSceneSignal>();

        Container.Bind<PlayerAttack>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Enemy>().FromComponentsInHierarchy(e => e.gameObject.activeSelf, false).AsSingle();
        Container.Bind<LightController>().FromComponentInHierarchy().AsSingle();

        Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerStateMachine>().AsSingle();
        Container.Bind<TurnController>().AsSingle();

        Container.Bind<IPlayerState>().WithId("Idle").To<PlayerIdleState>().AsSingle();
        Container.Bind<IPlayerState>().WithId("Attack").To<PlayerAttackState>().AsSingle();
        Container.Bind<IPlayerState>().WithId("Inspector").To<PlayerInspectorState>().AsSingle();
        Container.Bind<IPlayerState>().WithId("Movement").To<PlayerMovementState>().AsSingle();
        Container.Bind<IPlayerState>().WithId("FreeIdle").To<PlayerFreeIdleState>().AsSingle();
        Container.Bind<IPlayerState>().WithId("Heal").To<PlayerHealthState>().AsSingle();
        Container.Bind<IPlayerState>().WithId("Pistol").To<PlayerPistolState>().AsSingle().WithArguments(PistolDirections);
        Container.Bind<IPlayerState>().WithId("Dash").To<PlayerDashState>().AsSingle().WithArguments(DashDirections);
        Container.Bind(typeof(IPlayerState), typeof(IDisposable)).WithId("Locked").To<PlayerLockedState>().AsSingle();

        Container.Bind<GameObject>().WithId("ShotEffect").FromInstance(shotEffect).AsSingle();
        Container.Bind<SkillController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PistolSkill>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DashSkill>().FromComponentInHierarchy().AsSingle();
        Container.Bind<HealthSkill>().FromComponentInHierarchy().AsSingle();

        Container.Bind<TileManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
    }
}