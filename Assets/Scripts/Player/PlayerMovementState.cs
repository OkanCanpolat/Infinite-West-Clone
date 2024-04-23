using Zenject;

public class PlayerMovementState : IPlayerState
{
    private Player player;
    private PlayerAttack playerAttack;
    private TurnController turnController;
    private TileManager tileManager;
    [Inject(Id = "Attack")] private IPlayerState attackState;
    [Inject(Id = "Locked")] private IPlayerState lockedState;
    [Inject(Id = "FreeIdle")] private IPlayerState freeIdleState;

    public PlayerMovementState(Player player, PlayerAttack playerAttack, TurnController turnController, TileManager tileManager)
    {
        this.player = player;
        this.playerAttack = playerAttack;
        this.turnController = turnController;
        this.tileManager = tileManager;
    }
    public void OnEnemyClickDown(Enemy enemy)
    {
    }
    public async void OnEnter()
    {
        await player.Movement();

        Tile tile = tileManager.GetTile(player.currentTile);
        if (tile.Collectable != null) tile.Collectable.OnCollect();


        if (playerAttack.IsEnemyInRange())
        {
            player.stateMachine.ChangeStete(attackState);
        }
        else
        {
            CheckLevelState();
        }
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
