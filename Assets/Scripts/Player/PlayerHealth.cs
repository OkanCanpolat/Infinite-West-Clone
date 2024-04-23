using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IRestorable
{
    public event Action<int> OnHealthChanged;
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private bool isAlive;
    private Animator animator;

    #region Getter/Setter
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isAlive = true;
    }
    public void RestoreHealth(int value)
    {
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth);
        animator.SetTrigger("Damage");

        if (currentHealth <= 0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}
