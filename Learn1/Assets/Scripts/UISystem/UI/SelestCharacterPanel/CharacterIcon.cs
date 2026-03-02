using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterIcon : MonoBehaviour,IPointerClickHandler
{
    private CharacterConfiguration config;
    public Action<CharacterConfiguration> OnSelected;
    
    public void Init(CharacterConfiguration config)
    {
        this.config = config;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelected?.Invoke(config);
    }
}
