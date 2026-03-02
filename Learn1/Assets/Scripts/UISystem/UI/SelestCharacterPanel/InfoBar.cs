using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private TextMeshProUGUI infoName;
    [SerializeField] private TextMeshProUGUI infoValue;
    public void SetInfo(float maxValue, float value, string name)
    {
        infoName.text = name;
        infoValue.text = value.ToString();
        bar.DOFillAmount(Mathf.Min(value/maxValue,1),0.3f);
    }
}
