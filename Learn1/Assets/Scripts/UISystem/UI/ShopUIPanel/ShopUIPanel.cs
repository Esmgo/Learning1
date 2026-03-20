using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ShopUIPanel : UIPanel
{
    private bool isInited = false;

    private List<RelicConfiguration> items = new();
    [SerializeField] private List<ItemForSale> itemForSales = new();

    public override async void OnOpen()
    {
        items = await ResourceManager.Instance.LoadResourcesByLabelAsync<RelicConfiguration>("RelicConfiguration");
        foreach (var item in itemForSales)
        {
            item.Init(items[Tool.RandomInt(0, items.Count)]);
        }

        if (isInited) return;
        RegisterButton("ContinueBtn", async () =>
        {
            UIManager.Instance.ClosePanel("ShopUIPanel");
            await UIManager.Instance.OpenPanelAsync<FightUIPanel>("FightUIPanel");
            CharacterManager.Instance.currentCharacter.RestoreFullState();
            EnemyManager.instance.StartSpawnEnemy();
        });
        isInited = true;
    }
}
