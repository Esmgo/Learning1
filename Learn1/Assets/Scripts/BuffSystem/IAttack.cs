using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack : IBuffTarget
{
    Stat Damage { get; }
    Stat AttackSpeed { get; }
    Stat AttackEnergyCost { get; }

    bool CanAttack();
    void Attack();
}
