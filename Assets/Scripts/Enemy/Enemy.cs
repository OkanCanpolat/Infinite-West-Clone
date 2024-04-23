using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
public class Enemy : MonoBehaviour
{
    public TileManager tileManager;
    public Vector2Int currentTile;
    public Vector2Int[] MovementPaths;
    public int movementIndex;
    private Player player;
    public EnemyStateMachine stateMachine;
    [Inject(Id = "EnemyIdle")] private IEnemyState firstState;
   
    private void Start()
    {
        Tile tile = tileManager.GetTile(currentTile);
        tile.IsFull = true;
        tile.Enemy = this;
        stateMachine.ChangeState(firstState);
    }

    [Inject]
    public void Construct(Player player, EnemyStateMachine stateMachine)
    {
        this.player = player;
        this.stateMachine = stateMachine;
    }

    private void OnMouseDown()
    {
        player.stateMachine.CurrentState.OnEnemyClickDown(this);
    }

    public async Task Movement(Vector3 target)
    {
        Tile cTile2 = tileManager.GetTile(currentTile);
        cTile2.Enemy = this;
        cTile2.IsFull = true;
        await RotateToTarget(target);
        await MoveTarget(target);
    }
    public async Task MoveTarget(Vector3 target, float speed = 10f)
    {
        float t = 0;
        Vector3 startPos = transform.position;

        while (t < 1)
        {
            transform.position = Vector3.Lerp(startPos, target, t);
            t += Time.deltaTime * speed;
            await Task.Yield();
        }

        transform.position = target;
    }
    public async Task RotateToTarget(Vector3 target, float speed = 2f)
    {
        if (Vector3.Dot(transform.forward, (target - transform.position).normalized) == 1)
        {
            return;
        }

        float t = 0;
        Quaternion lookRotation = Quaternion.LookRotation(target - transform.position);

        while (t < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, t);
            t += Time.deltaTime * speed;
            await Task.Yield();
        }

        transform.rotation = lookRotation;
    }
    public void ReversePath()
    {
        for (int i = 0; i < MovementPaths.Length; i++)
        {
            MovementPaths[i] *= -1;
        }
        Array.Reverse(MovementPaths);
    }
}
