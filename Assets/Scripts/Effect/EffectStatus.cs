using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�t�F�N�g�̎��
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
    /// ���
    /// </summary>
    [SerializeField]
    private EFFECT_TYPE effectType = EFFECT_TYPE.NONE;

    /// <summary>
    /// �����e�w��
    /// </summary>
    [SerializeField]
    private PEFFECT_ROOT_TYPE rootType = PEFFECT_ROOT_TYPE.NONE;

    /// <summary>
    /// �����ʒu
    /// </summary>
    [SerializeField]
    private Vector3 pos = Vector3.zero;

    #region �v���p�e�B
    public EFFECT_TYPE EffectType => effectType;
    public PEFFECT_ROOT_TYPE RootType => rootType;
    public Vector3 Pos => pos;
    #endregion
}

interface IPlayerEffect  { }
