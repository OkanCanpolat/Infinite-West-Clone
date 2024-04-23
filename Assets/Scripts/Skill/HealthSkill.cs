using TMPro;
using UnityEngine;
using Zenject;

public class HealthSkill : SkillBase
{
    [Inject(Id = "Heal")] private IPlayerState healState;
    private Player player;

    [Header("UI")]
    [SerializeField] private TMP_Text maxSkillCountText;
    [SerializeField] private TMP_Text currentSkillCountText;

    [Inject]
    public void Construct(Player player)
    {
        this.player = player;
    }

    private void Awake()
    {
        currentSkillCountText.text = CurrentSkillCount.ToString();
        maxSkillCountText.text = MaxSkillCount.ToString();
    }

    public override void OnClick()
    {
        if (CurrentSkillCount <= 0) return;
        if (player.stateMachine.CurrentState.GetType() != typeof(PlayerIdleState)) return;
        player.stateMachine.ChangeStete(healState);
    }
    public void OnUse()
    {
        CurrentSkillCount--;
        currentSkillCountText.text = CurrentSkillCount.ToString();
    }

    public override void AddSkill()
    {
        CurrentSkillCount++;
        CurrentSkillCount = Mathf.Clamp(CurrentSkillCount, 0, MaxSkillCount);
        currentSkillCountText.text = CurrentSkillCount.ToString();
    }
}
