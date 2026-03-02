using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IHealth
{
    private Stat maxHealth;
    private float currentHealth;
    private Stat healthRegenRate;
    private Stat hitInterval;

    public event Action OnHealthChange;
    public event Action OnDead;

    private float healthRegen;


    public Stat MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public Stat HealthRegenrate => healthRegenRate;
    public Stat HitInterval => hitInterval;


    public void Init(CharacterConfiguration config)
    {
        maxHealth = new(config.maxHealth, 1);
        currentHealth = maxHealth.FinalValue;
        healthRegenRate = new(config.healthRegenRate, 0);
        healthRegen = 0;
        hitInterval = new(0.2f, 0.01f);

        OnHealthChange?.Invoke();
    }

    public void Init(EnemyConfiguration config)
    {
        maxHealth = new(config.maxHealth, 1);
        currentHealth = maxHealth.FinalValue;
        healthRegenRate = new(config.healthRegenRate, 0);
        healthRegen = 0;

        OnHealthChange?.Invoke();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth.FinalValue;
        healthRegen = 0;
        OnHealthChange?.Invoke();
    }

    private void Update()
    {
        RegenrateHealth(Time.deltaTime);
    }

    public void Heal(float value)
    {
        currentHealth = Mathf.Min(maxHealth.FinalValue, currentHealth + value);
        OnHealthChange?.Invoke();
    }

    public void RegenrateHealth(float deltaTime)
    {
        healthRegen += deltaTime * healthRegenRate.FinalValue;
        if(healthRegen >= 1 && currentHealth != maxHealth.FinalValue)
        {
            Heal(Mathf.Floor(healthRegen));
            healthRegen -= Mathf.Floor(healthRegen);
            OnHealthChange?.Invoke();
        }
    }

    public void TakeDamage(float value)
    {
        currentHealth = Mathf.Max(currentHealth - value, 0);
        if(currentHealth <= 0)
        {
            OnDead?.Invoke();
        }
        OnHealthChange?.Invoke();
    }
}
