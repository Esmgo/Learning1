using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareComponent : MonoBehaviour, IEnemy, IPoolable
{
    private HealthComponent healthComponent;
    private BaseMoveComponent_Enemy moveComponent;
    private CommonEnemyAttackComponent attackComponent;
    public Stat MaxHealth => healthComponent.MaxHealth;
    public float CurrentHealth => healthComponent.CurrentHealth;

    public Stat MoveSpeed => moveComponent.MoveSpeed;
    public Stat Damage => attackComponent.Damage;
    public void Init(EnemyConfiguration config)
    {
        healthComponent = GetComponent<HealthComponent>();
        healthComponent.Init(config);
        moveComponent = GetComponent<BaseMoveComponent_Enemy>();
        moveComponent.Init(config);
        attackComponent = GetComponent<CommonEnemyAttackComponent>();
        attackComponent.Init(config);

        healthComponent.OnDead += Dead;
    }

    public void TakeDamage(float damage)
    {
        FloatingTextManager.Instance.ShowDamageText(damage, transform.position); 
        healthComponent.TakeDamage(damage);
    }

    private void Dead()
    {
        GetComponent<PooledObject>()?.ReturnToPool();
    }

    public void OnGetFromPool()
    {
        
    }

    public void OnReturnToPool()
    {
        healthComponent.OnDead -= Dead;
    }
}
