using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IRelic 
{
    public string itemName { get; protected set; }
    public string itemDescription { get; protected set; }
    public abstract void OnGet(ICharacter owner);
    public abstract void OnUpdate();
}
