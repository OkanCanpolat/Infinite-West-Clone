using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class TNT : MonoBehaviour
{
    [SerializeField] private Tile tile;
    [SerializeField] private Vector2Int[] explosionArea;
    private PlayerPistolState pistolState;
    private Player player;
    private SignalBus signalBus;
    private Collider col;
    private TileManager tileManager;
    private Animator animator;

    [Inject]
    public void Construct([Inject(Id = "Pistol")] IPlayerState pistolState, Player player, SignalBus signalBus, TileManager tileManager)
    {
        this.tileManager = tileManager;
        this.signalBus = signalBus;
        this.pistolState = pistolState as PlayerPistolState;
        this.player = player;
        tile.IsFull = true;
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
    }
    private void Awake()
    {
        signalBus.Subscribe<EnterPistolStateSignal>(OnEnterPistolState);
        signalBus.Subscribe<ExitPistolStateSignal>(OnExitPistolState);
    }
    private void OnMouseDown()
    {
        if (player.stateMachine.CurrentState != pistolState) return;

        Vector2Int[] directions = pistolState.Directions.Vectors;
        Vector2Int playerPos = player.currentTile;


        foreach (Vector2Int direction in directions)
        {
            Vector2Int result = playerPos + new Vector2Int(direction.y, direction.x);

            if (result == new Vector2Int(tile.X, tile.Y))
            {
                PlayerAnim();
            }
        }
    }
    private void OnEnterPistolState()
    {
        col.enabled = true;
    }
    private void OnExitPistolState()
    {
        col.enabled = false;
    }
    private void OnExplodeDamage()
    {
        Vector2Int currentTile = new Vector2Int(tile.X, tile.Y);

        foreach (Vector2Int tile in explosionArea)
        {
            Vector2Int result = currentTile + new Vector2Int(tile.y, tile.x);
            Tile resultTile = tileManager.GetTile(result);

            if (resultTile.Enemy != null)
            {
                IDamageable enemyHealth = resultTile.Enemy.GetComponent<IDamageable>();
                enemyHealth.TakeDamage(3);
            }
        }
    }
    private async void OnExplodeAnim()
    {
        animator.SetTrigger("Explosion");
        await Task.Delay(500);
        OnExplodeDamage();
        await Task.Delay(200);

        if(this != null)
        {
            tile.IsFull = false;
            Destroy(gameObject);
            pistolState.CheckLevelState();
        }
    }
    private async void PlayerAnim()
    {
        Vector3 relativeYPos = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        await player.RotateToTarget(relativeYPos);

        Vector3 currentPosition = player.transform.position;
        Vector3 direction = player.transform.position - transform.position;
        direction *= 0.1f;

        pistolState.ShotEffect(true);
        await player.MoveTarget(currentPosition + direction, 8f);
        pistolState.PistolSkill.OnShot();
        OnExplodeAnim();
        pistolState.ShotEffect(false);
        await player.MoveTarget(currentPosition, 4f);
    }
    private void OnDestroy()
    {
        signalBus.Unsubscribe<EnterPistolStateSignal>(OnEnterPistolState);
        signalBus.Unsubscribe<ExitPistolStateSignal>(OnExitPistolState);
    }
}
