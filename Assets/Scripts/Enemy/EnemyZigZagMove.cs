using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

/// <summary>
/// とりあえずジグザグに移動
/// </summary>
public class EnemyZigZagMove : EnemyMovement
{
    /// <summary>
    /// ジグザグ加算値
    /// </summary>
    private float add_x;
    
    /// <summary>
    /// zigzag移動スピード
    /// </summary>
    [SerializeField]
    private float zigzagSpeed = 10.0f;

    /// <summary>
    /// zigzag半径
    /// </summary>
    [SerializeField]
    private float radius =　0.2f; 

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

        add_x = radius * Mathf.Sin(Time.time * zigzagSpeed);
        BaseMoving(movePos.x + add_x, movePos.y);
    }
}
