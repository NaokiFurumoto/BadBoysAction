using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

public enum AROUND_STATE
{
    NONE,//���ݒ�
    CHASE,//�܂������i��
    AROUND,//����
}
/// <summary>
/// �܂������i�ރ^�C�v�̓G�ɃA�^�b�`�FENEMY_MOVETYPE.CHASE
/// </summary>
public class EnemyAroundMove : EnemyMovement
{
    /// <summary>
    /// �ړ����
    /// </summary>
    [SerializeField]
    private AROUND_STATE state = AROUND_STATE.NONE;

    /// <summary>
    /// ��]����������
    /// </summary>
    private bool isInitializeAround;

    /// <summary>
    /// �i�ޏ���������
    /// </summary>
    private bool isInitializeChase;

    /// <summary>
    /// -1.0f�Ŏ��v���A1.0f�Ŕ����v���
    /// </summary>
    [SerializeField]
    private float direction = -1.0f;

    /// <summary>
    /// ��]�X�s�[�h
    /// </summary>
    [SerializeField]
    private float roundSpeed = 3.0f;

    /// <summary>
    /// �ǔ����\
    /// </summary>
    [SerializeField]
    private float followRate = 0.1f;

    /// <summary>
    /// �v���C���[�Ƃ̋���
    /// </summary>
    [SerializeField]
    private float followTargetDistance = 2.0f;

    /// <summary>
    /// ���������̉�]
    /// </summary>
    private Quaternion leftRotation = Quaternion.Euler(0, 180, 0);

    /// <summary>
    /// �E��������Rotation�B
    /// </summary>
    private Quaternion rightRotation = Quaternion.Euler(0, 0, 0);

    /// <summary>
    /// ���񎞊�
    /// </summary>
    [SerializeField]
    private float aroundTime = 5.0f;

    /// <summary>
    /// ����J�n����
    /// </summary>
    private float aroundStartTime;

    /// <summary>
    /// ���񒆔���
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
    /// �ړ������@TypeMove()�̌��EnemyMove(x,y)�����s
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
        else//��]����
        {
            if (!isInitializeAround)
            {
                state = AROUND_STATE.AROUND;
                aroundStartTime = Time.time;
                isInitializeAround = true;
            }

            //���񎞊Ԍo��
            if (Time.time - aroundStartTime < aroundTime)
            {
                Around();
            }
            else
            {
                //��x�������s
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
    /// �܂������i�ޏ����@:������������
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

        //�v���C���[�Ɍ������ړ��l
        movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
        BaseMoving(movePos.x, movePos.y);
    }

    /// <summary>
    /// ����
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
    /// ��{�ړ�����
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    protected override void BaseMoving(float x, float y)
    {
        base.BaseMoving(x, y);
    }
}
