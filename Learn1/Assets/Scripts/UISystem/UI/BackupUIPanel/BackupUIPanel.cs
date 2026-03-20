using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BackupUIPanel : UIPanel
{
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private RectTransform gridsStartPoint;
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject content;
    [SerializeField] private List<RelicConfiguration> relics = new();

    [SerializeField] private int gridCount = 14;

    private bool isInit = false;
    private List<List<Grid>> grids = new();

    private RelicPrefab currentPrefab;
    private Grid currentGrid;

    public override void OnOpen()
    {
        if(!isInit)
        {
            Init();
            isInit = true;
        }
    }

    private async void Init()
    {
        for(int i = 0; i < gridCount; i++)
        {
            List<Grid> list = new List<Grid>();
            for (int j = 0; j < gridCount; j++)
            {
                var grid = Instantiate(gridPrefab, gridParent);
                var rect = grid.GetComponent<RectTransform>();
                rect.anchoredPosition = gridsStartPoint.anchoredPosition + new Vector2(i * rect.sizeDelta.x, j * rect.sizeDelta.y);
                
                Grid g = grid.GetComponent<Grid>();
                g.Init(i, j);
                g.OnMouseEnter += OnGridMouseEnter;

                list.Add(g);
            }
            grids.Add(list);
        }

        foreach(var r in relics)
        {
            GameObject handle = await ResourceManager.Instance.LoadResourceAsync<GameObject>(r.prefabPath, "RelicPrefab");
            if(handle != null){
                var relicObj = Instantiate(handle, content.transform);
                RelicPrefab rf = relicObj.GetComponent<RelicPrefab>();
                rf.Init(r);
                rf.OnDrag += OnStartDrag;
                rf.OnMouseUp += OnRelicMouseUp;
            }
        }
    }

    private void OnStartDrag(RelicPrefab relic)
    {
        Vector2Int pos = relic.GetPosition();
        if (pos.x != -1)
        {
            SetGridOwns(grids[pos.x][pos.y], relic.GetShape(), null);
        }
        currentPrefab = relic;
        relic.transform.SetParent(transform);
    }

    private void OnRelicMouseUp(RelicPrefab relic)
    {
        if (currentGrid != null)
        {
            if (currentGrid.IsEmpty() && IsValidPosition(relic.GetShape(), currentGrid))
            {
                SetGridOwns(currentGrid, relic.GetShape(), relic);
                relic.SetPosition(new Vector2Int(currentGrid.x, currentGrid.y));
                relic.transform.position = currentGrid.transform.position;
            }
            else
            {
                relic.transform.SetParent(content.transform);
                relic.SetPosition(new Vector2Int(-1, -1));
            }
        }
        else
        {
            relic.transform.SetParent(content.transform);
            relic.SetPosition(new Vector2Int(-1, -1));
        }
    }

    private void OnGridMouseEnter(Grid grid)
    {
        currentGrid = grid;
    }

    private bool IsValidPosition(List<Vector2Int> shape, Grid grid)
    {
        foreach(var s in shape)
        {
            if((grid.x + s.x) < 0 || (grid.x + s.x) >= gridCount || (grid.y + s.y) < 0 || (grid.y + s.y) >= gridCount)
            {
                return false;
            }
            if (!grids[(grid.x + s.x)][(grid.y + s.y)].GetComponent<Grid>().IsEmpty())
            {
                return false;
            }
        }
        return true;
    }

    private void SetGridOwns(Grid grid, List<Vector2Int> shape, RelicPrefab relic)
    {
        foreach(var s in shape)
        {
            grids[grid.x + s.x][grid.y + s.y].GetComponent<Grid>().SetOwnedRelic(relic);
            if(relic != null)
            {
                grids[grid.x + s.x][grid.y + s.y].GetComponent<Grid>().SetColor(Color.blue);
            }
            else
            {
                grids[grid.x + s.x][grid.y + s.y].GetComponent<Grid>().SetColor();
            }
        }
    }
}
