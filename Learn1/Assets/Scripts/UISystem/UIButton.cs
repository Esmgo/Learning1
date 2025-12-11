using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Animation))]
public class UIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("动画片段")]
    public AnimationClip onClickAnimation;
    public AnimationClip onDownAnimation;
    public AnimationClip onUpAnimation;
    public AnimationClip onEnterAnimation;
    public AnimationClip onExitAnimation;

    private Animation _animation;

    /// <summary>
    /// 单击事件
    /// </summary>
    public Action onClick;

    /// <summary>
    /// 按下事件
    /// </summary>
    public Action onDown;

    /// <summary>
    /// 抬起事件
    /// </summary>
    public Action onUp;

    /// <summary>
    /// 指针进入事件
    /// </summary>
    public Action onEnter;

    /// <summary>
    /// 指针离开事件
    /// </summary>
    public Action onExit;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _animation.playAutomatically = false;
        
        AddAndLegacyClip(onClickAnimation);
        AddAndLegacyClip(onDownAnimation);
        AddAndLegacyClip(onUpAnimation);
        AddAndLegacyClip(onEnterAnimation);
        AddAndLegacyClip(onExitAnimation);
    }

    private void AddAndLegacyClip(AnimationClip clip)
    {
        if (clip != null)
        {
            clip.legacy = true;
            _animation.AddClip(clip, clip.name);
        }
    }

    /// <summary>
    /// 播放动画，如果动画片段不为空
    /// </summary>
    private void PlayAnimation(AnimationClip clip)
    {
        // 优化：仅当动画组件和动画片段都有效时才播放
        if (_animation != null && clip != null)
        {
            _animation.Play(clip.name);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayAnimation(onClickAnimation);
        onClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayAnimation(onDownAnimation);
        onDown?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayAnimation(onEnterAnimation);
        onEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PlayAnimation(onExitAnimation);
        onExit?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayAnimation(onUpAnimation);
        onUp?.Invoke();
    }

    /// <summary>
    /// 在脚本销毁时清除所有事件监听，防止内存泄漏
    /// </summary>
    private void OnDestroy()
    {
        onClick = null;
        onDown = null;
        onUp = null;
        onEnter = null;
        onExit = null;
    }
}
