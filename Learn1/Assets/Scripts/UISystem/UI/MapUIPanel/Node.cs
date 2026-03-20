using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int PosX { get; private set; }
    public int PosY { get; private set; }
    public List<Node> Connections { get; private set; } = new List<Node>();

    [SerializeField] private TextMeshProUGUI text;

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
}
