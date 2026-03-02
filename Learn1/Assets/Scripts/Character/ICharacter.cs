using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    GameObject gameObject { get; }
    Stat maxHealth { get; }
    float currentHealth { get; }
    Stat healthRegenrate { get; }
    Stat maxEnergy { get; }
    float currentEnergy { get; }
    Stat energyRegenrate { get; }
    Stat hitInterval { get; }
    Stat moveSpeed { get; }
    void Init(CharacterConfiguration config);
    void RestoreFullState();
}
