using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    #region µ¥ÀýÊµÏÖ
    public static TimeManager Instance { get; private set; }

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

    public Action recycleTrigger;
    [SerializeField]private float recycleInterval = 5f;

    public void Init()
    {
        StartCoroutine(RecycleCoroutine());
    }

    private IEnumerator RecycleCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(recycleInterval);
            recycleTrigger?.Invoke();
        }
    }
}
