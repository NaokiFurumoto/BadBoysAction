using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using DG.Tweening;
/// <summary>
/// 一定間隔まで進んで急接近する
/// </summary>
public enum SPEED_STATE
{
    WALK,//歩く
    ENERGY,//力をためる
    ENERGYEND,//力をためる終了
    RAPID,//急接近
    IDLE,//待機
}
public class EnemyRapidedMove : EnemyMovement
{
    /// <summary>
    /// 状態
    /// </summary>
    [SerializeField]
    private SPEED_STATE state;

    /// <summary>
    /// 待機時間
    /// </summary>
    [SerializeField]
    private float idleTime = 5.0f;

    /// <summary>
    /// 追いかける時間
    /// </summary>
    [SerializeField]
    private float rapidTime = 3.0f;

    /// <summary>
    /// 追いかける時間
    /// </summary>
    [SerializeField]
    private float rapidSpeed = 10.0f;

    /// <summary>
    /// 待機開始時間
    /// </summary>
    private float idleStartTime;

    /// <summary>
    /// 追いかけ開始時間
    /// </summary>
    private float rapidStartTime;

    /// <summary>
    /// ノックバック距離
    /// </summary>
    private float add_X = 2.0f;
    private float add_Y = 2.0f;


    protected override void Initialize()
    {
        base.Initialize();
        state = SPEED_STATE.WALK;
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    protected override void TypeMove()
    {
        if (enemyMoveType != ENEMY_MOVETYPE.RAPIDED)
            return;

        if (Time.time - lastFollowTime > turningTimeDelay)
        {
            playerLastPos = playerCenter.position;
            lastFollowTime = Time.time;
        }

        if (state == SPEED_STATE.WALK)
        {
            movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
            if (Vector3.Distance(enemyTrans.position, playerLastPos) < 3.0f)
            {
                state = SPEED_STATE.ENERGY;
                movePos = Vector3.zero;
            }
        }

        if (state == SPEED_STATE.ENERGY)
        {
            //向いてる方向と反対方向に移動
            var direction = new Vector2(playerLastPos.x - enemyTrans.position.x,
                                  playerLastPos.y - enemyTrans.position.y).normalized;
            var x = Mathf.Sign(direction.x);
            var y = Mathf.Sign(direction.y);

            add_X = add_X * x;
            add_Y = add_Y * y;

            var vec = enemyTrans.position - new Vector3(add_X, add_Y);
            this.transform.DOMove(vec, 1f)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() =>
                {
                    state = SPEED_STATE.RAPID;
                    rapidStartTime = Time.time; 
                });
            state = SPEED_STATE.ENERGYEND;
        }

        if(state == SPEED_STATE.RAPID)
        {
            movePos = (playerLastPos - enemyTrans.position).normalized * rapidSpeed;
            if (Time.time - rapidStartTime >= rapidTime)
            {
                state = SPEED_STATE.IDLE;
                idleStartTime = Time.time;  
                movePos = Vector3.zero;
            }
        }

        if (state == SPEED_STATE.IDLE)
        {
            //時間経過
            if (Time.time - idleStartTime >= idleTime)
            {
                state = SPEED_STATE.RAPID;
                rapidStartTime = Time.time;
            }
        }

        BaseMoving(movePos.x, movePos.y);
    }
}
