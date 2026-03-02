using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemConfig", menuName = "Game/Config/Item")]
public class ItemConfiguration : ScriptableObject
{
    public string itemName; // 道具名称
    public string description; // 道具描述
    public Sprite icon; // 道具图标
    public int price; // 道具价格
    public int maxCount; // 最大叠加数量
    public ItemType itemType; // 道具类型

    public enum ItemType
    {
        Apple,
        Banana
    }
}
