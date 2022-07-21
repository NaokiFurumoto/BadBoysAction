using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// �G�ړ��N���X
/// </summary>
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
    private Vector2 moveDelta;

    ///// <summary>
    ///// �^�[�Q�b�g�𔭌����Ă邩�̔���
    ///// </summary>
    //private bool hasPlayerTarget;

    /// <summary>
    /// �v���C���[���
    /// </summary>
    private GameObject player;
    private Transform playerCenter;
    private Vector3 playerLastPos;
        
    /// <summary>
    /// ���g�̏��
    /// </summary>
    private Vector3 startPos, movePos;

    /// <summary>
    /// �ǂ�������X�s�[�h
    /// </summary>
    [SerializeField]
    private float chaseSpeed = 0.8f;

    /// <summary>
    /// ��]�̒x��
    /// </summary>
    [SerializeField]
    private float turningDelay = 1f;

    /// <summary>
    /// ���ɕ����]���\�Ȏ���
    /// </summary>
    [SerializeField]
    private float turningTimeDelay = 1f;

    /// <summary>
    /// �v���C���[�̈ʒu���Ō�ɔc����������
    /// </summary>
    private float lastFollowTime;

    /// <summary>
    /// �����ۑ��p
    /// </summary>
    private Vector3 tempScale;

    /// <summary>
    /// �G�̃A�j���[�^�[
    /// </summary>
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// �{�́F�����ύX�p
    /// </summary>
    private Transform body;

    /// <summary>
    /// �G�̃X�e�[�^�X�Ǘ��N���X
    /// </summary>
    private EnemyStatusController enemyStatusController;

    /// <summary>
    /// �G�̍U���N���X
    /// </summary>
    private EnemyAttack enemyAttack;
    #endregion

    #region �v���p�e�B
    public Vector2    MoveDelta       => moveDelta;
    #endregion

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        player                = GameObject.FindWithTag("Player").gameObject;
        playerCenter          = player?.GetComponent<PlayerStatusController>().PlayerCenter;
        playerLastPos         = playerCenter.position;
        startPos              = transform.position;
        lastFollowTime        = Time.time;
        turningTimeDelay     *= turningDelay;
        enemyStatusController = GetComponent<EnemyStatusController>();
        enemyAttack           = GetComponent<EnemyAttack>();
        body                  = animator.gameObject.transform;

        if (animator == null)
        {
            animator = GameObject.FindGameObjectWithTag("EnemyAnimator").
                                  GetComponent<Animator>();
        }
    }

    /// <summary>
    /// �ړ��X�V
    /// </summary>
    private void FixedUpdate()
    {
        //���g�����S��Ԃ��A�_���[�W���󂯂Ă�����s���Ȃ�
        if (enemyStatusController.IsDead || enemyStatusController.IsDamage)
        {
            return;
        }

        MoveAnimation();
        TurnAround();
        ChaseingPlayer();
    }

    //�v���C���[��ǂ�������
    private void ChaseingPlayer()
    {
        //�v���[���[�𔭌�
        if (enemyStatusController.HasPlayerTarget)
        {
            //�U�����Ă��Ȃ��ꍇ
            if (!enemyAttack.IsAttacked)
            {
                //�ǂ�������
                Chase();
            }
            else//�U�������ꍇ
            {
                //�N�[���_�E���o�߂��Ă��Ȃ�
                if (!enemyAttack.IsDamageCoolDown)
                {
                    //�v���C���[���痣���H
                    //�m�b�N�o�b�N�H�H
                }
                else//�N�[���_�E�����o�߂���
                {
                    //�U�����Ă��Ȃ����̂Ƃ���
                    enemyAttack.IsAttacked = false;
                }
            }
        }
        else//�������Ă��Ȃ��ꍇ
        {
            //��������̏���
        }

        //�ړ�������
        EnemyMove(movePos.x, movePos.y);
    }

    /// <summary>
    /// �������ɒǂ�������
    /// </summary>
    private void Chase()
    {
        //�܂������i�񂾔���
        if(Time.time - lastFollowTime > turningTimeDelay)
        {
            playerLastPos = player.transform.position;
            lastFollowTime = Time.time;
        }

        //�\������Ă�FTODO�@0.15�����ǂ�����H
        if (Vector3.Distance(transform.position, playerLastPos) > PL_EN_DISTANCE)
        {
            movePos = (playerLastPos - transform.position).normalized * chaseSpeed;
        }
        else//�\���߂Â��Ă�
        {
            //�ړ���~�F�U���H�H
            movePos = Vector3.zero;
        }
    }


    /// <summary>
    /// �G���ړ�������
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    protected virtual void EnemyMove(float x, float y)
    {
        moveDelta = new Vector2(x * xSpeed, y * ySpeed);
        transform.Translate(moveDelta.x * Time.deltaTime,
                            moveDelta.y * Time.deltaTime, 0);
    }

    /// <summary>
    /// �����̕ύX
    /// </summary>
    private void TurnAround()
    {
        tempScale = body.localScale;
        if (enemyStatusController.HasPlayerTarget)
        {
            if (playerCenter.position.x > transform.position.x)
            {
                tempScale.x = Mathf.Abs(tempScale.x);
            }

            if (playerCenter.position.x < transform.position.x)
            {
                tempScale.x = -Mathf.Abs(tempScale.x);
            }
            else//�T�m�ł��Ă��Ȃ��ꍇ�͐������ꂽ�ʒu�ɂ���Č�����ύX
            {
                if (startPos.x > transform.position.x)
                {
                    tempScale.x = Mathf.Abs(tempScale.x);
                }

                if (startPos.x < transform.position.x)
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
    private void MoveAnimation()
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
