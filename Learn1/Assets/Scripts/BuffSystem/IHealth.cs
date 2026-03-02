using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实现此接口以使对象拥有可被Buff系统修改的生命属性
/// </summary>
public interface IHealth : IBuffTarget
{
    Stat MaxHealth { get; }
    float CurrentHealth { get; }
    Stat HealthRegenrate { get; }
    Stat HitInterval { get; }

    void TakeDamage(float value);
    void Heal(float value);
    void RegenrateHealth(float deltaTime);

    event Action OnHealthChange;
    event Action OnDead;
}
