using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// DoTween全局监控工具：定位Tween超过2000的根源
/// 挂载到任意全局GameObject（如GameManager）即可
/// </summary>
public class DoTweenOverloadDetector : MonoBehaviour
{
    //[Header("监控配置")]
    //[Tooltip("触发报警的Tween阈值")]
    //public int tweenWarningThreshold = 2000;
    //[Tooltip("统计间隔（秒），越小越精准但略耗性能")]
    //public float checkInterval = 0.5f;
    //[Tooltip("是否记录每个Tween的创建栈（精准溯源用）")]
    //public bool recordTweenStackTrace = true;

    //// 核心统计数据
    //private float _lastCheckTime;
    //private int _lastActiveTweenCount;
    //private int _maxTweenCount = 0;

    //// 溯源数据：Tween -> 创建栈信息
    //private readonly Dictionary<Tween, TweenCreateInfo> _tweenCreateInfos = new Dictionary<Tween, TweenCreateInfo>();
    //// 统计数据：创建来源 -> 累计创建次数
    //private readonly Dictionary<string, int> _sourceCreateCount = new Dictionary<string, int>();
    //// 统计数据：目标对象 -> 绑定的Tween数
    //private readonly Dictionary<Object, int> _targetTweenCount = new Dictionary<Object, int>();

    ///// <summary>
    ///// 单个Tween的创建信息（溯源核心）
    ///// </summary>
    //private class TweenCreateInfo
    //{
    //    public string SourceScript; // 创建的脚本（含行号）
    //    public string SourceMethod; // 创建的方法名
    //    public Object TargetObj;    // Tween目标对象
    //    public float CreateTime;    // 创建时间
    //    public string TweenType;    // Tween类型（Move/Fade等）
    //}

    //private void Awake()
    //{
    //    // 注册DoTween的创建回调（核心：拦截所有Tween创建）
    //    DOTween.OnTweenCreated += OnTweenCreated;
    //    // 注册Tween销毁回调（清理溯源数据）
    //    DOTween.OnTweenKill += OnTweenKilled;
    //    DOTween.OnTweenComplete += OnTweenCompleted;

    //    Debug.Log("✅ DoTween监控工具已启动，阈值：" + tweenWarningThreshold);
    //}

    //private void Update()
    //{
    //    // 定时检查Tween数量
    //    if (Time.time - _lastCheckTime < checkInterval) return;
    //    _lastCheckTime = Time.time;

    //    // 获取DoTween全局统计
    //    int activeTweens = DOTween.TotalPlayingTweens();
    //    int totalCreated = DOTween.TotalCreatedTweens();
    //    _maxTweenCount = Mathf.Max(_maxTweenCount, activeTweens);
    //    _lastActiveTweenCount = activeTweens;

    //    // 打印基础统计（可选：关闭以减少日志）
    //    Debug.Log($"📊 DoTween统计 | 活跃数：{activeTweens} | 总创建数：{totalCreated} | 历史峰值：{_maxTweenCount}");

    //    // 超过阈值时触发深度分析
    //    if (activeTweens >= tweenWarningThreshold)
    //    {
    //        Debug.LogError($"⚠️ Tween数量超限（当前：{activeTweens} / 阈值：{tweenWarningThreshold}），开始深度分析...");
    //        AnalyzeOverloadTweens();
    //    }
    //}

    //#region 核心：拦截Tween创建/销毁，记录溯源信息
    //private void OnTweenCreated(Tween tween)
    //{
    //    if (!recordTweenStackTrace || tween == null) return;

    //    // 收集Tween创建信息
    //    var createInfo = new TweenCreateInfo
    //    {
    //        TargetObj = tween.target as Object,
    //        CreateTime = Time.time,
    //        TweenType = GetTweenTypeName(tween)
    //    };

    //    // 解析调用栈（定位具体代码行）
    //    var stackTrace = new StackTrace(true);
    //    for (int i = 0; i < stackTrace.FrameCount; i++)
    //    {
    //        var frame = stackTrace.GetFrame(i);
    //        var method = frame.GetMethod();
    //        // 跳过DoTween内部代码，只取业务代码
    //        if (method.DeclaringType?.Namespace?.StartsWith("DG.Tweening") == true)
    //            continue;

    //        // 记录业务代码的来源
    //        string fileName = frame.GetFileName() ?? "UnknownFile";
    //        int lineNumber = frame.GetFileLineNumber();
    //        createInfo.SourceScript = $"{fileName.Split('/').Last()} (行：{lineNumber})";
    //        createInfo.SourceMethod = $"{method.DeclaringType?.Name}.{method.Name}()";
    //        break;
    //    }

    //    // 保存溯源信息
    //    _tweenCreateInfos[tween] = createInfo;

    //    // 统计来源创建次数
    //    string sourceKey = $"{createInfo.SourceScript} -> {createInfo.SourceMethod}";
    //    if (_sourceCreateCount.ContainsKey(sourceKey))
    //        _sourceCreateCount[sourceKey]++;
    //    else
    //        _sourceCreateCount[sourceKey] = 1;

    //    // 统计目标对象的Tween数
    //    if (createInfo.TargetObj != null)
    //    {
    //        if (_targetTweenCount.ContainsKey(createInfo.TargetObj))
    //            _targetTweenCount[createInfo.TargetObj]++;
    //        else
    //            _targetTweenCount[createInfo.TargetObj] = 1;
    //    }
    //}

    //private void OnTweenKilled(Tween tween)
    //{
    //    CleanTweenInfo(tween);
    //}

    //private void OnTweenCompleted(Tween tween)
    //{
    //    CleanTweenInfo(tween);
    //}

    //private void CleanTweenInfo(Tween tween)
    //{
    //    if (_tweenCreateInfos.TryGetValue(tween, out var info))
    //    {
    //        // 减少目标对象的Tween计数
    //        if (info.TargetObj != null && _targetTweenCount.ContainsKey(info.TargetObj))
    //            _targetTweenCount[info.TargetObj] = Mathf.Max(0, _targetTweenCount[info.TargetObj] - 1);

    //        _tweenCreateInfos.Remove(tween);
    //    }
    //}
    //#endregion

    //#region 核心：分析超限原因，输出精准报告
    //private void AnalyzeOverloadTweens()
    //{
    //    // 1. 输出「高频创建来源TOP10」（定位最耗Tween的代码）
    //    Debug.LogError("🔍 高频创建来源TOP10（累计创建次数）：");
    //    var topSources = _sourceCreateCount.OrderByDescending(k => k.Value).Take(10);
    //    foreach (var (source, count) in topSources)
    //    {
    //        Debug.LogError($"   {count}次 | {source}");
    //    }

    //    // 2. 输出「Tween最多的目标对象TOP10」（定位问题对象）
    //    Debug.LogError("🔍 Tween最多的目标对象TOP10（当前绑定数）：");
    //    var topTargets = _targetTweenCount.OrderByDescending(k => k.Value).Take(10);
    //    foreach (var (target, count) in topTargets)
    //    {
    //        string targetName = target == null ? "Null对象" : target.name;
    //        Debug.LogError($"   {count}个 | 对象名：{targetName} | 类型：{target?.GetType().Name}");
    //    }

    //    // 3. 输出「活跃Tween类型分布」（看哪种动画占比最高）
    //    Debug.LogError("🔍 活跃Tween类型分布：");
    //    Dictionary<string, int> typeCount = new Dictionary<string, int>();
    //    foreach (var (tween, info) in _tweenCreateInfos)
    //    {
    //        if (tween.IsPlaying())
    //        {
    //            if (typeCount.ContainsKey(info.TweenType))
    //                typeCount[info.TweenType]++;
    //            else
    //                typeCount[info.TweenType] = 1;
    //        }
    //    }
    //    foreach (var (type, count) in typeCount.OrderByDescending(k => k.Value))
    //    {
    //        Debug.LogError($"   {count}个 | 类型：{type}");
    //    }

    //    // 4. 输出「长期运行的Tween」（超过5秒未结束的Tween，可能是泄漏）
    //    Debug.LogError("🔍 运行超5秒的Tween（疑似泄漏）：");
    //    var longRunningTweens = _tweenCreateInfos.Where(kvp =>
    //        kvp.Key.IsPlaying() && (Time.time - kvp.Value.CreateTime) > 5f).Take(5);
    //    foreach (var (tween, info) in longRunningTweens)
    //    {
    //        float runTime = Time.time - info.CreateTime;
    //        Debug.LogError($"   运行{runTime:F1}秒 | 来源：{info.SourceScript} | 目标：{info.TargetObj?.name}");
    //    }
    //}
    //#endregion

    //#region 辅助方法：获取Tween类型名称
    //private string GetTweenTypeName(Tween tween)
    //{
    //    if (tween is TweenerCore<Vector3, Vector3, VectorOptions>)
    //        return "Move/Position";
    //    if (tween is TweenerCore<Vector2, Vector2, VectorOptions>)
    //        return "Move2D/Position2D";
    //    if (tween is TweenerCore<float, float, FloatOptions>)
    //    {
    //        // 区分Fade/Scale/Rotate等float类型Tween
    //        if (tween.target is CanvasGroup || tween.target is SpriteRenderer || tween.target is Image)
    //            return "Fade/Alpha";
    //        if (tween.target is Transform && tween.propertyName == "localScale.x")
    //            return "Scale";
    //        if (tween.target is Transform && (tween.propertyName == "localEulerAngles.x" || tween.propertyName == "rotation.z"))
    //            return "Rotate";
    //        return "Float/Other";
    //    }
    //    if (tween is Sequence)
    //        return "Sequence（序列动画）";
    //    return "Unknown";
    //}
    //#endregion

    //#region 调试工具：手动触发分析（编辑器右键可用）
    //[ContextMenu("手动触发Tween超限分析")]
    //public void ManualAnalyzeTweens()
    //{
    //    Debug.LogError("📢 手动触发Tween分析：");
    //    AnalyzeOverloadTweens();
    //}

    //[ContextMenu("重置所有统计数据")]
    //public void ResetStats()
    //{
    //    _tweenCreateInfos.Clear();
    //    _sourceCreateCount.Clear();
    //    _targetTweenCount.Clear();
    //    _maxTweenCount = 0;
    //    Debug.Log("🔄 统计数据已重置");
    //}
    //#endregion

    //private void OnDestroy()
    //{
    //    // 取消DoTween回调，避免内存泄漏
    //    DOTween.OnTweenCreated -= OnTweenCreated;
    //    DOTween.OnTweenKill -= OnTweenKilled;
    //    DOTween.OnTweenComplete -= OnTweenCompleted;
    //}
}