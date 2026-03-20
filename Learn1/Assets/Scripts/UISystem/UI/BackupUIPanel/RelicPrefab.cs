using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 只负责UI相关的
/// </summary>
public class RelicPrefab : MonoBehaviour, IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    private bool isDragging = false;
    private bool isMouseDown = false;
    private RelicConfiguration config;
    private List<Image> fillCells = new List<Image>();
    private Vector2Int ownedPosition = new Vector2Int(-1, -1);

    [SerializeField] private Transform fillCellContent;
    [SerializeField] private GameObject fillCellPrefab;

    public Action<RelicPrefab> OnDrag;
    public Action<RelicPrefab> OnMouseUp;
    
    public List<Vector2Int> GetShape()
    {
        return config.shape;
    }

    public Vector2Int GetPosition()
    {
        return ownedPosition;
    }
    public void SetPosition(Vector2Int pos)
    {
        ownedPosition = pos;
    }

    public void Init(RelicConfiguration config)
    {
        this.config = config;
        foreach(var pos in config.shape)
        {
            GameObject cell = Instantiate(fillCellPrefab, fillCellContent);
            cell.GetComponent<RectTransform>().anchoredPosition = new Vector2 (pos.x * 50, pos.y * 50);
            fillCells.Add(cell.GetComponent<Image>());
        }
    }

    private void Update()
    {
        if(isDragging)
        {
            Vector2 mousePosition = Input.mousePosition;
            transform.position = mousePosition;
            
        }
    }

    private bool isSeted = false;
    private void SetRayCastOn()
    {
        foreach(var img in fillCells)
        {
            img.raycastTarget = true;
        }
        isSeted = false;
    }

    private void SetRayCastOff()
    {
        foreach (var img in fillCells)
        {
            img.raycastTarget = false;
        }
        isSeted = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        isMouseDown = false;
        OnMouseUp?.Invoke(this);
        SetRayCastOn();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (isMouseDown)
        {
            isDragging = true;
            OnDrag?.Invoke(this);
            SetRayCastOff();
        }
    }
}
