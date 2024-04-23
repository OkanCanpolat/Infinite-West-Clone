using UnityEngine;

public class PlayerDashState : IPlayerState
{
    private Directions directions;
    private Player player;
    private TileManager tileManager;
    private Animator animator;

    public PlayerDashState(Directions directions, Player player, TileManager tileManager)
    {
        this.directions = directions;
        this.player = player;
        this.tileManager = tileManager;
        animator = player.GetComponent<Animator>();
    }
    public void OnEnemyClickDown(Enemy enemy)
    {
    }
    public void OnEnter()
    {
        animator.SetBool("Dash", true);
        tileManager.EnableDash(player.currentTile, directions.Vectors);
        tileManager.EnableIndicators(player.currentTile, directions.Vectors, Color.white);
    }
    public void OnExit()
    {
        animator.SetBool("Dash", false);
        tileManager.DisableDash(player.currentTile, directions.Vectors);
        tileManager.DisableIndicators();
    }
    public void OnPlayerClickDown()
    {
    }
    public void OnPlayerClickUp()
    {
    }
}
