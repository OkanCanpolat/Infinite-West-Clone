using UnityEngine;
using Zenject;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private Enemy enemy;
    private bool isAlive;
    private TileManager tileManager;
    private SignalBus signalBus;

    [Inject]
    public void Construct(TileManager tileManager, SignalBus signalBus)
    {
        this.tileManager = tileManager;
        this.signalBus = signalBus;
    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        isAlive = true;
    }
    public async void TakeDamage(int damage)
    {
        Die();
        await enemy.MoveTarget(enemy.transform.position + Vector3.up * 7, 4f);
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    private void Die()
    {
        isAlive = false;
        signalBus.Fire(new EnemyDiedSignal() { Enemy = enemy });
        Tile tile = tileManager.GetTile(enemy.currentTile);
        tile.IsFull = false;
        tile.Enemy = null;
    }
}
