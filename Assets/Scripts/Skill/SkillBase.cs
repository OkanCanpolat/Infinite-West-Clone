using UnityEngine;

public enum SkillType
{
    Dash, Pistol, Heal
}
public abstract class SkillBase : MonoBehaviour
{
    public int CurrentSkillCount;
    public int MaxSkillCount;
    public SkillType SkillType;
    public abstract void OnClick();
    public abstract void AddSkill();
}
