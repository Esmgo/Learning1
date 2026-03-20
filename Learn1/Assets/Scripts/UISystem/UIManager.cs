using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region 单例实现
    public static UIManager Instance { get; private set; }
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

    private Dictionary<string, UIPanel> activePanels = new Dictionary<string, UIPanel>();
    [SerializeField] private Transform uiRoot;

    public async void Init()
    {
        await OpenPanelAsync<MainPanel>("MainPanel");
    }

    /// <summary>
    /// 打开面板（异步）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelName">面板名，与AA地址相同</param>
    /// <returns></returns>
    public async Task<T> OpenPanelAsync<T>(string panelName) where T : UIPanel
    {
        if (activePanels.ContainsKey(panelName))
        {
            if (!activePanels[panelName].gameObject.activeSelf)
            {
                activePanels[panelName].gameObject.SetActive(true);
                activePanels[panelName].OnOpen();
            }
            return activePanels[panelName] as T;
        }

        GameObject go = Instantiate(await ResourceManager.Instance.LoadResourceAsync<GameObject>(panelName, "UIPanel"), uiRoot);

        T panel = go.GetComponent<T>();
        if (panel == null)
            panel = go.AddComponent<T>();
        activePanels.Add(panelName, panel);
        panel.OnOpen();
        return panel;
    }

    // 关闭UI面板
    public void ClosePanel(string panelName)
    {
        if (activePanels.TryGetValue(panelName, out UIPanel panel))
        {
            panel.OnClose();
            panel.gameObject.SetActive(false);
        }
    }

    // 销毁UI面板
    public void DestroyPanel(string panelName)
    {
        if (activePanels.TryGetValue(panelName, out UIPanel panel))
        {
            Destroy(panel.gameObject);
            activePanels.Remove(panelName);
        }
    }
}
