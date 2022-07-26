using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

/// <summary>
/// �Ƃ肠�����W�O�U�O�Ɉړ�
/// </summary>
public class EnemyZigZagMove : EnemyMovement
{

    private float add_x;                       //�O�p�֐��̐��l�ݒ�
    private float speed = 10f;              //�X�s�[�h�̐��l
    private float radius =0.2f;           //���a�̐ݒ�

    /// <summary>
    /// ������
    /// </summary>
    protected override void Initialize()
    {
        base.Initialize();
    }

    /// <summary>
    /// �ړ�����
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
        else//�\���߂Â��Ă�
        {
            movePos = Vector3.zero;
        }

        add_x = radius * Mathf.Sin(Time.time * speed);
        BaseMoving(movePos.x + add_x, movePos.y);
    }


}
