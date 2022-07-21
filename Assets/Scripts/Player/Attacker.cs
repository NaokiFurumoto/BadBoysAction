using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// �L�����N�^�[�̍U���Ɋւ���N���X
/// </summary>

//�U������
public enum ATTACK_DIRECTION
{
    NONE,
    FRONT,      //����
    UP,         //��
    SIDE,       //��
    SIDEUP,     //�΂ߏ�
    SIDEDOWN    //�΂߉�
}

public class Attacker : MonoBehaviour
{
    /// <summary>
    /// Sprite
    /// </summary>
    [SerializeField]
    private GameObject sprite;

    /// <summary>
    /// �A�j���[�V����
    /// </summary>
    private Animator animator;

    /// <summary>
    /// �U���̕���
    /// </summary>
    [SerializeField]
    private ATTACK_DIRECTION attackType;

    /// <summary>
    /// �ړ��N���X
    /// </summary>
    private PlayerMovement playerMovement;

    #region �v���p�e�B
    /// <summary>
    /// �U�������̎擾
    /// </summary>
    public ATTACK_DIRECTION AttackType => attackType;
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
        playerMovement = GameObject.FindGameObjectWithTag("Player").
                                    GetComponent<PlayerMovement>();

        animator = sprite?.GetComponent<Animator>();
    }

    /// <summary>
    /// �Z���T�[�ɏՓ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var _enemyStatus = collision.GetComponent<EnemyStatusController>();
            var _rigid2D = _enemyStatus.Rigid2D;

            //�_���[�W���󂯂Ă��Ȃ���Ԃ��G���ړ���
            if(!_enemyStatus.IsDamage && _enemyStatus.State == ENEMY_STATE.MOVE)
            {
                animator.SetTrigger("Attack");

                _enemyStatus.SetDamageStatus();
                _enemyStatus.Damage(playerMovement.Direction.normalized, ATTACK_POWER);
            }
        }
    }

    /// <summary>
    /// �Z���T�[�ɏՓ˒�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    /// <summary>
    /// �Z���T�[���甲����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //�U�������L��������������
        }
    }
}
