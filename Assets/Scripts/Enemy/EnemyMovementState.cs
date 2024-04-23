using ModestTree;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class EnemyMovementState : IEnemyState
{
    private Enemy enemy;
    private TileManager tileManager;
    private SignalBus signalBus;
    public EnemyMovementState(Enemy enemy, TileManager tileManager, SignalBus signalBus)
    {
        this.enemy = enemy;
        this.tileManager = tileManager;
        this.signalBus = signalBus;
    }

    public async void OnEnter()
    {
        Vector2Int currentDirection = enemy.MovementPaths[enemy.movementIndex];
        Vector2Int dest = new Vector2Int(enemy.currentTile.x + currentDirection.y, enemy.currentTile.y + currentDirection.x);
        Tile targetTile = tileManager.GetTile(dest);

        if (targetTile.IsFull)
        {
            bool result = await WaitTile(targetTile);

            if (result)
            {
                Move(dest);
            }

            else
            {
                signalBus.Fire<EnemyTurnEndSignal>();
            }
        }
        else
        {
            Move(dest);
        }
    }

    public void OnExit()
    {
    }
    private async void Move(Vector2Int dest)
    {
        Tile currentTile = tileManager.GetTile(enemy.currentTile);

        currentTile.Enemy = null;
        currentTile.IsFull = false;
        enemy.currentTile = dest;
        Vector3 pos = tileManager.GetTilePosition(dest);
        Vector3 relativeYPos = new Vector3(pos.x, enemy.transform.position.y, pos.z);
        await enemy.Movement(relativeYPos);
        enemy.movementIndex++;

        if (enemy.movementIndex >= enemy.MovementPaths.Length)
        {
            enemy.ReversePath();
            enemy.movementIndex = 0;
        }
        signalBus.Fire<EnemyTurnEndSignal>();
    }
    private async Task<bool> WaitTile(Tile tile)
    {
        float t = 0;

        while (t < 1)
        {
            await Task.Yield();
            t += Time.deltaTime;
            if (!tile.IsFull) return true;
        }
        return false;
    }
   
}
