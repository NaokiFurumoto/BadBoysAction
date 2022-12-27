using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

public enum AROUND_STATE
{
    NONE,//未設定
    CHASE,//まっすぐ進む
    AROUND,//周回
}
/// <summary>
/// まっすぐ進むタイプの敵にアタッチ：ENEMY_MOVETYPE.CHASE
/// </summary>
public class EnemyAroundMove : EnemyMovement
{
    /// <summary>
    /// 移動状態
    /// </summary>
    [SerializeField]
    private AROUND_STATE state = AROUND_STATE.NONE;

    /// <summary>
    /// 回転初期化判定
    /// </summary>
    private bool isInitializeAround;

    /// <summary>
    /// 進む初期化判定
    /// </summary>
    private bool isInitializeChase;

    /// <summary>
    /// -1.0fで時計回り、1.0fで反時計回り
    /// </summary>
    [SerializeField]
    private float direction = -1.0f;

    /// <summary>
    /// 回転スピード
    /// </summary>
    [SerializeField]
    private float roundSpeed = 3.0f;

    /// <summary>
    /// 追尾性能
    /// </summary>
    [SerializeField]
    private float followRate = 0.1f;

    /// <summary>
    /// プレイヤーとの距離
    /// </summary>
    [SerializeField]
    private float followTargetDistance = 2.0f;

    /// <summary>
    /// 左向き時の回転
    /// </summary>
    private Quaternion leftRotation = Quaternion.Euler(0, 180, 0);

    /// <summary>
    /// 右向き時のRotation。
    /// </summary>
    private Quaternion rightRotation = Quaternion.Euler(0, 0, 0);

    /// <summary>
    /// 周回時間
    /// </summary>
    [SerializeField]
    private float aroundTime = 5.0f;

    /// <summary>
    /// 周回開始時間
    /// </summary>
    private float aroundStartTime;

    /// <summary>
    /// 周回中判定
    /// </summary>
    //private bool isArouding;

    protected override void Initialize()
    {
        base.Initialize();
        isInitializeAround = false;
        isInitializeChase = false;
        //isArouding = false;
        state = AROUND_STATE.CHASE;
    }

    /// <summary>
    /// 移動処理　TypeMove()の後にEnemyMove(x,y)が実行
    /// </summary>
    protected override void TypeMove()
    {
        if (enemyMoveType != ENEMY_MOVETYPE.AROUND)
            return;

        if (
             state == AROUND_STATE.CHASE &&
             Vector3.Distance(enemyTrans.position, playerLastPos) > EN_AROUND &&
             !isInitializeChase
            )
        {
            Chase();
        }
        else//回転処理
        {
            if (!isInitializeAround)
            {
                state = AROUND_STATE.AROUND;
                aroundStartTime = Time.time;
                isInitializeAround = true;
            }

            //周回時間経過
            if (Time.time - aroundStartTime < aroundTime)
            {
                Around();
            }
            else
            {
                //一度だけ実行
                if (!isInitializeChase)
                {
                    playerLastPos = playerCenter.position;
                    lastFollowTime = Time.time;
                    state = AROUND_STATE.CHASE;
                    enemyTrans.rotation = rightRotation;
                    isInitializeChase = true;
                }
            }
        }

        if (isInitializeChase)
        {
            Chase();
        }

    }

    /// <summary>
    /// まっすぐ進む処理　:初期化がいる
    /// </summary>
    private void Chase()
    {
        if (state != AROUND_STATE.CHASE)
            return;

        if ((Time.time - lastFollowTime) > turningTimeDelay)
        {
            playerLastPos = playerCenter.position;
            lastFollowTime = Time.time;
        }

        //プレイヤーに向かう移動値
        movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
        BaseMoving(movePos.x, movePos.y);
    }

    /// <summary>
    /// 周回
    /// </summary>
    private void Around()
    {
        if (state != AROUND_STATE.AROUND)
            return;

        enemyTrans.position = Vector3.Lerp(enemyTrans.position, playerCenter.position +
                                  (enemyTrans.position - playerCenter.position).normalized *
                                  followTargetDistance, followRate);
        enemyTrans.RotateAround(playerCenter.position, Vector3.forward, direction * roundSpeed);

        if (playerCenter.position.x - enemyTrans.position.x < 0)
        {
            enemyTrans.rotation = leftRotation;
        }
        else if (0 < playerCenter.position.x - enemyTrans.position.x)
        {
            enemyTrans.rotation = rightRotation;
        }
    }

    /// <summary>
    /// 基本移動処理
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    protected override void BaseMoving(float x, float y)
    {
        base.BaseMoving(x, y);
    }
}
