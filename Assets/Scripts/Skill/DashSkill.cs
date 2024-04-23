using TMPro;
using UnityEngine;
using Zenject;

public class DashSkill : SkillBase
{
    private Player player;
    private Animator animator;
    private bool activated;
    [Inject(Id = "Dash")] private IPlayerState dashState;
    [Inject(Id = "Idle")] private IPlayerState idleState;

    [Header("UI")]
    [SerializeField] private GameObject activateSprite;
    [SerializeField] private GameObject deactivateSprite;
    [SerializeField] private TMP_Text maxSkillCountText;
    [SerializeField] private TMP_Text currentSkillCountText;

    [Inject]
    public void Construct(Player player)
    {
        this.player = player;
        animator = player.GetComponent<Animator>();
    }

    private void Awake()
    {
        maxSkillCountText.text = MaxSkillCount.ToString();
        currentSkillCountText.text = CurrentSkillCount.ToString();
    }
    public override void OnClick()
    {
        if (CurrentSkillCount <= 0) return;

        if (activated)
        {
            animator.SetTrigger("Shake");
            activateSprite.SetActive(false);
            deactivateSprite.SetActive(true);
            player.stateMachine.ChangeStete(idleState);
            activated = false;
        }
        else
        {
            activateSprite.SetActive(true);
            deactivateSprite.SetActive(false);
            player.stateMachine.ChangeStete(dashState);
            activated = true;
        }
    }
    public void OnDash()
    {
        activated = false;
        activateSprite.SetActive(false);
        deactivateSprite.SetActive(true);
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
