using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : IRelic
{
    private float value = 0.01f;
    private PercentModifier p1;
    public Banana(RelicConfiguration config)
    {
        itemName = config.relicName;
        itemDescription = config.description;
        p1 = new(value);
    }
    public override void OnGet(ICharacter owner)
    {
        p1.SetValue(value * RelicManager.Instance.GetCount(itemName));
        if(owner.moveSpeed.Modifiers.Contains(p1)) owner.moveSpeed.AddModifier(p1);
    }

    public override void OnUpdate()
    {
        
    }
}
    
