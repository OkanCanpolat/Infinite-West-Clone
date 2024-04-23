using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public enum Direction
{
    N, E, S, W, NE, NW, SE, SW
}
public class Player : MonoBehaviour
{
    public Vector3 destination;
    public Vector2Int currentTile;
    public TileManager TileManager;
    public PlayerStateMachine stateMachine;

    [Inject]
    public void Construct(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    private void Start()
    {
        Vector3 currentPosition = TileManager.GetTilePosition(currentTile.x, currentTile.y);
        transform.position = currentPosition;
    }
    private void OnMouseDown()
    {
        stateMachine.CurrentState.OnPlayerClickDown();
    }
    private void OnMouseUp()
    {
        stateMachine.CurrentState.OnPlayerClickUp();
    }
    public async Task Movement()
    {
        await RotateToTarget(destination);
        await JumptTarget(destination);
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
    public async Task JumptTarget(Vector3 target, float speed = 2f)
    {
        
        float distance = Vector3.Distance(transform.position, target);
        float relDistance = distance - Mathf.Sqrt(distance);


        Vector3 center = (transform.position + target) * 0.5f;
        center -= new Vector3(0, relDistance, 0);

        Vector3 riseRelCenter = transform.position - center;
        Vector3 setRelCenter = target - center;

        float t = 0;

        while (t < 1)
        {
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, t);
            transform.position += center;
            t += Time.deltaTime * speed;
            await Task.Yield();
        }
        transform.position = target;
    }
    public async Task RotateToTarget(Vector3 target, float speed = 2f)
    {

        if (Vector3.Dot(transform.forward, (target - transform.position).normalized) == 1)
        {
            speed *= 2;
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
}
