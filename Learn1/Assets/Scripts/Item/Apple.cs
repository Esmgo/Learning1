using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : IItem
{
    private float value = 10;
    private FlatModifier m1;
    public Apple(ItemConfiguration config)
    {
        itemName = config.itemName;
        itemDescription = config.description;
        m1 = new FlatModifier(value);
    }
    public override void OnGet(ICharacter owner)
    {
        m1.SetValue(value * ItemManager.Instance.GetCount(itemName));
        if(!owner.maxHealth.Modifiers.Contains(m1)) owner.maxHealth.AddModifier(m1);

    }

    public override void OnUpdate()
    {
        
    }
}
