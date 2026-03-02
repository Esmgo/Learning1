using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现此接口以使对象拥有可被Buff系统修改的能量属性
/// </summary>
public interface IEnergy : IBuffTarget
{
    Stat MaxEnergy { get; }
    float CurrentEnergy { get; }
    Stat EnergyRegenrate { get; }
    void ConsumeEnergy(float value);
    void RestoreEnergy(float  value);
    void RegenrateEnergy(float deltaTime);

    event Action OnEnergyChange;
}
