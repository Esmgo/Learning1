using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new config", menuName = "Game/Config/Character")]
public class CharacterConfiguration : ScriptableObject
{
    [Header("基本信息")]
    [Tooltip("名字")]
    public string characterName;
    [Tooltip("描述")]
    public string description;

    [Tooltip("预制体地址")]
    public string address;

    [Header("属性")]
    [Tooltip("最大生命")]
    public float maxHealth;
    [Tooltip("生命回复速度")]
    public float healthRegenRate;
    [Tooltip("最大能量")]
    public float maxEnergy;
    [Tooltip("能量回复速度")]
    public float energyRegenRate;
    [Tooltip("移动速度")]
    public float moveSpeed;
    [Tooltip("冲刺速度")]
    public float dashSpeed;
    [Tooltip("冲刺冷却时间")]
    public float dashCooldown;
    [Tooltip("冲刺持续时间")]
    public float dashDuration;
    [Tooltip("伤害")]
    public float damage;
    [Tooltip("攻击速度")]
    public float attackSpeed;
    [Tooltip("攻击耗能")]
    public float attackEnergyCost;
    
}
