using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable 
{
    /// <summary>
    /// 当对象从池中取出时调用
    /// </summary>
    void OnGetFromPool();

    /// <summary>
    /// 当对象返回池中时调用
    /// </summary>
    void OnReturnToPool();

}
