using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardHealthModule :MonoBehaviour, IModule
{
    private ICharacter owner;
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 5;

    [SerializeField] private List<float> maxHealthMDFRBaseValue = new List<float> { 10, 50, 100, 500, 1000};

    private Stat maxHealthMDFRStat;

    private FlatModifier maxHealthFlatMDFR;

    public ICharacter Owner => owner;
    public int Level => level;
    public int MaxLevel => maxLevel;
    public string Description => $"增加{maxHealthMDFRBaseValue[level - 1]}最大生命";

    public void Init(int level)
    {
        this.level = level;

        maxHealthMDFRStat = new Stat(maxHealthMDFRBaseValue[level - 1], 0);

        maxHealthFlatMDFR = new FlatModifier(maxHealthMDFRStat.FinalValue);
    }

    public void OnInstall(ICharacter owner)
    {
        this.owner = owner;
        owner.maxHealth.AddModifier(maxHealthFlatMDFR);
    }

    public void OnUninstall()
    {
        if(owner == null) return;
        owner.maxHealth.RemoveModifier(maxHealthFlatMDFR);
    }

    public void OnUpdate()
    {
        
    }

    public void Upgrade()
    {
        level = Mathf.Min(level + 1, maxLevel);
         
        maxHealthMDFRStat.BaseValue = maxHealthMDFRBaseValue[level - 1];

        maxHealthFlatMDFR.SetValue(maxHealthMDFRStat.FinalValue);
    }
}
