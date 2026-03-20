using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemForSale : MonoBehaviour
{
    private RelicConfiguration config;
    private bool isInited = false;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private UIButton buyButton;
    public void Init(RelicConfiguration config)
    {
        gameObject.SetActive(true);
        this.config = config;
        itemIcon.sprite = config.icon;
        itemNameText.text = config.relicName;
        itemDescriptionText.text = config.description;
        itemPriceText.text = config.price.ToString();

        if (isInited) return;
        buyButton.onClick += Buy;
        isInited = true;
    }
    private void OnDestroy()
    {
        buyButton.onClick -= Buy;
    }

    private void Buy()
    {
        RelicManager.Instance.GetItem(CharacterManager.Instance.currentCharacter, config);
        gameObject.SetActive(false);
    }
}
