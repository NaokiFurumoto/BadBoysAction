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
    public readonly static int WALL_DAMAGETIMES = 4;

    /// <summary>
    /// プレイヤーの攻撃力
    /// </summary>
    public readonly static float ATTACK_POWER = 20.0f;

    /// <summary>
    /// プレイヤーの最大ライフ値
    /// </summary>
    public readonly static int MAX_LIFEPOINT = 10;

    /// <summary>
    /// プレイヤーの最小ライフ値
    /// </summary>
    public readonly static int START_LIFEPOINT = 1;

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
    public static readonly float SHAKETIME = 0.4f;

    /// <summary>
    /// シェイクの強さ
    /// </summary>
    public static readonly Vector3 SHAKESTRENGTH = new Vector3(1.2f, 1.2f, 1.2f);

    /// <summary>
    /// Stayシェイク時間
    /// </summary>
    public static readonly float STAY_SHAKETIME = 0.7f;

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

    /// <summary>
    /// 敵生成半径
    /// </summary>
    public static readonly float EN_CREATEPOS_RADIUS = 2.0f;

    /// <summary>
    /// 敵生成差分X
    /// </summary>
    public static readonly float CREATE_DIFFX = 0.4f;

    /// <summary>
    /// 敵生成差分Y
    /// </summary>
    public static readonly float CREATE_DIFFY = 0.5f;

    //最小生成時間：最大生成時間

    /// </summary>
    /// 1つの生成器で表示される最大数
    /// <summary>
    public static readonly int MAX_GE_CREATECOUNT = 30;

    /// <summary>
    /// ドロップアイテム配置位置最小X
    /// </summary>
    public static readonly float DROPITEM_POSX_MIN = -2.5f;

    /// <summary>
    /// ドロップアイテム配置位置最大X
    /// </summary>
    public static readonly float DROPITEM_POSX_MAX = 2.5f;

    /// <summary>
    /// ドロップアイテム配置位置最小Y
    /// </summary>
    public static readonly float DROPITEM_POSY_MIN = -4.2f;

    /// <summary>
    /// ドロップアイテム配置位置最大Y
    /// </summary>
    public static readonly float DROPITEM_POSY_MAX = 4.2f;

    /// <summary>
    /// スタミナ回復時間
    /// </summary>
    public static float STAMINA_RECOVERY_TIME = 300.0f;

    /// <summary>
    /// 最大スタミナ数
    /// </summary>
    public static int STAMINA_MAXNUMBER = 3;

    /// <summary>
    /// スタート開始時間
    /// </summary>
    public static float START_PLAYINGTIME = 2.0f;

    //敵キャラの移動スピード調整：100体撃破毎
    //Chase：最小0.5最大2　：増加0.1
    //ZigZag：最小0.5最大2　：増加0.1
    //Around:最小0.5最大3：0.1　回転時間2-5：ランダム　スピード2/10：0.2
}
