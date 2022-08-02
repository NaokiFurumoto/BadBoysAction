using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// �v���[���[�����莞�ԓ�����
/// </summary>
public enum ESCAPE_STATE
{
    WALK,//����
    ESCAPE,//������
    IDLE,//�ҋ@
}
public class EnemyEscapeMove : EnemyMovement
{
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    private ESCAPE_STATE state;

    /// <summary>
    /// �ҋ@����
    /// </summary>
    [SerializeField]
    private float idleTime = 1.0f;

    /// <summary>
    /// �����鎞��
    /// </summary>
    [SerializeField]
    private float EscapeTime = 10.0f;

    /// <summary>
    /// �ҋ@�J�n����
    /// </summary>
    private float idleStartTime;

    /// <summary>
    /// �ǂ������J�n����
    /// </summary>
    private float escapeStartTime;

    private Rigidbody2D rigid;

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////
    /// </summary>
    Vector2 A, C, AB, AC;
    Vector2 move = new Vector2(1, 0); // �i�ޕ���
    float speed = 5f; // �����X�s�[�h
    float arot = 0; // �����̊p�x

    float Maxkaku = 0.05f; // �Ȃ���ő�p�x
    public float rotation; // �Ȃ���p�x

    private GameObject target; // �����Ώ�


    protected override void Initialize()
    {
        base.Initialize();
        target = playerCenter.gameObject;
        rigid = this.gameObject.GetComponent<Rigidbody2D>();    
        state = ESCAPE_STATE.WALK;
    }



    /// <summary>
    /// �ړ�����
    /// </summary>
    protected override void TypeMove()
    {
        //�����̈ʒu
        A = new Vector2(enemyTrans.position.x, enemyTrans.position.y); 
        //�v���C���[�̈ʒu
        C = new Vector2(target.transform.position.x, target.transform.position.y); 

        //�ړ�����
        AB = new Vector2(move.x,move.y);
        //�^�[�Q�b�g�̃x�N�g��
        AC = C - A;

        //�Ȃ��p�����߂�
        //����
        float dot = AB.x * AC.x + AB.y * AC.y;

        // �A�[�N�R�T�C�����g���ē��ςƃx�N�g���̒�������p�x�����߂�
        float r = Acosf(dot / ((float)length(AB) * (float)length(AC)));


        // �Ȃ�����������߂�
        if (AB.x * AC.y - AB.y * AC.x < 0)
        {
            r = -r;
        }

        r = r * 180 / Mathf.PI; // ���W�A������p�x��

        // ��]�p�x����
        if (r > Maxkaku)
        {
            r = Maxkaku;
        }
        if (r < -Maxkaku)
        {
            r = -Maxkaku;
        }


        rotation = r; // �Ȃ���p�x������


        Move();


        //if (enemyMoveType != ENEMY_MOVETYPE.ESCAPE)
        //    return;

        //if (Time.time - lastFollowTime > turningTimeDelay)
        //{
        //    playerLastPos = playerCenter.position;
        //    lastFollowTime = Time.time;
        //}

        //if (state == ESCAPE_STATE.WALK)
        //{
        //    movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
        //    if (Vector3.Distance(enemyTrans.position, playerLastPos) < 3.0f)
        //    {
        //        state = ESCAPE_STATE.ESCAPE;
        //        movePos = Vector3.zero;
        //        escapeStartTime = Time.time;
        //    }
        //}

        //if (state == ESCAPE_STATE.ESCAPE)
        //{
        //   // chaseSpeed *= -2.0f;
        //}





        //BaseMoving(movePos.x, movePos.y);
    }

    /// <summary>
    /// �ړ�����
    /// </summary>
    protected  void TypeMoveer()
    {

       
        if (Time.time - lastFollowTime > turningTimeDelay)
        {
            playerLastPos = playerCenter.position;
            lastFollowTime = Time.time;
        }

        if (state == ESCAPE_STATE.WALK)
        {
            movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
            if (Vector3.Distance(enemyTrans.position, playerLastPos) < 3.0f)
            {
                state = ESCAPE_STATE.ESCAPE;
                movePos = Vector3.zero;
                escapeStartTime = Time.time;
            }
        }

        if (state == ESCAPE_STATE.ESCAPE)
        {
            // chaseSpeed *= -2.0f;
        }





        BaseMoving(movePos.x, movePos.y);
    }

    //-------------------------------------------------
    // �I�u�W�F�N�g�̈ړ�����
    //-------------------------------------------------
    void Move()
    {
        float rot = rotation; // �Ȃ���p�x

        float tx = move.x, ty = move.y;

        move.x = tx * Mathf.Cos(rot) - ty * Mathf.Sin(rot);
        move.y = tx * Mathf.Sin(rot) + ty * Mathf.Cos(rot);

        arot = Mathf.Atan2(move.x, move.y); // �ړ��ʂ���p�x�����߂�
        float kaku = arot * 180.0f / Mathf.PI *-1 + 90; // ���W�A������p�x��

        rigid.velocity = new Vector2(move.x, move.y) * speed; // �ړ�(�Ō�́[1�������Ă��鏊�������ƃv���C���[��ǂ������܂�)
        //transform.rotation = Quaternion.Euler(0, 0, kaku); // ��]

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
}
