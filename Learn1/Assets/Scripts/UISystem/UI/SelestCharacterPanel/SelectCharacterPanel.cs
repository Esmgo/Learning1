using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SelectCharacterPanel : UIPanel
{
    [SerializeField] private Transform characterListContainer;
    [SerializeField] private GameObject characterIconPrefab;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject infoBarContainer;
    [SerializeField] private GameObject infoBarPrefab;
    [SerializeField] private List<InfoBar> infoBarList = new();


    private List<CharacterConfiguration> configs = new();
    private List<CharacterIcon> characterIconList = new();


    private CharacterConfiguration selectedConfig;

    private bool isInited = false;

    public override void OnOpen()
    {
        if (!isInited)
        {
            Init();
            isInited = true;
        }
    }

    private async void Init()
    {
        configs = await ResourceManager.Instance.LoadResourcesByLabelAsync<CharacterConfiguration>("CharacterConfiguration");
        foreach (var config in configs)
        {
            GameObject go = Instantiate(characterIconPrefab, characterListContainer);
            CharacterIcon icon = go.GetComponent<CharacterIcon>();
            characterIconList.Add(icon);
            icon.Init(config);
            icon.OnSelected += SetSelectedCharacter;
        }
        SetSelectedCharacter(configs[0]);

        RegisterButton("StartBtn", async () =>
        {
            CharacterManager.Instance.SetSelectedCharacterConfig(selectedConfig);
            //GameManager.Instance.StartGame();
            await UIManager.Instance.OpenPanelAsync<MapUIPanel>("MapUIPanel");
            UIManager.Instance.ClosePanel("SelectCharacterPanel");
        });
    }

    private void OnDestroy()
    {
        foreach (var icon in characterIconList)
        {
            icon.OnSelected -= SetSelectedCharacter;
        }
    }

    private void SetSelectedCharacter(CharacterConfiguration config)
    {
        selectedConfig = config;
        description.text = config.description;
        SetInfo(selectedConfig);
    }

    private void SetInfo(CharacterConfiguration config)
    {
        SetInfoBar(200, config.maxHealth, "生命", 0);
        SetInfoBar(200, config.maxEnergy, "能量", 1);
        SetInfoBar(20, config.damage, "伤害", 2);
        SetInfoBar(30, config.moveSpeed, "速度", 3);
    }

    private void SetInfoBar(float maxValue, float value, string name, int barNO)
    {
        if(barNO <= infoBarList.Count - 1)
        {
            infoBarList[barNO].SetInfo(maxValue, value, name);
        }
    }
}
