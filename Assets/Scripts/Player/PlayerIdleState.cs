using UnityEngine;
using Zenject;

public class PlayerIdleState : IPlayerState
{
    private Vector2 touch;
    private Vector2 release;
    private TileManager tileManager;
    private Player player;
    private float swipeOffset = 0.5f;
    private Camera camera;
    [Inject (Id = "Inspector")] private IPlayerState inspectorState;
    [Inject(Id = "Movement")] private IPlayerState movementState;

    public PlayerIdleState(Player player, TileManager tileManager, Camera camera)
    {
        this.player = player;
        this.tileManager = tileManager;
        this.camera = camera;
    }
    public void OnEnemyClickDown(Enemy enemy)
    {
        IAttack attack = enemy.GetComponent<IAttack>();
        Vector2Int[] directions = attack.AttackableDirections;
        tileManager.EnableIndicators(enemy.currentTile, directions, Color.red);
        player.stateMachine.ChangeStete(inspectorState);
    }

    public void OnPlayerClickDown()
    {
        touch = camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -camera.transform.position.z));
    }

    public void OnPlayerClickUp()
    {
        release = camera.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -camera.transform.position.z));

        if (Vector2.Distance(release, touch) < swipeOffset) return;

        Vector2Int direction = GetDirection(Mathf.Atan2(release.y - touch.y, release.x - touch.x) * Mathf.Rad2Deg);
        Vector2Int dest = new Vector2Int(player.currentTile.x + direction.y, player.currentTile.y + direction.x);
        Vector3 pos = tileManager.GetTilePosition(dest.x, dest.y);
        Tile tile = tileManager.GetTile(dest);

        if (!tile.IsFull)
        {
            player.destination = pos;
            player.currentTile = dest;
            player.stateMachine.ChangeStete(movementState);
        }
    }
    public Vector2Int GetDirection(float angle)
    {
        if (angle > -45 && angle <= 45)
        {
            return Vector2Int.right;
        }
        else if (angle > 45 && angle <= 135)
        {
            return Vector2Int.up;
        }
        else if (angle > 135 || angle <= -135)
        {
            return Vector2Int.left;
        }
        else
        {
            return Vector2Int.down;
        }
    }
    public void OnEnter()
    {
    }
    public void OnExit()
    {
    }
}


