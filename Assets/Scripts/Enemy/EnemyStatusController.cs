using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵の状態管理クラス
/// </summary>

///状態
public enum ENEMY_STATUS
{
    NONE,//未設定
    IDLE,//待機
    MOVE,//移動中
    ATTACK,//攻撃中
    POOL,//Poolされてる
    DEATH,//死亡
}
public class EnemyStatusController : MonoBehaviour
{
    /// <summary>
    /// 体力
    /// </summary>
    [SerializeField]
    private int Life = 1;

    /// <summary>
    /// 敵の状態
    /// </summary>
    [SerializeField]
    private ENEMY_STATUS status = ENEMY_STATUS.NONE;

    /// <summary>
    /// ダメージ判定
    /// </summary>
    [SerializeField]
    private bool isDamage;

   
}
