using UnityEngine;

public interface IAttack  
{
    public Vector2Int[] AttackableDirections { get; }
    public void Attack();
    public bool IsEnemyInRange();
}
