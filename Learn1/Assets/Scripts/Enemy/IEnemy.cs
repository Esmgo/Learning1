using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    Stat MaxHealth { get; }
    float CurrentHealth { get; }
    Stat MoveSpeed { get; }
    Stat Damage { get; }
    void Init(EnemyConfiguration config);

    void TakeDamage(float damage); 
}