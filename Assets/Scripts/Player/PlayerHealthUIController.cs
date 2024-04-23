using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUIController : MonoBehaviour
{
    [SerializeField] private HealthUI healthUIPrefab;
    [SerializeField] private Transform healthParent;
    [SerializeField] private PlayerHealth playerHealth;
    private List<HealthUI> healthUIs;

    private void Awake()
    {
        healthUIs = new List<HealthUI>();
    }
    private void Start()
    {
        playerHealth.OnHealthChanged += OnHealthChanged;
        InitUI();
    }
    private void InitUI()
    {
        int maxHealth = playerHealth.MaxHealth;
        int currentHealth = playerHealth.CurrentHealth;

        for (int i = 0; i < maxHealth; i++)
        {
            HealthUI healthUI = Instantiate(healthUIPrefab, healthParent);
            healthUIs.Add(healthUI);
        }
        for (int i = 0; i < currentHealth; i++)
        {
            healthUIs[i].healthInterior.SetActive(true);
        }
    }
    private void OnHealthChanged(int currentHealth)
    {
        int counter = 0;

        for (int i = 0; i < healthUIs.Count; i++)
        {
            if (counter < currentHealth)
            {
                healthUIs[i].healthInterior.SetActive(true);
            }
            else
            {
                healthUIs[i].healthInterior.SetActive(false);
            }
            counter++;
        }
    }
}
