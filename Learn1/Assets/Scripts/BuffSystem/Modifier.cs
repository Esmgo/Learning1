using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Modifier 
{
    void SetValue(float value);
}

public class FlatModifier : Modifier
{
    public float value { get; private set; }

    public FlatModifier(float value)
    {
        this.value = value;
    }
    public void SetValue(float value)
    {
        this.value = value;
    }
}

public class PercentModifier : Modifier
{
    public float value { get; private set; }
    public PercentModifier(float value)
    {
        this.value = value;
    }
    public void SetValue(float value)
    {
        this.value = value;
    }
}

public class OverrideModifier : Modifier
{
    public float value { get; private set; }
    public OverrideModifier(float value)
    {
        this.value = value;
    }
    public void SetValue(float value)
    {
        this.value = value;
    }
}