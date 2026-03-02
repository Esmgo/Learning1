using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule
{
    ICharacter Owner { get; }
    int Level { get; }
    int MaxLevel { get; }
    string Description { get; }
    void Init(int level);
    void OnInstall(ICharacter owner);
    void OnUninstall();
    void OnUpdate();
    void Upgrade();
}

/// <summary>
/// 定义一个模块效果，清晰地描述了要修改的目标属性和具体的修饰器。
/// </summary>
public class ModuleEffect
{
    public TargetStatType TargetStat { get; }
    public Modifier Modifier { get; }

    public ModuleEffect(TargetStatType target, Modifier modifier)
    {
        TargetStat = target;
        Modifier = modifier;
    }
}

/// <summary>
/// 枚举，定义所有可以被模块影响的Stat。
/// 这个文件可以放在一个核心的 "Enums" 文件夹中。
/// </summary>
public enum TargetStatType
{
    // 生命相关
    MaxHealth,
    HealthRegenRate,

    // 攻击相关
    Damage,
    AttackSpeed,

    // 移动相关
    MoveSpeed,

    // ... 添加所有你想让模块影响的属性
}