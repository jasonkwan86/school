using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 3;
    [SerializeField] private ui_HeartController ui_HeartController;
    private int _health;

    public void Start()
    {
        Health = MaxHealth;
    }

    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            ui_HeartController.UpdateHeartsUI(this);
        }
    }

    public void Heal(int healPoints)
    {
        Health = Math.Min(MaxHealth, Health + healPoints);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            GameManager.RestartLevel();
        }
    }
}
