using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///  Ù–‘¿‡
/// </summary>
[Serializable]
public class Stat 
{
    [SerializeField] private float baseValue;
    private float minValue;
    private float maxValue;
    private List<Modifier> modifiers = new List<Modifier>();
    public List<Modifier> Modifiers
    {
        get { return modifiers; }
    }

    public Stat(float baseValue, float minValue, float maxValue = int.MaxValue)
    {
        this.baseValue = baseValue;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    public float BaseValue { get => baseValue; set => baseValue = value; }

    public float FinalValue
    {
        get
        {
            float value = baseValue;

            var flatModifiers = modifiers.OfType<FlatModifier>();
            foreach (var mod in flatModifiers)
            {
                value += mod.value;
            }

            var percentModifiers = modifiers.OfType<PercentModifier>();
            float percentSum = 0f;
            foreach (var mod in percentModifiers)
            {
                percentSum += mod.value;
            }
            value *= (1 + percentSum);

            return Mathf.Min(Mathf.Max(value, 0), maxValue);
        }
    }

    public void AddModifier(Modifier mod)
    {
        modifiers.Add(mod);
    }

    public void RemoveModifier(Modifier mod)
    {
        modifiers.Remove(mod);
    }

    public void ClearModifiers()
    {
        modifiers.Clear();
    }

    public void ClearModifiersOfType<T>() where T : Modifier
    {
        modifiers.RemoveAll(m => m is T);
    }
}
