using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

/// <summary>
/// まっすぐ進むタイプの敵にアタッチ：ENEMY_MOVETYPE.CHASE
/// </summary>
public class EnemyChaseMove : EnemyMovement
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    protected override void TypeMove()
    {
        if (enemyMoveType != ENEMY_MOVETYPE.CHASE)
            return;
       
        //まっすぐ進んだ判定
        if (Time.time - lastFollowTime > turningTimeDelay)
        {
            playerLastPos = playerCenter.position;
            lastFollowTime = Time.time;
        }

        if (Vector3.Distance(enemyTrans.position, playerLastPos) > PL_EN_DISTANCE)
        {
            movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
        }
        else//十分近づいてる
        {
            movePos = Vector3.zero;
        }

        BaseMoving(movePos.x, movePos.y);
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
