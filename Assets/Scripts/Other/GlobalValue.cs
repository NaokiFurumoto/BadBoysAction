using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : MonoBehaviour
{
    /// <summary>
    /// プレイヤーと敵の最小接近距離
    /// </summary>
    public readonly static float PL_EN_DISTANCE = 0.05f;

    /// <summary>
    /// 敵が飛ばされた時の壁の接触回数
    /// </summary>
    public readonly static int WALL_DAMAGETIMES = 3;

    /// <summary>
    /// プレイヤーの攻撃力
    /// </summary>
    public readonly static float ATTACK_POWER = 30.0f;

    /// <summary>
    /// プレイヤーの最大ライフ値
    /// </summary>
    public readonly static int MAX_LIFEPOINT = 5;

    /// <summary>
    /// プレイヤーの最小ライフ値
    /// </summary>
    public readonly static int START_LIFEPOINT = 1;

    /// <summary>
    /// プレイヤーが受けるダメージ
    /// </summary>
    public static readonly int PLAYER_DAMAGE = 1;

    /// <summary>
    /// ライフ回復値
    /// </summary>
    public readonly static int RECOVERY_LIFEPOINT = 1;

    /// <summary>
    /// 敵キャラシェイク時間
    /// </summary>
    public static readonly float ENEMY_SHAKETIME = 0.3f;

    /// <summary>
    /// 敵キャラシェイクの強さ
    /// </summary>
    public static readonly float ENEMY_SHAKESTRENGTH = 1.2f;

}
