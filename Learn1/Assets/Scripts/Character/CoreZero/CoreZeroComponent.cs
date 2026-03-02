using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreZeroComponent : MonoBehaviour, ICharacter
{
    private CharacterConfiguration config;
    private HealthComponent healthComponent;
    private EnergyComponent energyComponent;
    private BaseMoveComponent_Character moveComponent;
    private CoreZeroAttackComponent attackComponent;


    private float lastHitTime = -999f;

    public Stat maxHealth => healthComponent.MaxHealth;

    public float currentHealth => healthComponent.CurrentHealth;

    public Stat healthRegenrate => healthComponent.HealthRegenrate;

    public Stat maxEnergy => energyComponent.MaxEnergy;

    public float currentEnergy => energyComponent.CurrentEnergy;

    public Stat energyRegenrate => energyComponent.EnergyRegenrate;

    public Stat hitInterval => healthComponent.HitInterval;
    public Stat moveSpeed => moveComponent.MoveSpeed;

    public void Init(CharacterConfiguration config)
    {
        this.config = config;
        healthComponent = GetComponent<HealthComponent>();
        healthComponent?.Init(config);
        energyComponent = GetComponent<EnergyComponent>();
        energyComponent?.Init(config);
        moveComponent = GetComponent<BaseMoveComponent_Character>();
        moveComponent?.Init(config);
        attackComponent = GetComponent<CoreZeroAttackComponent>();
        attackComponent?.Init(config);

        lastHitTime = -999f;
    }

    public void RestoreFullState()
    {
        healthComponent.ResetHealth();
        energyComponent.ResetEnergy();
        lastHitTime = -999f;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (attackComponent.CanAttack())
            {
                attackComponent.Attack();
                energyComponent.ConsumeEnergy(attackComponent.AttackEnergyCost.FinalValue);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if (enemy != null && Time.time >= lastHitTime + healthComponent.HitInterval.FinalValue) 
        {
            lastHitTime = Time.time;
            healthComponent.TakeDamage(enemy.Damage.FinalValue);
        }
    }
}
