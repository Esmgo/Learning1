using UnityEditor;
using UnityEngine;

public class WeaponRotate : MonoBehaviour
{
    [Header("旋转参数")]
    public Transform rotationCenter; // 旋转中心（可拖拽角色Transform或自定义点）
    public float fixAngle = 0f;      // 偏移角度修正

    [Header("平滑设置")]
    public bool useSmoothing = true;
    public float smoothSpeed = 10f;

    private float currentAngle = 0f; // 当前映射角度，用于平滑过渡

    void Awake()
    {
        if (rotationCenter == null)
            rotationCenter = transform;
    }

    void Update()
    {
        if (rotationCenter == null) return;

        float targetAngle = Tool.GetMouseAngle(transform);

        //平滑过渡到目标角度
        if (useSmoothing)
        {
            currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, smoothSpeed * Time.deltaTime);
        }
        else
        {
            currentAngle = targetAngle;
        }

        // 应用旋转
        transform.position = rotationCenter.position; // 保证武器始终围绕旋转中心
        transform.rotation = Quaternion.Euler(0, 0, currentAngle - fixAngle);
    }
}