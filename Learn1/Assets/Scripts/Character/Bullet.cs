using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour,IPoolable
{
    [SerializeField] private Rigidbody2D rb;    //子弹的刚体组件
    private float lifeStartTime = -999;
    private float lifeTime = 3;

    private float speed;    //子弹速度
    protected float damage;     //子弹伤害

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void OnDestroy()
    {
        TimeManager.Instance.recycleTrigger -= ReturnToPool;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="pack"></param>
    public void Init(BulletDataPack pack)
    {
        speed = pack.speed;
        damage = pack.damage;
        rb.velocity = transform.right * speed;
    }

    private void ReturnToPool()
    {
        if(lifeStartTime + lifeTime <= Time.time)
        {
            GetComponent<PooledObject>()?.ReturnToPool();
        }
    }

    public void OnGetFromPool()
    {
        lifeStartTime = Time.time;
        TimeManager.Instance.recycleTrigger += ReturnToPool;
    }

    public void OnReturnToPool()
    {
        lifeStartTime = -999;
        TimeManager.Instance.recycleTrigger -= ReturnToPool;
    }
}

/// <summary>
/// 子弹数据包
/// </summary>
public struct BulletDataPack
{
    public float speed;
    public float damage;

    public BulletDataPack(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
    }
}
