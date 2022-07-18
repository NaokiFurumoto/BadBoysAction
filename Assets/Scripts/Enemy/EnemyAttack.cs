using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵攻撃クラス
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    /// <summary>
    /// 攻撃力
    /// </summary>
    [SerializeField]
    private int damageAmount = 1;

    /// <summary>
    /// 攻撃判定
    /// </summary>
    [SerializeField]
    private bool isAttacked = false;

    /// <summary>
    /// クールダウン時間
    /// </summary>
    [SerializeField]
    private float damageCoolDown = 1f;

    /// <summary>
    /// クールダウン計測用
    /// </summary>
    private float damageCoolDownTimer;
}
