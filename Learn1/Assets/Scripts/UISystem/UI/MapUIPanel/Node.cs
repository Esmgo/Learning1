using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int PosX { get; private set; }
    public int PosY { get; private set; }
    public List<Node> Connections { get; private set; } = new List<Node>();
    public List<Node> BeConnecteds { get; private set; } = new List<Node>();//反向连接，记录哪些节点连接了这个节点

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image icon;

    [SerializeField] private Sprite eventIcon;
    [SerializeField] private Sprite shopIcon;
    [SerializeField] private Sprite fightIcon;
    [SerializeField] private Sprite restIcon;
    [SerializeField] private Sprite treasureIcon;

    private NodeType nodeType;

    public void Init(int x, int y)
    {
        PosX = x;
        PosY = y;
        text.text = $"({x}, {y})";
    }

    public void AddConnection(Node node)
    {
        if (!Connections.Contains(node))
        {
            Connections.Add(node);
        }
        if(!node.BeConnecteds.Contains(this))
        {
            node.BeConnecteds.Add(this);
        }
    }

    public int GetLeftXConnected()
    {
        int leftX = int.MaxValue;
        foreach(Node node in Connections)
        {
            if (node.PosX < leftX)
            {
                leftX = node.PosX;
            }
        }
        return leftX;
    }

    public int GetRightXConnected()
    {
        int rightX = int.MinValue;
        foreach (Node node in Connections)
        {
            if (node.PosX > rightX)
            {
                rightX = node.PosX;
            }
        }
        return rightX;
    }
    
    public void SetNodeType(NodeType type)
    {
        nodeType = type;
        UpdateDisplay(type);
    }

    private void UpdateDisplay(NodeType type)
    {
        switch(type)
        {
            case NodeType.Event:
                icon.sprite = eventIcon;
                break;
            case NodeType.Shop:
                icon.sprite = shopIcon;
                break;
            case NodeType.Fight:
                icon.sprite = fightIcon;
                break;
            case NodeType.Rest:
                icon.sprite = restIcon;
                break;
            case NodeType.Treasure:
                icon.sprite = treasureIcon;
                break;
            case NodeType.EliteFight:
                icon.sprite = fightIcon;
                icon.color = Color.red;
                break;
        }
    }

     public NodeType GetNodeType()
    {
        return nodeType;
    }

}

public enum NodeType
{
    Event,
    Shop,
    Fight,
    EliteFight,
    Rest,
    Treasure
}
