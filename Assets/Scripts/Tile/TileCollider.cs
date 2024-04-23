using UnityEngine;
using Zenject;

public class TileCollider : MonoBehaviour
{
    private Tile tile;
    private Player player;
    private TileManager tileManager;
    [Inject(Id = "Movement")] private IPlayerState movementState;

    [Inject]
    public void Construct(Player player, TileManager tileManager)
    {
        this.player = player;
        this.tileManager = tileManager;
    }

    private void Awake()
    {
        tile = GetComponent<Tile>();
    }
    private void OnMouseDown()
    {
        Vector2Int tilePosition = tileManager.GetTile(tile);
        if (tilePosition == player.currentTile) return;
        player.currentTile = tilePosition;
        player.destination = transform.position;
        player.stateMachine.ChangeStete(movementState);
    }
}
