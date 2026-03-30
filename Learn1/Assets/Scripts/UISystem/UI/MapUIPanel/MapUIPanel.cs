using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapUIPanel : UIPanel
{
    private bool isInited = false;

    [SerializeField] private const int floorNumber = 15;
    [SerializeField] private int groudFloorPosY = -500;
    [SerializeField] private int paddingX = 100;
    [SerializeField] private int nodeNumberX = 7;
    [SerializeField] private RectTransform mapRectTransform;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject nodeContent;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private UIButton regenerate;
    [SerializeField] private int minTotalEliteFights = 3; // 整个地图的保底精英战斗数量
    [SerializeField] private int minTotalRestNodes = 5; // 全局休息点保底数量

    private float nodePadding = 0;
    
    private Dictionary<(int x, int y), Node> nodes = new();
    private int totalEliteFightsGenerated = 0; // 追踪整个地图的精英战斗总数

    private int consecutiveFightOrEvent = 0;

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
        float distance = direction.magnitude - 65; // 线的长度
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
        // 重置全局精英计数器
        totalEliteFightsGenerated = 0;

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

        // --- 新增：全局精英保底检查 ---
        EnforceGlobalEliteCount();
        EnforceGlobalRestCount();
    }

    /// <summary>
    /// 返回路径的最后一个点
    /// </summary>
    private Node GeneratePath(Node startNode)
    {
        Node currentNode = startNode;
        Node nextNode = null;
        Node lNode = null, rNode = null;
        int minX, maxX, nextX;

        startNode.SetNodeType(NodeType.Fight);
        consecutiveFightOrEvent = 1;
        
        while (currentNode.PosY < floorNumber - 1)
        {
            lNode = GetNode(currentNode.PosX - 1, currentNode.PosY);
            rNode = GetNode(currentNode.PosX + 1, currentNode.PosY);

            minX = lNode == null ? currentNode.PosX - 1 : lNode.GetRightXConnected();
            maxX = rNode == null ? currentNode.PosX + 1 : rNode.GetLeftXConnected();

            minX = Mathf.Max(currentNode.PosX - 1, minX, 0);
            maxX = Mathf.Min(currentNode.PosX + 1, maxX, nodeNumberX - 1);

            if (minX > maxX) {
                minX = Mathf.Max(0, currentNode.PosX - 1);
                maxX = Mathf.Min(nodeNumberX - 1, currentNode.PosX + 1);
            }

            nextX = Random.Range(minX, maxX + 1);
            nextNode = GetNode(nextX, currentNode.PosY + 1);

            bool isNewNode = false;
            if (nextNode == null)
            {
                nextNode = CreateNode(nextX, currentNode.PosY + 1);
                nodes[(nextX, currentNode.PosY + 1)] = nextNode;
                isNewNode = true;
            }

            currentNode.AddConnection(nextNode);
            GenerateLine(currentNode.gameObject, nextNode.gameObject);

            if (isNewNode)
            {
                NodeType newType = DetermineNodeType(nextNode, currentNode);
                nextNode.SetNodeType(newType);

                if (newType == NodeType.EliteFight)
                {
                    totalEliteFightsGenerated++; // 更新全局计数
                }

                // 更新连续计数
                if (((newType == NodeType.Fight || newType == NodeType.EliteFight)
                    && (currentNode.GetNodeType() == NodeType.Fight || currentNode.GetNodeType() == NodeType.EliteFight)
                    ) 
                    || newType == currentNode.GetNodeType())
                {
                    consecutiveFightOrEvent++;
                }
                else
                {
                    consecutiveFightOrEvent = 0;
                }
            }

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

    /// <summary>
    /// 根据规则确定新节点的类型
    /// </summary>
    private NodeType DetermineNodeType(Node newNode, Node previousNode)
    {
        int y = newNode.PosY;

        // 规则 1: 固定层类型
        if (y == 7) return NodeType.Treasure;
        if (y == floorNumber - 1) return NodeType.Rest;

        // 规则 2: 倒数第二层不能是休息
        if (y == floorNumber - 2)
        {
            return GetRandomNode(false, true, true, true, false); // 禁止精英
        }

        // 规则 3: 两个休息点或商店不能相连
        if (previousNode.GetNodeType() == NodeType.Rest)
        {
            return GetRandomNode(false, true, true, true, false);
        }
        if (previousNode.GetNodeType() == NodeType.Shop)
        {
            return GetRandomNode(true, false, true, true, false);
        }

        // 规则 4: 事件与战斗最多连续四个
        if (consecutiveFightOrEvent >= 4)
        {
            consecutiveFightOrEvent = 1;
            switch (previousNode.GetNodeType())
            {
                case NodeType.Fight:
                case NodeType.EliteFight:
                    return GetRandomNode(true, true, true, false, false);
                case NodeType.Event:
                    return GetRandomNode(true, true, false, true, false);
            }
        }

        // 默认随机规则
        return GetRandomNode(true, true, true, true, false); // 禁止精英
    }

    /// <summary>
    /// 获取一个随机节点类型
    /// </summary>
    private NodeType GetRandomNode(bool canBeRest, bool canBeShop, bool canBeEvent, bool canBeFight, bool canBeEliteFight = false)
    {
        float eventWeight = canBeEvent ? 0.3f : 0f;
        float restWeight = canBeRest ? 0.1f : 0f;
        float fightWeight = canBeFight ? 0.8f : 0f;
        float shopWeight = canBeShop ? 0.1f : 0f;

        float totalWeight = eventWeight + restWeight + fightWeight + shopWeight;
        float randomValue = Random.Range(0, totalWeight);

        if (randomValue < eventWeight)
            return NodeType.Event;
        randomValue -= eventWeight;

        if (randomValue < restWeight)
            return NodeType.Rest;

        // 这里不再生成精英
        return NodeType.Fight;
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

    /// <summary>
    /// 新增：在所有路径生成后，强制满足全局精英数量
    /// </summary>
    private void EnforceGlobalEliteCount()
    {
        int neededElites = minTotalEliteFights - totalEliteFightsGenerated;
        if (neededElites <= 0)
            return;

        int upgraded = 0;
        HashSet<Node> alreadyElite = new HashSet<Node>();
        // 记录已升级为精英的节点，防止重复

        while (upgraded < neededElites)
        {
            // 每次都重新筛选可升级节点
            List<Node> candidates = new List<Node>();
            foreach (var node in nodes.Values)
            {
                if (node.GetNodeType() != NodeType.Fight) continue;
                if (node.PosY <= 2) continue; // y=0,1,2为前三层
                if (node.PosY == 7 || node.PosY >= floorNumber - 2) continue;
                if (alreadyElite.Contains(node)) continue;
                if (CanBeElite(node))
                    candidates.Add(node);
            }

            if (candidates.Count == 0)
                break;

            // 随机选择一个节点升级
            int idx = Random.Range(0, candidates.Count);
            Node n = candidates[idx];
            n.SetNodeType(NodeType.EliteFight);
            totalEliteFightsGenerated++;
            upgraded++;
            alreadyElite.Add(n);
        }

        if (totalEliteFightsGenerated < minTotalEliteFights)
        {
            Debug.LogWarning($"无法满足全局精英保底数量！目标: {minTotalEliteFights}, 实际: {totalEliteFightsGenerated}。可能是普通战斗节点不足或分布受限。");
        }
    }

    private bool CanBeElite(Node node)
    {
        foreach (var n in node.Connections)
        {
            if (n.GetNodeType() == NodeType.EliteFight)
                return false;
        }
        foreach (var n in node.BeConnecteds)
        {
            if (n.GetNodeType() == NodeType.EliteFight)
                return false;
        }
        return true;
    }

    private void EnforceGlobalRestCount()
    {
        int currentRest = 0;
        foreach (var node in nodes.Values)
        {
            if (node.GetNodeType() == NodeType.Rest)
                currentRest++;
        }
        int neededRest = minTotalRestNodes - currentRest;
        if (neededRest <= 0) return;

        int upgraded = 0;
        HashSet<Node> alreadyRest = new HashSet<Node>();
        while (upgraded < neededRest)
        {
            List<Node> candidates = new List<Node>();
            foreach (var node in nodes.Values)
            {
                if (node.GetNodeType() != NodeType.Fight && node.GetNodeType() != NodeType.Event) continue;
                if (node.PosY == 7 || node.PosY >= floorNumber - 2) continue;
                if (alreadyRest.Contains(node)) continue;
                if (CanBeRest(node))
                    candidates.Add(node);
            }
            if (candidates.Count == 0) break;
            int idx = Random.Range(0, candidates.Count);
            Node n = candidates[idx];
            n.SetNodeType(NodeType.Rest);
            upgraded++;
            alreadyRest.Add(n);
        }
        if (upgraded < neededRest)
            Debug.LogWarning($"无法满足全局休息点保底数量！目标: {minTotalRestNodes}，实际: {currentRest + upgraded}");
    }

    private bool CanBeRest(Node node)
    {
        foreach (var n in node.Connections)
            if (n.GetNodeType() == NodeType.Rest)
                return false;
        foreach (var n in node.BeConnecteds)
            if (n.GetNodeType() == NodeType.Rest)
                return false;
        return true;
    }
}
