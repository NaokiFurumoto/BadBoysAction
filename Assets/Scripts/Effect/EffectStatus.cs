using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクトの種類
/// </summary>
public enum EFFECT_TYPE
{
    NONE,
    LIFE_RECOVERY,
    MUTEKI,
}

public enum PEFFECT_ROOT_TYPE
{
    NONE,
    UNDER,
    TOP,
    CENTER,
}
public class EffectStatus : MonoBehaviour,IPlayerEffect
{
    /// <summary>
    /// 種類
    /// </summary>
    [SerializeField]
    private EFFECT_TYPE effectType = EFFECT_TYPE.NONE;

    /// <summary>
    /// 生成親指定
    /// </summary>
    [SerializeField]
    private PEFFECT_ROOT_TYPE rootType = PEFFECT_ROOT_TYPE.NONE;

    /// <summary>
    /// 生成位置
    /// </summary>
    [SerializeField]
    private Vector3 pos = Vector3.zero;

    #region プロパティ
    public EFFECT_TYPE EffectType => effectType;
    public PEFFECT_ROOT_TYPE RootType => rootType;
    public Vector3 Pos => pos;
    #endregion
}

interface IPlayerEffect  { }
