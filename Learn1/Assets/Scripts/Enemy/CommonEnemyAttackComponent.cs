using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonEnemyAttackComponent : MonoBehaviour, IAttack
{
    private Stat damage;
    private Stat attackSpeed;
    private Stat attackEnergyCost;
    public Stat Damage => damage;

    public Stat AttackSpeed => attackSpeed;

    public Stat AttackEnergyCost => attackEnergyCost;

    public void Init(EnemyConfiguration config)
    {
        damage = new(config.damage, 0);
        attackSpeed = new(0.01f, 0.01f);
        attackEnergyCost = new(0, 0);
    }
    public void Attack()
    {
        
    }

    public bool CanAttack()
    {
        return true;
    }
}
