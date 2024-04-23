using UnityEngine;
using Zenject;

public class EnemyRangedAttack : MonoBehaviour, IAttack
{
    [SerializeField] private Vector2Int[] attackableDirections;
    [SerializeField] private GameObject shotEffect;
    private Player player;
    private Enemy enemy;
    private SignalBus signalBus;
    
    public Vector2Int[] AttackableDirections => attackableDirections;

    [Inject]
    public void Construct(Player player, Enemy enemy, SignalBus signalBus)
    {
        this.player = player;
        this.enemy = enemy;
        this.signalBus = signalBus;
    }
    public async void Attack()
    {
        await enemy.RotateToTarget(player.transform.position);

        Vector3 currentPosition = enemy.transform.position;
        Vector3 direction = enemy.transform.position - player.transform.position;
        direction *= 0.25f;

        shotEffect.SetActive(true);
        await enemy.MoveTarget(currentPosition + direction);
        IDamageable health = player.GetComponent<IDamageable>();
        health.TakeDamage(1);
        await enemy.MoveTarget(currentPosition);
        shotEffect.SetActive(false);
        signalBus.Fire<EnemyTurnEndSignal>();
    }

    public bool IsEnemyInRange()
    {
        Vector2Int playerTile = player.currentTile;

        foreach (Vector2Int direction in attackableDirections)
        {
            Vector2Int attackTile = new Vector2Int(enemy.currentTile.x + direction.y, enemy.currentTile.y + direction.x);

            if (attackTile == playerTile)
            {
                return true;
            }
        }
        return false;
    }
}
