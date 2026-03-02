using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponComponent : MonoBehaviour
{
    private Stat damage;
    private Stat attackInterval;
    public Stat attackEnergyCost { get; private set; }


    public Stat Damage => damage;
    public Stat AttackSpeed => attackInterval;

    /// <summary>
    /// ≥ı ºªØ
    /// </summary>
    /// <param name="config"></param>
    public virtual void Init(CharacterConfiguration config)
    {
        //damage = new(config.damage, 0);
        //attackInterval = new(config.attackSpeed, 0.01f);
        //attackEnergyCost = new(config.attackEnergyCost, 0);
    }

    public void UpdateStats(WeaponStatPack pack)
    {

    }

    public virtual void Attack() { }
}

public struct WeaponStatPack
{
    public int damage;
    public float attackInterval;
    public int attackEnergyCost;
    public WeaponStatPack(int damage, float attackInterval, int attackEnergyCost)
    {
        this.damage = damage;
        this.attackInterval = attackInterval;
        this.attackEnergyCost = attackEnergyCost;
    }
}
