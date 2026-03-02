using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyComponent : MonoBehaviour, IEnergy
{
    private Stat maxEnergy;
    private float currentEnergy;
    private Stat energyRegenRate;

    public event Action OnEnergyChange;

    private float energyRegen;

    public Stat MaxEnergy => maxEnergy;

    public float CurrentEnergy => currentEnergy;

    public Stat EnergyRegenrate => energyRegenRate;

    public void Init(CharacterConfiguration config)
    {
        maxEnergy = new Stat(config.maxEnergy, 1);
        currentEnergy = maxEnergy.FinalValue;
        energyRegenRate = new Stat(config.energyRegenRate, 0);
        energyRegen = 0;

        OnEnergyChange?.Invoke();
    }

    public void ResetEnergy()
    {
        currentEnergy = maxEnergy.FinalValue;
        energyRegen = 0;
        OnEnergyChange?.Invoke();
    }

    private void Update()
    {
        RegenrateEnergy(Time.deltaTime);
    }

    public void ConsumeEnergy(float value)
    {
        currentEnergy = Mathf.Max(currentEnergy - value, 0);
        OnEnergyChange?.Invoke();
    }

    public void RegenrateEnergy(float deltaTime)
    {
        energyRegen += energyRegenRate.FinalValue * deltaTime;
        if(energyRegen >= 1 && currentEnergy != maxEnergy.FinalValue)
        {
            RestoreEnergy(Mathf.Floor(energyRegen));
            energyRegen -= Mathf.Floor(energyRegen);
            OnEnergyChange?.Invoke();
            //PrintAllSubscribers();
        }
    }

    public void RestoreEnergy(float value)
    {
        currentEnergy = Mathf.Min(currentEnergy + value, MaxEnergy.FinalValue);
        OnEnergyChange?.Invoke();
    }

    public void PrintAllSubscribers()
    {
        if (OnEnergyChange == null)
        {
            Debug.Log("无订阅方法");
            return;
        }

        Debug.Log("所有订阅的方法：");
        foreach (var del in OnEnergyChange.GetInvocationList())
        {
            Debug.Log($"  方法名：{del.Method.Name} | 所属类：{del.Target?.GetType().Name}");
        }
    }
}
