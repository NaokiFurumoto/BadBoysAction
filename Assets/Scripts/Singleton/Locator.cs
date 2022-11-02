using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Singletonよりも使いやすいサービスを提供する基底クラス
/// Interfaceと組み合わせると強力
/// </summary>

public static class Locator<T> where T : class
{
    public static T Instance { get; private set; }

    /// <summary>
    /// nullチェック
    /// </summary>
    /// <returns></returns>
    public static bool IsValid() => Instance != null;

    /// <summary>
    /// 外部から設定する
    /// Locator<クラス名>.Bind(this)
    /// Locator<クラス名>.Bind(new クラス名)
    /// </summary>
    /// <param name="instance"></param>
    public static void Bind(T instance) 
    {
        Instance = instance;
    }

    /// <summary>
    /// クリア：判定
    /// </summary>
    /// <param name="instance"></param>
    public static void Unbind(T instance)
    {
        if(Instance == instance)
        {
            Instance = null;
        }
    }

    /// <summary>
    /// クリア:強制
    /// </summary>
    public static void Clear()
    {
        Instance = null;
    }


}
