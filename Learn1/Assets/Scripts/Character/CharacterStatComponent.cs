using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatComponent : MonoBehaviour
{
    private Stat maxHealth;
    private float currentHealth;
    private Stat healthRegenRate;
    private Stat maxEnergy;
    private float currentEnergy;
    private Stat energyRegenRate;

    public Stat MaxHealth => maxHealth;

    public float CurrentHealth => currentHealth;

    public Stat HealthRegenrate => healthRegenRate;

    public Stat MaxEnergy => maxEnergy;

    public float CurrentEnergy => currentEnergy;

    public Stat EnergyRegenrate => energyRegenRate;

    public void Init(CharacterConfiguration config)
    {
        //maxHealth = new(config.maxHealth);
        //currentHealth = maxHealth.FinalValue;
        //healthRegenRate = new(config.healthRegenRate);

        //maxEnergy = new(config.maxEnergy);
        currentEnergy = maxEnergy.FinalValue;
        //energyRegenRate = new(config.energyRegenRate);
    }

    private void Update()
    {
        RegenrateHealth(Time.deltaTime);
        RegenrateEnergy(Time.deltaTime);
    }

    public void UpdateStats(CharacterStatPack pack)
    {
        
    }


    public void TakeDamage(float value)
    {
        currentHealth = Mathf.Max(currentHealth - value, 0);
    }

    public void Heal(float value)
    {
        currentHealth = Mathf.Min(maxHealth.FinalValue, currentHealth + value);
    }

    public void RegenrateHealth(float deltaTime)
    {
        Heal(healthRegenRate.FinalValue * deltaTime);
    }

    public void ConsumeEnergy(float value)
    {
        currentEnergy = Mathf.Max(currentEnergy - value, 0);
    }

    public void RestoreEnergy(float value)
    {
        currentEnergy = Mathf.Min(maxEnergy.FinalValue, currentEnergy + value);
    }

    public void RegenrateEnergy(float deltaTime)
    {
        RestoreEnergy(energyRegenRate.FinalValue * deltaTime);
    }

    public struct CharacterStatPack
    {
        public int maxHealth;
        public float healthRegen;
        public int maxEnergy;
        public float energyRegen;

        public CharacterStatPack(int maxHealth, float healthRegen, int maxEnergy, float energyRegen)
        {
            this.maxHealth = maxHealth;
            this.healthRegen = healthRegen;
            this.maxEnergy = maxEnergy;
            this.energyRegen = energyRegen;
        }
    }
}
