using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerAttack : MonoBehaviour
{
    public Vector2Int[] AttackableDirections;
    private Player player;
    private TileManager tileManager;

    [Inject]
    public void Construct(Player player, TileManager tileManager)
    {
        this.player = player;
        this.tileManager = tileManager;
    }

    public bool IsEnemyInRange()
    {
        Vector2Int playerTile = player.currentTile;

        foreach (Vector2Int direction in AttackableDirections)
        {
            Tile tile = null;

            if (tileManager.TryGetTile(new Vector2Int(playerTile.x + direction.y, playerTile.y + direction.x), ref tile))
            {
                if (tile.Enemy != null) return true;
            }
        }
        return false;
    }
    public List<Enemy> GetAllEnemies()
    {
        List<Enemy> enemies = new List<Enemy>();

        Vector2Int playerTile = player.currentTile;

        foreach (Vector2Int direction in AttackableDirections)
        {
            Tile tile = tileManager.GetTile(new Vector2Int(playerTile.x + direction.y, playerTile.y + direction.x));
            if (tile!= null && tile.Enemy != null) enemies.Add(tile.Enemy);
        }
        return enemies;
    }
}
