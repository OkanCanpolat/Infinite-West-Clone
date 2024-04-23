using UnityEngine;
using Zenject;

public class PlayerInspectorState : IPlayerState
{
    private TileManager tileManager;
    private PlayerStateMachine stateMachine;
    [Inject(Id = "Idle")] private IPlayerState idleState;
    public PlayerInspectorState(TileManager tileManager, PlayerStateMachine stateMachine)
    {
        this.tileManager = tileManager;
        this.stateMachine = stateMachine;
    }
    public void OnEnemyClickDown(Enemy enemy)
    {
        IAttack attack = enemy.GetComponent<IAttack>();
        Vector2Int[] directions = attack.AttackableDirections;
        tileManager.DisableIndicators();
        tileManager.EnableIndicators(enemy.currentTile, directions, Color.red);
    }
    public void OnEnter()
    {
    }
    public void OnExit()
    {
        tileManager.DisableIndicators();
    }
    public void OnPlayerClickDown()
    {
        tileManager.DisableIndicators();
    }
    public void OnPlayerClickUp()
    {
        stateMachine.ChangeStete(idleState);
    }
}
