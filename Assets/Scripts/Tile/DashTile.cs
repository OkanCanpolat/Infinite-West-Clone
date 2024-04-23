using UnityEngine;
using Zenject;
public class DashTile : MonoBehaviour
{
    [SerializeField] private Tile tile;
    private Player player;
    private TileManager tileManager;
    private DashSkill dashSkill;
    [Inject(Id = "Movement")] private IPlayerState movemenetState;

    [Inject]
    public void Construct(Player player, TileManager tileManager, DashSkill dashSkill)
    {
        this.player = player;
        this.dashSkill = dashSkill;
        this.tileManager = tileManager;
    }

    private void OnMouseDown()
    {
        if (tile.IsFull) return;
        Vector2Int tileDest = tileManager.GetTile(tile);
        player.currentTile = tileDest;
        player.destination = tile.transform.position;
        dashSkill.OnDash();
        player.stateMachine.ChangeStete(movemenetState);
    }
}
