using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Zenject;

public class PistolSkill : SkillBase
{
    private Player player;
    private Animator animator;
    private bool activated;
    [Inject(Id = "Pistol")] private IPlayerState pistolState;
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
    }
    private void Awake()
    {
        maxSkillCountText.text = MaxSkillCount.ToString();
        currentSkillCountText.text = CurrentSkillCount.ToString();
        animator = player.GetComponent<Animator>();
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
            player.stateMachine.ChangeStete(pistolState);
            activated = true;
        }
    }
    public void OnShot()
    {
        activated = false;
        deactivateSprite.SetActive(true);
        activateSprite.SetActive(false);
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
