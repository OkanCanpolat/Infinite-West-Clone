using UnityEngine;
using Zenject;

public class FinishTileCollider : MonoBehaviour
{
    private Tile tile;
    private Player player;
    private TileManager tileManager;
    private Animator animator;
    private SignalBus signalBus;
    private LightController lightController;
    [Inject(Id = "Locked")] private IPlayerState lockedState;

    [Inject]
    public void Construct(Player player, TileManager tileManager, SignalBus signalBus, LightController lightController)
    {
        this.player = player;
        this.tileManager = tileManager;
        this.signalBus = signalBus;
        this.lightController = lightController;
        animator = GetComponent<Animator>();
    }
    private void Awake()
    {
        signalBus.Subscribe<LevelFinishedSignal>(Fade);
        tile = GetComponent<Tile>();
    }
    private async void OnMouseDown()
    {
        Vector2Int tilePosition = tileManager.GetTile(tile);
        if (tilePosition == player.currentTile) return;

        player.stateMachine.ChangeStete(lockedState);
        player.currentTile = tilePosition;
        player.destination = transform.position;
        await player.Movement();
        await lightController.FadeOut();
        signalBus.TryFire<NextSceneSignal>();
    }
    private void Fade()
    {
        tileManager.EnableIndicator(new Vector2Int(tile.X, tile.Y), Color.blue);
        animator.SetTrigger("Fade");
    }
    private void OnDestroy()
    {
        signalBus.Unsubscribe<LevelFinishedSignal>(Fade);
    }
}
