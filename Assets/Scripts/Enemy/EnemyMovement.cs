using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// �G�ړ��N���X
/// </summary>

///�ړ��^�C�v
public enum ENEMY_MOVETYPE
{
    NONE,   //���ݒ�
    CHASE,  //�܂�����������
    AROUND, //����
    ZIGZAG,//�W�O�U�O
    RAPIDED,//�}�ڋ�
    //ESCAPE,
    HORMING,//�z�[�~���O
}
public class EnemyMovement : MonoBehaviour
{
    #region �ϐ�
    /// <summary>
    /// �ړ��X�s�[�h
    /// </summary>
    [SerializeField]
    protected float xSpeed = 1.5f, ySpeed = 1.5f;

    /// <summary>
    /// �ړ���
    /// </summary>
    protected Vector2 moveDelta;

    /// <summary>
    /// �v���C���[���
    /// </summary>
    protected GameObject player;
    protected Transform playerCenter;
    protected Vector3 playerLastPos;
        
    /// <summary>
    /// ���g�̏��
    /// </summary>
    protected Vector3 startPos, movePos;

    /// <summary>
    /// �ǂ�������X�s�[�h
    /// </summary>
    [SerializeField]
    protected float chaseSpeed = 0.8f;

    /// <summary>
    /// ��]�̒x��
    /// </summary>
    [SerializeField]
    protected float turningDelay = 1;

    /// <summary>
    /// ���ɕ����]���\�Ȏ���
    /// </summary>
    [SerializeField]
    protected float turningTimeDelay = 1f;

    /// <summary>
    /// �v���C���[�̈ʒu���Ō�ɔc����������
    /// </summary>
    protected float lastFollowTime;

    /// <summary>
    /// �����ۑ��p
    /// </summary>
    protected Vector3 tempScale;

    /// <summary>
    /// �G�̃A�j���[�^�[
    /// </summary>
    [SerializeField]
    protected  Animator animator;

    /// <summary>
    /// �{�́F�����ύX�p
    /// </summary>
    [SerializeField]
    protected  Transform body;

    /// <summary>
    /// �G�̃X�e�[�^�X�Ǘ��N���X
    /// </summary>
    protected  EnemyStatusController enemyStatusController;

    /// <summary>
    /// �G�̍U���N���X
    /// </summary>
    protected  EnemyAttack enemyAttack;

    /// <summary>
    /// �G�̍U���N���X
    /// </summary>
    protected Transform enemyTrans;

    /// <summary>
    /// �ړ��^�C�v
    /// </summary>
    [SerializeField]
    protected  ENEMY_MOVETYPE enemyMoveType;
    #endregion

    #region �v���p�e�B
    public Vector2 MoveDelta => moveDelta;
    public ENEMY_MOVETYPE MoveType => enemyMoveType;
    #endregion

    protected void Start()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    protected virtual void Initialize()
    {
        player                = GameObject.FindWithTag("Player").gameObject;
        playerCenter          = player?.GetComponent<PlayerStatusController>().PlayerCenter;
        playerLastPos         = playerCenter.position;
        startPos              = transform.position;
        lastFollowTime        = Time.time;
        turningTimeDelay     *= turningDelay;
        enemyTrans            = transform;
        enemyStatusController = GetComponent<EnemyStatusController>();
        enemyAttack           = GetComponent<EnemyAttack>();

        if (animator == null)
        {
            animator = GameObject.FindGameObjectWithTag("EnemyAnimator").
                                  GetComponent<Animator>();
        }
    }

    /// <summary>
    /// �ړ��X�V
    /// </summary>
    protected void FixedUpdate()
    {
        //���g�����S��Ԃ��A�_���[�W���󂯂Ă�����s���Ȃ�
        if (enemyStatusController.IsDead
            || enemyStatusController.State == ENEMY_STATE.DAMAGE
            || enemyStatusController.State == ENEMY_STATE.NOCKBACK)
        {
            return;
        }

        MoveAnimation();
        TurnAround();

        //���ʏ���
        if(enemyStatusController.HasPlayerTarget)
        {
            if (!enemyAttack.IsAttacked)
            {
                //�N���X��Ŏ��s
                TypeMove();
            }
            else//�U�������ꍇ
            {
                //�N�[���_�E����
                if (enemyAttack.IsDamageCoolDown)
                {
                    //���炩�̏���
                }
                else//�N�[���_�E�����o�߂���
                {
                    //�U�����Ă��Ȃ����̂Ƃ���
                    enemyAttack.IsAttacked = false;
                }
            }
        }
        else
        {
            //�������Ă��Ȃ��ꍇ�̏���
        }
    }

    
    protected virtual void TypeMove() { }


    protected virtual void BaseMoving(float x, float y)
    {
        if (enemyStatusController.IsDead
           || enemyStatusController.State == ENEMY_STATE.DAMAGE
           || enemyStatusController.State == ENEMY_STATE.NOCKBACK)
        {
            return;
        }
        moveDelta = new Vector2(x * xSpeed, y * ySpeed);
        transform.Translate(moveDelta.x * Time.deltaTime,
                            moveDelta.y * Time.deltaTime, 0);
    }

    /// <summary>
    /// �����̕ύX
    /// </summary>
    protected void TurnAround()
    {
        tempScale = body.localScale;
        if (enemyStatusController.HasPlayerTarget)
        {
            if (playerCenter.position.x > enemyTrans.position.x)
            {
                tempScale.x = Mathf.Abs(tempScale.x);
            }

            if (playerCenter.position.x < enemyTrans.position.x)
            {
                tempScale.x = -Mathf.Abs(tempScale.x);
            }
            else//�T�m�ł��Ă��Ȃ��ꍇ�͐������ꂽ�ʒu�ɂ���Č�����ύX
            {
                if (startPos.x > enemyTrans.position.x)
                {
                    tempScale.x = Mathf.Abs(tempScale.x);
                }

                if (startPos.x < enemyTrans.position.x)
                {
                    tempScale.x = -Mathf.Abs(tempScale.x);
                }
            }
        }
        //��L�ݒ��ɔ��f
        body.localScale = tempScale;
    }

    /// <summary>
    /// �ړ��A�j���[�V�����̐ؑ�:�ҋ@������
    /// </summary>
    protected void MoveAnimation()
    {
        //�ړ����Ȃ�ΌĂ΂Ȃ��p�ɂ���
        if (enemyStatusController?.State == ENEMY_STATE.MOVE)
            return;
       
        if(moveDelta.sqrMagnitude > 0)
        {
            animator.SetBool("Walk", true);
            enemyStatusController.SetEnemyState(ENEMY_STATE.MOVE);
        }
        else
        {
            animator.SetBool("Walk", false);
            enemyStatusController.SetEnemyState(ENEMY_STATE.IDLE);
        }
    }
}
