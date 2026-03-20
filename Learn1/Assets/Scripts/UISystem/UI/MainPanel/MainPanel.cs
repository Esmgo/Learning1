using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainPanel : UIPanel
{
    private bool isInited = false;
    public override void OnOpen()
    {
        Init();
    }

    private void Init()
    {
        if(isInited) return;
        RegisterButton("StartBtn", OnStartButtonClicked);
        RegisterButton("BCP", OpenBackup);
        isInited = true;
    }

    private async void OnStartButtonClicked()
    {
        await UIManager.Instance.OpenPanelAsync<SelectCharacterPanel>("SelectCharacterPanel");
        UIManager.Instance.ClosePanel("MainPanel");
    }

    /// <summary>
    /// ≤‚ ‘”√
    /// </summary>
    private async void OpenBackup()
    {
        await UIManager.Instance.OpenPanelAsync<BackupUIPanel>("BackupUIPanel");
        UIManager.Instance.ClosePanel("MainPanel");
    }
}
