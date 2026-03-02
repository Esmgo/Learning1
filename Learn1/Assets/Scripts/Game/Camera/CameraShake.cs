using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("震动参数")]
    public bool enableCameraShake = true; // 是否启用相机震动

    private Tween shakeTween; // 震动动画引用

    void Awake()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public void TriggerShake(float duration = 0.1f, float strength = 0.5f, int vibrato = 10, float randomness = 90f)
    {
        if (!enableCameraShake) return;

        StopShake();
        transform.localPosition = Vector3.zero;
        shakeTween = transform.DOShakePosition(duration, strength, vibrato, randomness, false, true);
    }

    public void StopShake()
    {
        if (shakeTween != null && shakeTween.IsActive())
        {
            shakeTween.Kill();
        }
        transform.localPosition = Vector3.zero;
    }

    public bool IsShaking()
    {
        return shakeTween != null && shakeTween.IsActive();
    }

    void OnDestroy()
    {
        StopShake();
    }

    void OnDisable()
    {
        StopShake();
    }
}
