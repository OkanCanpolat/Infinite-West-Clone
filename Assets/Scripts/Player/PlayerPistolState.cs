using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerPistolState : IPlayerState
{
    private Directions directions;
    private Player player;
    private Animator animator;
    private TileManager tileManager;
    private TurnController turnController;
    private PistolSkill pistolSkill;
    [Inject(Id = "Locked")] private IPlayerState lockedState;
    [Inject(Id = "FreeIdle")] private IPlayerState freeIdleState;
    [Inject(Id = "ShotEffect")] private GameObject shotEffect;
    [Inject] private SignalBus signalBus;
    public Directions Directions => directions;
    public PistolSkill PistolSkill => pistolSkill;
    public PlayerPistolState(Directions directions, Player player, TileManager tileManager, PistolSkill pistolSkill, TurnController turnController)
    {
        this.directions = directions;
        this.player = player;
        this.tileManager = tileManager;
        this.pistolSkill = pistolSkill;
        this.turnController = turnController;
        animator = player.GetComponent<Animator>();
    }
    public async void OnEnemyClickDown(Enemy enemy)
    {
        Vector2Int enemyTile = enemy.currentTile;

        if (IsEnemyInRange(enemyTile))
        {
            await player.RotateToTarget(enemy.transform.position);

            Vector3 currentPosition = player.transform.position;
            Vector3 direction = player.transform.position - enemy.transform.position;
            direction *= 0.1f;

            ShotEffect(true);
            await player.MoveTarget(currentPosition + direction, 8f);
            Shot(enemyTile);
            pistolSkill.OnShot();
            ShotEffect(false);
            await player.MoveTarget(currentPosition, 4f);
            CheckLevelState();
        }
    }
    public void ShotEffect(bool state)
    {
        shotEffect.SetActive(state);
    }
    public void OnEnter()
    {
        signalBus.TryFire<EnterPistolStateSignal>();
        tileManager.EnableIndicators(player.currentTile, directions.Vectors, Color.red);
        animator.SetBool("Fire", true);
    }

    public void OnExit()
    {
        signalBus.TryFire<ExitPistolStateSignal>();
        tileManager.DisableIndicators();
        animator.SetBool("Fire", false);
    }

    public void OnPlayerClickDown()
    {
    }

    public void OnPlayerClickUp()
    {
    }
    private bool IsEnemyInRange(Vector2Int enemyTile)
    {
        Vector2Int playerTile = player.currentTile;

        foreach (Vector2Int direction in directions.Vectors)
        {
            Vector2Int destination = new Vector2Int(playerTile.x + direction.y, playerTile.y + direction.x);
            if (destination == enemyTile) return true;
        }
        return false;
    }
    private List<Enemy> GetLineerEnemies(Vector2Int target)
    {
        List<Enemy> enemies = new List<Enemy>();

        Vector2Int direction = target - player.currentTile;
        int clampedX = Mathf.Clamp(direction.x, -1, 1);
        int clampedY = Mathf.Clamp(direction.y, -1, 1);

        Vector2Int clampedDirection = new Vector2Int(clampedX, clampedY);

        Vector2Int nextTile = player.currentTile;

        do
        {
            nextTile += clampedDirection;
            Tile tile = tileManager.GetTile(nextTile);
            if (tile.Enemy != null) enemies.Add(tile.Enemy);
        } 
        while (nextTile != target);

        return enemies;
    }
    private void Shot(Vector2Int target)
    {
        List<Enemy> enemies = GetLineerEnemies(target);

        foreach(Enemy enemy in enemies)
        {
            IDamageable enemyHealth = enemy.GetComponent<IDamageable>();
            enemyHealth.TakeDamage(1);
        }
    }
    public void CheckLevelState()
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
public class EnterPistolStateSignal { }
public class ExitPistolStateSignal { }

