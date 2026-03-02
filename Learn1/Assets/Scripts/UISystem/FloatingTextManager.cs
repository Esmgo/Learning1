using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class FloatingTextManager : MonoBehaviour
{ 
    public static FloatingTextManager Instance { get; private set; }

    [SerializeField] private GameObject damageTextPrefab; // 拖入你创建的伤害数字预制体
    [SerializeField] private int poolSize = 20; // 对象池大小

    [Header("动画设置")]
    [SerializeField] private float duration = 1f; // 动画持续时间
    [SerializeField] private Vector3 moveDistance = new Vector3(0, 1, 0); // 在世界空间中向上移动的距离

    private ObjectPool textPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Init()
    {
        textPool = ObjectPoolManager.Instance.CreatePool("DamageTextPool", damageTextPrefab, poolSize);
    }

    /// <summary>
    /// 显示伤害数字
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="worldPosition">伤害发生的世界坐标</param>
    public void ShowDamageText(float damage, Vector3 worldPosition)
    {
        if (textPool == null)
        {
            Debug.LogError("伤害数字对象池未初始化！");
            return;
        }

        // 从你的对象池获取 GameObject
        GameObject textObj = textPool.GetObject();
        if (textObj == null)
        {
            Debug.LogWarning("伤害数字对象池已空！");
            return;
        }

        TextMeshPro tmp = textObj.GetComponent<TextMeshPro>();
        tmp.text = Mathf.RoundToInt(damage).ToString();

        // TextMeshPro 直接使用世界坐标
        tmp.transform.position = worldPosition;

        // 激活对象并播放动画
        //textObj.SetActive(true);
        PlayAnimation(textObj, tmp);
    }

    private void PlayAnimation(GameObject textObj, TextMeshPro tmp)
    {
        // 重置状态 - 通过 color.a 设置透明度
        Color originalColor = tmp.color;
        tmp.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        
        Vector3 originalPos = tmp.transform.position;

        // 使用DOTween Sequence来组合动画
        Sequence sequence = DOTween.Sequence();
        // 使用 transform.DOMove
        sequence.Append(tmp.transform.DOMove(originalPos + moveDistance, duration).SetEase(Ease.OutQuad));
        // DOFade 同样适用于 TextMeshPro
        sequence.Join(tmp.DOFade(0, duration).SetEase(Ease.InQuad));
        sequence.OnComplete(() =>
        {
            // 动画结束后，将 GameObject 归还到对象池
            textPool.ReturnObject(textObj);
        });
    }
}