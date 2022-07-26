using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

/// <summary>
/// �܂������i�ރ^�C�v�̓G�ɃA�^�b�`�FENEMY_MOVETYPE.CHASE
/// </summary>
public class EnemyChaseMove : EnemyMovement
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    protected override void TypeMove()
    {
        if (enemyMoveType != ENEMY_MOVETYPE.CHASE)
            return;
       
        //�܂������i�񂾔���
        if (Time.time - lastFollowTime > turningTimeDelay)
        {
            playerLastPos = playerCenter.position;
            lastFollowTime = Time.time;
        }

        if (Vector3.Distance(enemyTrans.position, playerLastPos) > PL_EN_DISTANCE)
        {
            movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
        }
        else//�\���߂Â��Ă�
        {
            movePos = Vector3.zero;
        }

        EnemyMove(movePos.x, movePos.y);
    }

    
    private void EnemyMove(float x, float y)
    {
        moveDelta = new Vector2(x * xSpeed, y * ySpeed);
        transform.Translate(moveDelta.x * Time.deltaTime,
                            moveDelta.y * Time.deltaTime, 0);
    }
}
