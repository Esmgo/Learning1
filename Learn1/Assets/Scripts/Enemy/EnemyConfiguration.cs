using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new config", menuName = "Game/Config/Enemy")]
public class EnemyConfiguration : ScriptableObject
{
    [Header("基本信息")]
    [Tooltip("名字")]
    public string enemyName;
    [Tooltip("AA地址")]
    public string address;
    [Tooltip("最大生命")]
    public float maxHealth;
    [Tooltip("生命回复")]
    public float healthRegenRate;
    [Tooltip("伤害")]
    public float damage;
    [Tooltip("移动速度")]
    public float moveSpeed;
}
