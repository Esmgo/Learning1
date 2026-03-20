using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIPanel : UIPanel
{
    private bool isInited = false;

    [SerializeField] private int floorNumber = 15;
    [SerializeField] private int groudFloorPosY = -500;
    [SerializeField] private int paddingX = 100;
    [SerializeField] private int nodeNumberX = 7;
    [SerializeField] private RectTransform mapRectTransform;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject nodeContent;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private UIButton regenerate;

    private float nodePadding = 0;
    
    private Dictionary<(int x, int y), Node> nodes = new();

    public override void OnOpen()
    {
        if (!isInited)
        {
            Init();
        }
    }
    private void Init()
    {
        regenerate.onClick += OnRegenerateButtonClicked;

        GenerateMap();
        isInited = true;
    }

    void OnDestroy()
    {
        regenerate.onClick -= OnRegenerateButtonClicked;
    }

    private void GenerateMap()
    {
        if(nodeNumberX > 1)
        {
            nodePadding = (mapRectTransform.rect.width - 2 * paddingX) / (nodeNumberX - 1);
        }
        else
        {
            nodePadding = 0;
        }
        Node startNode = CreateNode(0, 0);
        startNode.transform.SetParent(nodeContent.transform);
        startNode.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, groudFloorPosY);
        nodes[(0, 0)] = startNode;

        Node endNode = CreateNode(0, 15);
        endNode.transform.SetParent(nodeContent.transform);
        endNode.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, GetNodePos(0, floorNumber).y);
        nodes[(floorNumber, floorNumber)] = endNode;

        GeneratePaths(startNode, endNode);
    }
    private void OnRegenerateButtonClicked()
    {
               // 清除现有的节点
        foreach (Transform child in nodeContent.transform)
        {
            Destroy(child.gameObject);
        }
        nodes.Clear();
        // 重新生成地图
        GenerateMap();
    }

    private void GenerateLine(GameObject from, GameObject to)
    {
        // 获取起点和终点的位置
        Vector3 fromPosition = from.GetComponent<RectTransform>().anchoredPosition;
        Vector3 toPosition = to.GetComponent<RectTransform>().anchoredPosition;

        // 计算线的中点作为线的锚点
        Vector3 midPoint = (fromPosition + toPosition) / 2;

        // 计算线的方向和角度
        Vector3 direction = toPosition - fromPosition;
        float distance = direction.magnitude; // 线的长度
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 线的旋转角度

        // 实例化线预制体
        GameObject line = Instantiate(linePrefab);
        line.transform.SetParent(nodeContent.transform); // 设置父物体
        RectTransform lineRect = line.GetComponent<RectTransform>();

        if (lineRect == null)
        {
            Debug.LogError("linePrefab does not have a RectTransform component!");
            return;
        }

        // 设置线的位置、大小和旋转
        lineRect.anchoredPosition = midPoint; // 设置线的中点为起点和终点的中间
        lineRect.sizeDelta = new Vector2(distance, lineRect.sizeDelta.y); // 设置线的长度
        lineRect.localRotation = Quaternion.Euler(0, 0, angle); // 设置线的旋转角度

    }

    private void GeneratePaths(Node startNode, Node endNode)
    {
        // 生成第一层
        List<Node> firstLayer = GenerateLayer(1, 1, 4);

        foreach (Node node in firstLayer)
        {
            startNode.AddConnection(node);
            GenerateLine(startNode.gameObject, node.gameObject);
        }

        int generatedPathCount = 0;

        foreach (Node node in firstLayer)
        {
            Node finalNode = GeneratePath(node);
            finalNode.AddConnection(endNode);
            GenerateLine(finalNode.gameObject, endNode.gameObject);
            generatedPathCount++;
        }
        while (generatedPathCount < 5)
        {
            Node randomStartNode = firstLayer[Random.Range(0, firstLayer.Count)];
            Node finalNode = GeneratePath(randomStartNode);
            finalNode.AddConnection(endNode);
            GenerateLine(finalNode.gameObject, endNode.gameObject);
            generatedPathCount++;
        }
    }

    /// <summary>
    /// 返回路径的最后一个点
    /// </summary>
    /// <param name="startNode"></param>
    /// <returns></returns>
    private Node GeneratePath(Node startNode)
    {
        Node currentNode = startNode;
        Node nextNode = null;
        Node lNode = null, rNode = null;
        int minX, maxX, nextX;

        while (currentNode.PosY < floorNumber - 1)
        {
            lNode = GetNode(currentNode.PosX - 1, currentNode.PosY);
            rNode = GetNode(currentNode.PosX + 1, currentNode.PosY);

            minX = lNode == null ? currentNode.PosX - 1 : lNode.GetRightXConnected();
            maxX = rNode == null ? currentNode.PosX + 1 : rNode.GetLeftXConnected();

            minX = Mathf.Max(currentNode.PosX - 1, minX, 0);
            maxX = Mathf.Min(currentNode.PosX + 1, maxX, nodeNumberX - 1);

            nextX = Random.Range(minX, maxX + 1);
            nextNode = GetNode(nextX, currentNode.PosY + 1);
            if (nextNode == null)
            {
                nextNode = CreateNode(nextX, currentNode.PosY + 1);
                nodes[(nextX, currentNode.PosY + 1)] = nextNode;
            }

            currentNode.AddConnection(nextNode);

            GenerateLine(currentNode.gameObject, nextNode.gameObject);
            currentNode = nextNode;
        }

        return currentNode;
    }

    private List<Node> GenerateLayer(int y, int minNodes, int maxNodes)
    {
        List<Node> layer = new List<Node>();
        HashSet<int> usedPositions = new HashSet<int>();
        int nodeCount = Random.Range(minNodes, maxNodes + 1);

        while (layer.Count < nodeCount)
        {
            int x = Random.Range(0, nodeNumberX);
            if (!usedPositions.Contains(x))
            {
                usedPositions.Add(x);
                Node node = CreateNode(x, y);
                layer.Add(node);
            }
        }

        return layer;
    }

    private Node CreateNode(int x, int y)
    {
        // 计算格子位置
        Vector2 nodePosition = GetNodePos(x, y);

        // 实例化格子
        GameObject nodeObject = Instantiate(nodePrefab);
        nodeObject.transform.SetParent(nodeContent.transform);
        nodeObject.GetComponent<RectTransform>().anchoredPosition = nodePosition;

        // 创建 Node 实例并返回
        Node node = nodeObject.GetComponent<Node>();
        node.Init(x, y);

        nodes[(x, y)] = node;

        return node;
    }

    /// <summary>
    /// 特别地，（0，0）为起始点，（floorNumber, floorNumber）为终点
    /// </summary>
    private Node GetNode(int x, int y)
    {
        if(nodes.TryGetValue((x, y), out Node node))
        {
            return node;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 均从0开始计数
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Vector2 GetNodePos(int x, int y)
    {
        if(x < 0 || x >= nodeNumberX || y < 0 || y > floorNumber) return Vector2.zero;
        return new Vector2((x - (nodeNumberX - 1) / 2) * nodePadding, groudFloorPosY + y * nodePadding);

    }
}
