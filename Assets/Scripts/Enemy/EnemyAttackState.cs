
public class EnemyAttackState : IEnemyState
{
    private IAttack attack;
    public EnemyAttackState(IAttack attack)
    {
        this.attack = attack;
    }
    public void OnEnter()
    {
        attack.Attack();
    }
    public void OnExit()
    {
    }
}
