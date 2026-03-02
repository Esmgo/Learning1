using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region µ¥ÀýÊµÏÖ
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    private void Start()
    {
        Tool.Init();
        TimeManager.Instance.Init();
        ResourceManager.Instance.Init();
        UIManager.Instance.Init();
        ObjectPoolManager.Instance.Init();
        
        FloatingTextManager.Instance.Init();
    }

    public async void StartGame()
    {
        await CharacterManager.Instance.StartGame();
        await UIManager.Instance.OpenPanelAsync<FightUIPanel>("FightUIPanel");
        EnemyManager.instance.Init();
        EnemyManager.instance.StartSpawnEnemy();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ResourceManager.Instance.PrintLoadedResourcesStatus();
        }
    }
}
