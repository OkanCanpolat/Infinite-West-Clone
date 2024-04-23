using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    [SerializeField] private List<SkillBase> skills;

    public void AddSkill(SkillType type)
    {
        foreach (SkillBase skill in skills)
        {
            if (skill.SkillType == type)
            {
                skill.AddSkill();
            }
        }
    }
}
