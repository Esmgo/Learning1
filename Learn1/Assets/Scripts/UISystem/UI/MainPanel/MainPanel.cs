using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainPanel : UIPanel
{
    public override void OnOpen()
    {
        RegisterButton("StartBtn", OnStartButtonClicked);
    }

    private async void OnStartButtonClicked()
    {
        await UIManager.Instance.OpenPanelAsync<SelectCharacterPanel>("SelectCharacterPanel");
        UIManager.Instance.ClosePanel("MainPanel");
    }
}
