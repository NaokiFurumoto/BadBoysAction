using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

/// <summary>
/// ミサイルタイプ
/// </summary>
public enum HORMING_STATE
{
    HORMING,//ホーミング
    IDLE,//待機
}
public class EnemyHormingMove : EnemyMovement
{
    /// <summary>
    /// 状態
    /// </summary>
    [SerializeField]
    private HORMING_STATE state;

    /// <summary>
    /// 追随時間
    /// </summary>
    [SerializeField]
    private float hormingTime = 15.0f;

    /// <summary>
    /// 待機時間
    /// </summary>
    [SerializeField]
    private float idleTime = 2.0f;

    /// <summary>
    /// 待機開始時間
    /// </summary>
    private float idleStartTime;

    /// <summary>
    /// 追随開始時間
    /// </summary>
    private float hormingStartTime;

    /// <summary>
    /// 方向
    /// </summary>
    private Vector2 moveDirection;

    /// <summary>
    /// ベクトル
    /// </summary>
    private Vector2 moveVelocity;

    /// <summary>
    ///  進む方向
    /// </summary>
    Vector2 move = new Vector2(1, 0);

    /// <summary>
    /// 自分の角度
    /// </summary>
    float arot = 0;

    /// <summary>
    /// 曲がる最大角度
    /// </summary>
    [SerializeField]
    float Maxkaku = 0.05f;

    /// <summary>
    /// 曲がる角度
    /// </summary>
    private float rotation;

    protected override void Initialize()
    {
        hormingStartTime = Time.time;
        state = HORMING_STATE.HORMING;
        base.Initialize();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    protected override void TypeMove()
    {
        if (enemyMoveType != ENEMY_MOVETYPE.HORMING)
            return;

        if(state == HORMING_STATE.HORMING)
        {
            if(Time.time - hormingStartTime < hormingTime )
            {
                HormingUpdate();
                HormingMove();
            }
            else
            {
                state = HORMING_STATE.IDLE;
                idleStartTime = Time.time;
            }
        }

        if(state == HORMING_STATE.IDLE)
        {
            if (Time.time - idleStartTime < idleTime)
            {
                movePos = Vector3.zero;
            }
            else
            {
                state = HORMING_STATE.HORMING;
                hormingStartTime = Time.time;
            }
        }
      
    }

    /// <summary>
    /// ホーミング中の更新処理
    /// </summary>
    void HormingUpdate()
    {
        //移動方向
        moveDirection = new Vector2(move.x, move.y);

        //ベクトル
        moveVelocity = playerCenter.position - enemyTrans.position;

        //内積
        float dot = moveDirection.x * moveVelocity.x +
                    moveDirection.y * moveVelocity.y;
        //角度
        float angle = Acosf(dot / ((float)length(moveDirection) *
                                   (float)length(moveVelocity)));

        if (moveDirection.x * moveVelocity.y - moveDirection.y * moveVelocity.x < 0)
            angle = -angle;

        //ラジアンから角度に変換
        angle = angle * 180 / Mathf.PI;

        // 回転角度制御
        if (angle > Maxkaku)
            angle = Maxkaku;

        if (angle < -Maxkaku)
            angle = -Maxkaku;

        rotation = angle;
    }

    //-------------------------------------------------
    // オブジェクトの移動処理
    //-------------------------------------------------
    void HormingMove()
    {
        float rot = rotation; // 曲がる角度
        float tx = move.x, ty = move.y;

        move.x = tx * Mathf.Cos(rot) - ty * Mathf.Sin(rot);
        move.y = tx * Mathf.Sin(rot) + ty * Mathf.Cos(rot);

        // 移動量から角度を求める
        arot = Mathf.Atan2(move.x, move.y);
        // ラジアンから角度に
        float kaku = arot * 180.0f / Mathf.PI * -1 + 90;
        BaseMoving(move.x, move.y);
    }

    /// <summary>
    /// ベクトルの長さを求める
    /// </summary>
    /// <param name="vec">2点間のベクトル</param>
    /// <returns>ベクトルの長さを返す</returns>
    public float length(Vector2 vec)
    {
        return Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y);
    }

    /// <summary>
    /// 長さが+-1を越えたとき1に戻す処理
    /// </summary>
    /// <param name="a">内積 / ベクトルの長さの答</param>
    /// <returns></returns>
    public float Acosf(float a)
    {
        if (a < -1) a = -1;
        if (a > 1) a = 1;

        return (float)Mathf.Acos(a);
    }

    protected override void BaseMoving(float x, float y)
    {
        moveDelta = new Vector2(x * xSpeed, y * ySpeed);
        transform.Translate(moveDelta.x * Time.deltaTime,
                            moveDelta.y * Time.deltaTime , 0);
    }
}
