using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 用来显示血量的脚本，要求对象上必须有一个实现了IHealth接口的组件并自行指定一个TextMeshPro组件，
/// TextMeshPro组件的文本会被设置为当前血量/最大血量的格式。
/// </summary>
public class HealthDisplayComponent : MonoBehaviour
{
    [SerializeField] private TextMeshPro tmp;
    private IHealth health;

    private void Start()
    {
        health = GetComponent<IHealth>();
        if(tmp == null )
        {
            Debug.LogError("HealthDisplayComponent requires a TextMeshPro component reference.");
        }
        if (health == null )
        {
            Debug.LogError("HealthDisplayComponent requires a component that implements IHealth.");
        }
        else
        {
            health.OnHealthChange += Display;
            Display();
        }
    }

    private void Display()
    {
        tmp.text = $"{health.CurrentHealth}/{health.MaxHealth.FinalValue}";
    }
}
