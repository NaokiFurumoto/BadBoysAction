using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : MonoBehaviour
{
    /// <summary>
    /// プレイヤーと敵の最小接近距離
    /// </summary>
    public readonly static float PL_EN_DISTANCE = 0.2f;

    /// <summary>
    /// 敵が飛ばされた時の壁の接触回数
    /// </summary>
    public readonly static int WALL_DAMAGETIMES = 3;

    /// <summary>
    /// プレイヤーの攻撃力
    /// </summary>
    public readonly static float ATTACK_POWER = 20.0f;

    /// <summary>
    /// プレイヤーの最大ライフ値
    /// </summary>
    public readonly static int MAX_LIFEPOINT = 5;

    /// <summary>
    /// プレイヤーの最小ライフ値
    /// </summary>
    public readonly static int START_LIFEPOINT = 2;

    /// <summary>
    /// 敵の最大ライフ値
    /// </summary>
    public readonly static int ENMAX_LIFEPOINT = 100;

    /// <summary>
    /// プレイヤーが受けるダメージ
    /// </summary>
    public static readonly int PLAYER_DAMAGE = 1;

    /// <summary>
    /// ライフ回復値
    /// </summary>
    public readonly static int RECOVERY_LIFEPOINT = 1;

    /// <summary>
    /// シェイク時間
    /// </summary>
    public static readonly float SHAKETIME = 0.3f;

    /// <summary>
    /// シェイクの強さ
    /// </summary>
    public static readonly Vector3 SHAKESTRENGTH = new Vector3(1.2f, 1.2f, 1.2f);

    /// <summary>
    /// Stayシェイク時間
    /// </summary>
    public static readonly float STAY_SHAKETIME = 0.6f;

    /// <summary>
    /// Stay_シェイクの強さ
    /// </summary>
    public static readonly Vector3 STAY_SHAKESTRENGTH = new Vector3(0.0f, 0.2f, 0.0f);

    /// <summary>
    /// 周回半径
    /// </summary>
    public static readonly float EN_AROUND = 2.0f;

    /// <summary>
    /// ノックバック移動差分
    /// </summary>
    public static readonly float NOCKBACK_DIFF = 0.5f;

    /// <summary>
    /// ノックバック移動時間
    /// </summary>
    public static readonly float NOCKBACK_TIME = 0.3f;

}
