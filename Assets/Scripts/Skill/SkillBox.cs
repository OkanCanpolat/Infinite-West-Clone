using UnityEngine;
using Zenject;
public class SkillBox : MonoBehaviour, ICollectable
{
    public SkillType Type;
    public Tile tile;
    private SkillController skillController;

    [Inject]
    public void Construct(SkillController skillController)
    {
        this.skillController = skillController;
        tile.Collectable = this;
    }
    public void OnCollect()
    {
        skillController.AddSkill(Type);
        tile.Collectable = null;
        Destroy(gameObject);
    }
}
