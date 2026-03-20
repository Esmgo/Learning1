using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Grid : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public int x { get; private set; }
    public int y { get; private set; }

    [SerializeField] private Image image;
    [SerializeField] private Image color;
    public Action<Grid> OnMouseEnter;

    private RelicPrefab ownedRelic;
    private Color oriColor;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
        ownedRelic = null;
        oriColor = color.color;
    }

    public void SetColor(Color c)
    {
        color.color = c;
    }
    public void SetColor()
    {
        color.color = oriColor;
    }

    public void SetOwnedRelic(RelicPrefab ownedRelic)
    {
        this.ownedRelic = ownedRelic;
    }

    public bool IsEmpty()
    {
        return ownedRelic == null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.red;
        OnMouseEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
        OnMouseEnter?.Invoke(null);
    }
}
