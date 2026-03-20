using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance { get; private set; }
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


    private Dictionary<string, (IRelic item, int count)> ownedItems = new();

    public void GetItem(ICharacter character, RelicConfiguration config)
    {
        if(ownedItems.ContainsKey(config.relicName))
        {
            var (item, count) = ownedItems[config.relicName];
            if (count < config.maxCount)
            {
                ownedItems[config.relicName] = (item, count + 1);
                item.OnGet(character);
            }
        }
        else
        {
            ownedItems[config.relicName] = (CreatItem(config), 1);
            ownedItems[config.relicName].item.OnGet(character);
        }
    }

    private IRelic CreatItem(RelicConfiguration config)
    {
        if (ownedItems.ContainsKey(config.relicName)) return ownedItems[config.relicName].item;
        switch(config.relicID) 
        {
            case 0: return new Apple(config); 
            case 1: return new Banana(config); 
            default: return null;
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
