using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerAttackState : IPlayerState
{
    private PlayerAttack playerAttack;
    private Player player;
    private TurnController turnController;
    [Inject(Id = "Locked")] private IPlayerState lockedState;
    [Inject(Id = "FreeIdle")] private IPlayerState freeIdleState;

    public PlayerAttackState(PlayerAttack playerAttack, Player player, TurnController turnController)
    {
        this.playerAttack = playerAttack;
        this.player = player;
        this.turnController = turnController;
    }

    public void OnEnemyClickDown(Enemy enemy)
    {
    }
    public async void OnEnter()
    {
        List<Enemy> enemies = playerAttack.GetAllEnemies();

        foreach(Enemy enemy in enemies)
        {
            await player.RotateToTarget(enemy.transform.position);

            Vector3 currentPosition = player.transform.position;
            Vector3 direction = enemy.transform.position - player.transform.position;
            direction *= 0.25f;

            await player.MoveTarget(currentPosition + direction);
            IDamageable enemyHealth = enemy.GetComponent<IDamageable>();
            enemyHealth.TakeDamage(1);
            await player.MoveTarget(currentPosition);
        }
        CheckLevelState();
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
    private void CheckLevelState()
    {
        if (turnController.IsLevelFinished)
        {
            player.stateMachine.ChangeStete(freeIdleState);
        }
        else
        {
            player.stateMachine.ChangeStete(lockedState);
            turnController.EndPlayerTurn();
        }
    }
}
