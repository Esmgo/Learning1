using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存档数据相关
/// </summary>
public class DataManager : MonoBehaviour
{
    public static DataManager instance { private set; get; }
    private void Awake()
    {
        if(instance == null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public PlayerData playerData { private set; get; }

    public void StartGame()
    {

    }
}

[Serializable]
class GameData
{
    public string gameSeed;

    public GameData(string gameSeed)
    {
        this.gameSeed = gameSeed;
    }
}
