using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : IRelic
{
    private float value = 10;
    private FlatModifier m1;
    public Apple(RelicConfiguration config)
    {
        itemName = config.relicName;
        itemDescription = config.description;
        m1 = new FlatModifier(value);
    }
    public override void OnGet(ICharacter owner)
    {
        m1.SetValue(value * RelicManager.Instance.GetCount(itemName));
        if(!owner.maxHealth.Modifiers.Contains(m1)) owner.maxHealth.AddModifier(m1);

    }

    public override void OnUpdate()
    {
        
    }
}
