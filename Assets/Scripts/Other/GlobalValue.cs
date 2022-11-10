using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GlobalValue
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
    public readonly static float ATTACK_POWER = 30.0f;

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
    /// プレイヤー移動範囲最大X
    /// </summary>
    public static readonly float PL_MAXMOVE_X = 4;

    /// <summary>
    /// プレイヤー移動範囲最小X
    /// </summary>
    public static readonly float PL_MINMOVE_X = -4;

    /// <summary>
    /// プレイヤー移動範囲最大Y
    /// </summary>
    public static readonly float PL_MAXMOVE_Y = 7;

    /// <summary>
    /// プレイヤー移動範囲最小Y
    /// </summary>
    public static readonly float PL_MINMOVE_Y = -8;

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
    public static readonly float STAY_SHAKETIME = 1.0f;

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
    public static readonly float EN_CREATEPOS_RADIUS = 4.0f;

    /// <summary>
    /// 敵生成差分X
    /// </summary>
    public static readonly float CREATE_DIFFX = 0.4f;

    /// <summary>
    /// 敵生成差分Y
    /// </summary>
    public static readonly float CREATE_DIFFY = 0.5f;

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
    //public static float STAMINA_RECOVERY_TIME = 1800.0f;
    public static float STAMINA_RECOVERY_TIME = 3600.0f;

    /// <summary>
    /// 最大スタミナ数
    /// </summary>
    public static int STAMINA_MAXNUMBER = 3;

    /// <summary>
    /// スタート開始時間
    /// </summary>
    public static float START_PLAYINGTIME = 2.0f;

    /// <summary>
    /// Fade時間
    /// </summary>
    public static float FADETIME = 2.0f;

    /// <summary>
    /// 初期Scale
    /// </summary>
    public static Vector3 INIT_SCALE = new Vector3(1.0f,1.0f,1.0f);

    /// <summary>
    /// サウンドグループ
    /// </summary>
    public static string SOUNDGROUP_NID = "SoundGroup_";

    /// <summary>
    /// カメラ初期サイズ
    /// </summary>
    public static float CAMERA_INITSIZE = 8.5f;

    /// <summary>
    /// カメラ初期位置
    /// </summary>
    public static Vector3 CAMERA_INITPOS = new Vector3(0.0f, 0.0f, -10.0f);

    /// <summary>
    /// 汎用ダイアログのパス
    /// </summary>
    public static string COMMONDIALOG_PREFAB_NAME = "Prefabs/Common/CommonDialog";

    /// <summary>
    /// スタミナ回復ダイアログのパス
    /// </summary>
    public static string STAMINADIALOG_PREFAB_NAME = "Prefabs/Common/StaminaRecoveryDialog";

    public readonly static string ISO_8601_FORMAT     = "yyyy-MM-ddTHH:mm:ss.fffZ";

    public readonly static float STOP_TIME     = Mathf.Infinity;

    /// <summary>
    /// 敵の生成位置の差分
    /// </summary>
    public readonly static float ENEMY_CREATE_DIFF_MAX = 5.0f;
    public readonly static float ENEMY_CREATE_DIFF_MIN = 1.0f;

    /// <summary>
    /// 開始時の敵生成遅延時間
    /// </summary>
    public readonly static int START_CREATE_DIFF = 1000;

    /// <summary>
    /// 画面に表示させる最大数
    /// </summary>
    public readonly static int ENEMY_SCREEN_MAXCOUNT = 100;

    /// <summary>
    /// レベルアップ数
    /// </summary>
    public readonly static int LEVELUP_COUNT = 50;

    /// <summary>
    /// 最大ゲームレベル
    /// </summary>
    public readonly static int MAX_GAMELEVEL = 100;


    //敵キャラの移動スピード調整：100体撃破毎
    //Chase：最小0.5最大2　：増加0.1
    //ZigZag：最小0.5最大2　：増加0.1
    //Around:最小0.5最大3：0.1　回転時間2-5：ランダム　スピード2/10：0.2
}
