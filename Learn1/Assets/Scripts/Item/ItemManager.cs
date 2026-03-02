using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private Dictionary<string, (IItem item, int count)> ownedItems = new();

    public void GetItem(ICharacter character, ItemConfiguration config)
    {
        if(ownedItems.ContainsKey(config.itemName))
        {
            var (item, count) = ownedItems[config.itemName];
            if (count < config.maxCount)
            {
                ownedItems[config.itemName] = (item, count + 1);
                item.OnGet(character);
            }
        }
        else
        {
            ownedItems[config.itemName] = (CreatItem(config), 1);
            ownedItems[config.itemName].item.OnGet(character);
        }
    }

    private IItem CreatItem(ItemConfiguration config)
    {
        if (ownedItems.ContainsKey(config.itemName)) return ownedItems[config.itemName].item;
        return config.itemType switch
        {
            ItemConfiguration.ItemType.Apple => new Apple(config),
            ItemConfiguration.ItemType.Banana => new Banana(config),
            _ => null
        };
    }

    public int GetCount(string name)
    {
        if(ownedItems.ContainsKey(name))
        {
            return ownedItems[name].count;
        }
        else
        {
            return 0;
        }
    }
}
