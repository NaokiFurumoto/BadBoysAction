using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

/// <summary>
/// とりあえずジグザグに移動
/// </summary>
public class EnemyZigZagMove : EnemyMovement
{

    private float add_x;                       //三角関数の数値設定
    private float speed = 10f;              //スピードの数値
    private float radius =0.2f;           //半径の設定

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    protected override void TypeMove()
    {
        if (enemyMoveType != ENEMY_MOVETYPE.ZIGZAG)
            return;

        playerLastPos = playerCenter.position;

        if (Vector3.Distance(enemyTrans.position, playerLastPos) > PL_EN_DISTANCE)
        {
            movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
        }
        else//十分近づいてる
        {
            movePos = Vector3.zero;
        }

        add_x = radius * Mathf.Sin(Time.time * speed);
        BaseMoving(movePos.x + add_x, movePos.y);
    }


}
