using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStatComponent))]
[RequireComponent(typeof(BaseMoveComponent_Character))]
public class CharacterComponent : MonoBehaviour,IBuffOwner
{
    public float maxHealth => statComponent?.MaxHealth.FinalValue ?? -1;
    public float currentHealth => statComponent?.CurrentHealth ?? -1;
    public float healthRegenRate => statComponent?.HealthRegenrate.FinalValue ?? -1;
    public float maxEnergy => statComponent?.MaxEnergy.FinalValue ?? -1;
    public float currentEnergy => statComponent?.CurrentEnergy ?? -1;
    public float energyRegenRate => statComponent?.EnergyRegenrate.FinalValue ?? -1;

    protected CharacterConfiguration config;        //配置文件
    protected CharacterStatComponent statComponent;     //数值组件
    protected BaseMoveComponent_Character moveComponent;     //移动组件
    protected CharacterWeaponComponent weaponComponent;     //武器组件

    protected float lastAttackTime = -999f;     //上次攻击时间

    public Action OnInfoChanged;        //信息变更回调

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="config"></param>
    public void Init(CharacterConfiguration config)
    {
        this.config = config;
        statComponent = GetComponent<CharacterStatComponent>();
        moveComponent = GetComponent<BaseMoveComponent_Character>();
        weaponComponent = GetComponent<CharacterWeaponComponent>();

        statComponent?.Init(config);
        moveComponent?.Init(config);
        weaponComponent?.Init(config);
        OnInfoChanged?.Invoke();
    }

    protected void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HandleAttackInput();
        }
    }

    protected void HandleAttackInput()
    {
        if (CanAttack())
        {
            weaponComponent.Attack();
            lastAttackTime = Time.time;
            //statComponent.ConsumeEnergy(weaponComponent.attackEnergyCost * -1);

            OnInfoChanged?.Invoke();
        }
    }

    protected bool CanAttack()
    {
        if(_CanAttack())
        if (weaponComponent == null || statComponent == null) return false;
        //if (statComponent.CurrentEnergy < weaponComponent.attackEnergyCost) return false;
        //if (Time.time < weaponComponent.attackInterval + lastAttackTime) return false;
        return true;
    }

    protected virtual bool _CanAttack() { return true; }

    public void TakeDamage(int value) 
    {
        statComponent.TakeDamage(value);
        _OnTakeDamage();
        OnInfoChanged?.Invoke();
    }

    protected virtual void _OnTakeDamage() { }

    public void TakeHeal(int value)
    {
        //statComponent.Health(value);
        _OnTakeDamage();
        OnInfoChanged?.Invoke();
    }

    protected virtual void _OnTakeHeal() 
    {
    }

    public void Energy(int value)
    {
        //statComponent.Energy(value);
        _OnEnergyChanged();
        OnInfoChanged?.Invoke();
    }

    protected virtual void _OnEnergyChanged()
    {

    }
}
