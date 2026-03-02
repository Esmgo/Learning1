using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatComponent : MonoBehaviour
{
    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public float healthRegenRate { get; private set; }


    public void Init(CharacterConfiguration config)
    {
        //maxHealth = config.maxHealth;
        currentHealth = maxHealth;
    }

    public void UpdateStats(EnemyStatPack pack)
    {
        maxHealth = pack.maxHealth;
        healthRegenRate = pack.healthRegen;
    }

    public void Health(int value)
    {
        int _value = currentHealth += value;
        if (_value > 0)
        {
            currentHealth = Mathf.Min(_value, maxHealth);
        }
        else
        {
            currentHealth = Mathf.Max(_value, 0);
        }
    }
    public struct EnemyStatPack
    {
        public int maxHealth;
        public float healthRegen;
        public int maxEnergy;
        public float energyRegen;

        public EnemyStatPack(int maxHealth, float healthRegen, int maxEnergy, float energyRegen)
        {
            this.maxHealth = maxHealth;
            this.healthRegen = healthRegen;
            this.maxEnergy = maxEnergy;
            this.energyRegen = energyRegen;
        }
    }
}
