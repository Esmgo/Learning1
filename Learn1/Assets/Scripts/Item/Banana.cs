using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : IItem
{
    private float value = 0.1f;
    private PercentModifier p1;
    public Banana(ItemConfiguration config)
    {
        itemName = config.itemName;
        itemDescription = config.description;
        p1 = new(value);
    }
    public override void OnGet(ICharacter owner)
    {
        p1.SetValue(value * ItemManager.Instance.GetCount(itemName));
        if(owner.moveSpeed.Modifiers.Contains(p1)) owner.moveSpeed.AddModifier(p1);
    }

    public override void OnUpdate()
    {
        
    }
}
    
