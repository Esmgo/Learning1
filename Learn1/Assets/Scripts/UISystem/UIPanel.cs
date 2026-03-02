using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public virtual void OnOpen() { }
    public virtual void OnClose() { }

    

    /// <summary>
    /// 通用注册按钮事件（同步Action）
    /// </summary>
    /// <param name="buttonName">按钮物体名</param>
    /// <param name="onClick">点击回调</param>
    public void RegisterButton(string buttonName, Action onClick)
    {
        var btnTrans = transform.Find(buttonName);
        if (btnTrans == null)
        {
            Debug.LogError($"未找到按钮: {buttonName}");
            return;
        }
        var btn = btnTrans.GetComponent<UIButton>();
        if (btn == null)
        {
            Debug.LogWarning($"物体 {buttonName} 未挂载UIButton组件,已创建");
            btn = btnTrans.AddComponent<UIButton>();
        }
        btn.onClick += onClick;
    }
}
