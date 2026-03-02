using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightUIPanel : UIPanel
{
    [SerializeField] private Image healthBuffer;
    [SerializeField] private Image healthRed;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Image energyBuffer;
    [SerializeField] private Image energyBlue;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI time;

    private bool isInited = false;
    private ICharacter currentCharacter;
    private IEnergy energyComponent;
    private IHealth healthComponent;

    private Tween healthTween;
    private Tween energyTween;


    public override void OnOpen()
    {
        if(!isInited)
        {
            Init();
            isInited = true;
        }
        SubscribeEvents();
        Debug.Log("¶©ÔÄ");
        //healthComponent.OnHealthChange += UpadateInfo;
        //energyComponent.OnEnergyChange += UpadateInfo;
        UpadateInfo();
    }

    private void Init()
    {
        currentCharacter = CharacterManager.Instance.currentCharacter;
        if(currentCharacter != null)
        {
            healthComponent = currentCharacter.gameObject.GetComponent<IHealth>();
            energyComponent = currentCharacter.gameObject.GetComponent<IEnergy>();
        }
    }

    public override void OnClose()
    {
        UnsubscribeEvents();
        Debug.Log("È¡Ïû¶©ÔÄ");
        //healthComponent.OnHealthChange -= UpadateInfo;
        //energyComponent.OnEnergyChange -= UpadateInfo;
    }

    private void SubscribeEvents()
    {
        healthComponent.OnHealthChange += UpadateInfo;
        energyComponent.OnEnergyChange += UpadateInfo;
    }

    private void UnsubscribeEvents()
    {
        healthComponent.OnHealthChange -= UpadateInfo;
        energyComponent.OnEnergyChange -= UpadateInfo;
    }

    private void OnDestroy()
    {
        healthComponent.OnHealthChange -= UpadateInfo;
        energyComponent.OnEnergyChange -= UpadateInfo;
    }

    private void Update()
    {
        time.text = EnemyManager.instance.waveTimeTimer.ToString();
    }

    private void UpadateInfo(){
        if (currentCharacter == null) return;
        float healthPercent = currentCharacter.currentHealth / currentCharacter.maxHealth.FinalValue;
        float energyPercent = currentCharacter.currentEnergy / currentCharacter.maxEnergy.FinalValue;
        healthRed.fillAmount = healthPercent;
        if(healthTween != null && healthTween.IsActive())
        {
            healthTween.Kill();
        }
        healthBuffer.DOFillAmount(healthPercent, 0.3f).SetAutoKill();
        healthText.text = $"{currentCharacter.currentHealth} / {currentCharacter.maxHealth.FinalValue}";
        energyBlue.fillAmount = energyPercent;
        if(energyTween != null && energyTween.IsActive())
        {
            energyTween.Kill();
        }
        energyBuffer.DOFillAmount(energyPercent, 0.3f).SetAutoKill();
        energyText.text = $"{currentCharacter.currentEnergy} / {currentCharacter.maxEnergy.FinalValue}";
    }
}
