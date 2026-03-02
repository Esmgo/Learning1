using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultCharacter_Bullet : Bullet
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage);
            GetComponent<PooledObject>()?.ReturnToPool();
        }
    }
}
