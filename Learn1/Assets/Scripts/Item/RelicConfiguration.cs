using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRelicConfig", menuName = "Game/Config/Relic")]
public class RelicConfiguration : ScriptableObject
{
    [Tooltip("名字")]
    public string relicName;
    [Tooltip("道具描述")]
    public string description;
    [Tooltip("道具图标")]
    public Sprite icon; 
    [Tooltip("道具价格")]
    public int price; 
    [Tooltip("最大叠加数量")]
    public int maxCount; 
    [Tooltip("道具ID")]
    public int relicID;
    [Tooltip("预制体地址")]
    public string prefabPath;
    [Tooltip("道具形状")]
    public List<Vector2Int> shape = new List<Vector2Int>() {new Vector2Int(0,0)};
}
