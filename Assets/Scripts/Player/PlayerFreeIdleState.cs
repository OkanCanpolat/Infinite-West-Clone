
public class PlayerFreeIdleState : IPlayerState
{
    private TileManager tileManager;
    public PlayerFreeIdleState(TileManager tileManager)
    {
        this.tileManager = tileManager;
    }
    public void OnEnemyClickDown(Enemy enemy)
    {
    }
    public void OnEnter()
    {
        tileManager.EnableColliders();
    }
    public void OnExit()
    {
        tileManager.DisableColliders();
    }
    public void OnPlayerClickDown()
    {
    }
    public void OnPlayerClickUp()
    {
    }
}
