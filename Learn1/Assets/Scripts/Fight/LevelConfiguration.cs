using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfiguration", menuName = "Game/Config/Level", order = 1)]
[Serializable]
public class LevelConfiguration : ScriptableObject
{
    public List<SummonConfiguration> levelConfig = new();
}

[Serializable]
public class SummonConfiguration
{
    public EnemyConfiguration enemyConfig;
    [Tooltip("最小数量")]
    public int minNumber;
    [Tooltip("最大数量")]
    public int maxNumber;
    [Tooltip("开始出现波次")]
    public int spawnWave;
}