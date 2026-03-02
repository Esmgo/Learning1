using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ModuleConfigurationBase : ScriptableObject
{
    [Header("配置")]
    [Tooltip("模块名字")]
    public string moduleName;
    [Tooltip("描述")]
    [TextArea] public string description;
    [Tooltip("模块图标")]
    public Sprite icon;
}
[Serializable]
public class FloatLevelCurve
{
    [Tooltip("从1级开始，索引0=1级，默认1~5级")]
    public List<float> values = new List<float> { 1,2,3,4,5 }; 
}
