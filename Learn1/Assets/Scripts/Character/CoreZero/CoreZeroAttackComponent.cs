using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreZeroAttackComponent : MonoBehaviour, IAttack
{
    private Stat damage;
    private Stat attackSpeed;
    private Stat attackEnergyCost;

    private float lastAttackTime = -999f;
    private float attackInterval => 1f / attackSpeed.FinalValue;
    private ObjectPool bulletPool;
    private EnergyComponent energyComponent;

    [SerializeField] private Transform bulletPoint;
    [SerializeField] private GameObject bulletPrefab;

    public void Init(CharacterConfiguration config)
    {
        damage = new(config.damage, 0);
        attackSpeed = new(config.attackSpeed, 0.01f);
        attackEnergyCost = new(config.attackEnergyCost, 0);

        bulletPool = ObjectPoolManager.Instance.CreatePool("CoreZero_Bullet", bulletPrefab, 20);
        if (bulletPoint == null)
        {
            Debug.LogError("Bullet Point is not assigned.");
        }

        energyComponent = GetComponent<EnergyComponent>();
    }

    public Stat Damage => damage;
    public Stat AttackSpeed => attackSpeed;
    public Stat AttackEnergyCost => attackEnergyCost;

    public bool CanAttack()
    {
        if( energyComponent == null) return false;
        if (energyComponent.CurrentEnergy < attackEnergyCost.FinalValue) return false;
        if(Time.time < attackInterval + lastAttackTime) return false;
        return true;
    }

    public void Attack()
    {
        lastAttackTime = Time.time;
        var bullet = bulletPool.GetObject();
        bullet.transform.position = bulletPoint.position;
        bullet.transform.rotation = bulletPoint.transform.rotation;
        var b = bullet.GetComponent<DefaultCharacter_Bullet>();
        b.Init(new BulletDataPack(20f, Damage.FinalValue));
    }
}
