using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

/// <summary>
/// �~�T�C���^�C�v
/// </summary>
public enum HORMING_STATE
{
    HORMING,//�z�[�~���O
    IDLE,//�ҋ@
}
public class EnemyHormingMove : EnemyMovement
{
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    private HORMING_STATE state;

    /// <summary>
    /// �ǐ�����
    /// </summary>
    [SerializeField]
    private float hormingTime = 15.0f;

    /// <summary>
    /// �ҋ@����
    /// </summary>
    [SerializeField]
    private float idleTime = 2.0f;

    /// <summary>
    /// �ҋ@�J�n����
    /// </summary>
    private float idleStartTime;

    /// <summary>
    /// �ǐ��J�n����
    /// </summary>
    private float hormingStartTime;

    /// <summary>
    /// ����
    /// </summary>
    private Vector2 moveDirection;

    /// <summary>
    /// �x�N�g��
    /// </summary>
    private Vector2 moveVelocity;

    /// <summary>
    ///  �i�ޕ���
    /// </summary>
    Vector2 move = new Vector2(1, 0);

    /// <summary>
    /// �����̊p�x
    /// </summary>
    float arot = 0;

    /// <summary>
    /// �Ȃ���ő�p�x
    /// </summary>
    [SerializeField]
    float Maxkaku = 0.05f;

    /// <summary>
    /// �Ȃ���p�x
    /// </summary>
    private float rotation;

    protected override void Initialize()
    {
        hormingStartTime = Time.time;
        state = HORMING_STATE.HORMING;
        base.Initialize();
    }

    /// <summary>
    /// �ړ�����
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
    /// �z�[�~���O���̍X�V����
    /// </summary>
    void HormingUpdate()
    {
        //�ړ�����
        moveDirection = new Vector2(move.x, move.y);

        //�x�N�g��
        moveVelocity = playerCenter.position - enemyTrans.position;

        //����
        float dot = moveDirection.x * moveVelocity.x +
                    moveDirection.y * moveVelocity.y;
        //�p�x
        float angle = Acosf(dot / ((float)length(moveDirection) *
                                   (float)length(moveVelocity)));

        if (moveDirection.x * moveVelocity.y - moveDirection.y * moveVelocity.x < 0)
            angle = -angle;

        //���W�A������p�x�ɕϊ�
        angle = angle * 180 / Mathf.PI;

        // ��]�p�x����
        if (angle > Maxkaku)
            angle = Maxkaku;

        if (angle < -Maxkaku)
            angle = -Maxkaku;

        rotation = angle;
    }

    //-------------------------------------------------
    // �I�u�W�F�N�g�̈ړ�����
    //-------------------------------------------------
    void HormingMove()
    {
        float rot = rotation; // �Ȃ���p�x
        float tx = move.x, ty = move.y;

        move.x = tx * Mathf.Cos(rot) - ty * Mathf.Sin(rot);
        move.y = tx * Mathf.Sin(rot) + ty * Mathf.Cos(rot);

        // �ړ��ʂ���p�x�����߂�
        arot = Mathf.Atan2(move.x, move.y);
        // ���W�A������p�x��
        float kaku = arot * 180.0f / Mathf.PI * -1 + 90;
        BaseMoving(move.x, move.y);
    }

    /// <summary>
    /// �x�N�g���̒��������߂�
    /// </summary>
    /// <param name="vec">2�_�Ԃ̃x�N�g��</param>
    /// <returns>�x�N�g���̒�����Ԃ�</returns>
    public float length(Vector2 vec)
    {
        return Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y);
    }

    /// <summary>
    /// ������+-1���z�����Ƃ�1�ɖ߂�����
    /// </summary>
    /// <param name="a">���� / �x�N�g���̒����̓�</param>
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
