using System.Threading.Tasks;
using UnityEngine;
using Zenject;
public class PlayerHealthState : IPlayerState
{
    private Player player;
    private Animator animator;
    private TurnController turnController;
    private IRestorable health;
    private HealthSkill healthSkill;
    private const int healthAmount = 1;
    [Inject(Id = "Locked")] private IPlayerState lockedState;

    public PlayerHealthState(Player player, TurnController turnController, HealthSkill healthSkill)
    {
        this.player = player;
        this.turnController = turnController;
        this.healthSkill = healthSkill;
        animator = player.GetComponent<Animator>();
        health = player.GetComponent<IRestorable>();
    }
    public void OnEnemyClickDown(Enemy enemy)
    {
    }

    public async void OnEnter()
    {
        animator.SetTrigger("Heal");
        await Task.Delay(200);
        health.RestoreHealth(healthAmount);
        healthSkill.OnUse();
        player.stateMachine.ChangeStete(lockedState);
        turnController.EndPlayerTurn();
    }

    public void OnExit()
    {
    }

    public void OnPlayerClickDown()
    {
    }

    public void OnPlayerClickUp()
    {
    }
}
