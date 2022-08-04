using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using DG.Tweening;
using System;

/// <summary>
/// �v���[���[�̃X�e�[�^�X�Ǘ�
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    /// <summary>
    /// ���݂̃��C�t
    /// </summary>
    [SerializeField]
    private int life;

    /// <summary>
    /// �������C�t
    /// </summary>
    [SerializeField]
    private int startLife;

    [SerializeField]
    private Transform playerCenter;

    /// <summary>
    /// �{��
    /// </summary>
    [SerializeField]
    private Transform body;

    /// <summary>
    /// �F�ύX�p
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// �A�j���[�V����
    /// </summary>
    private Animator animator;

    /// <summary>
    /// ���S����
    /// </summary>
    private bool isDead;

    /// <summary>
    /// Life�Ǘ�
    /// </summary>
    private LifesManager lifesManager;

    #region �v���p�e�B
    public bool IsDead => isDead;

    public Transform PlayerCenter => playerCenter;
    #endregion

    #region �R�[���o�b�N
    public Action OnComplate;
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
        sprite   = body.GetComponent<SpriteRenderer>();
        animator = body.gameObject.GetComponent<Animator>();
        lifesManager = GameObject.FindGameObjectWithTag("LifesRoot").
                                  GetComponent<LifesManager>();
        //life     = startLife 
        //         = START_LIFEPOINT;
        isDead   = false;
        
        lifesManager?.SetLife(life);
    }

    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        if (life <= 0 || isDead)
            return;

        life -= damage;
        lifesManager.SetLife(life);

        if ( life <= 0) //���S����
        {
            isDead = true;
            //�ڐG����𖳂���
            Dead();
        }
        else//�_���[�W����
        {
            sprite.color = Color.red;

            transform.DOPunchScale(
                SHAKESTRENGTH,
                SHAKETIME).OnComplete(() =>
                {
                    sprite.color = Color.white;
                });
        }
    }

    /// <summary>
    /// ���C�t��
    /// </summary>
    public void RecoveryLife()
    {
        //�ő�l�ȏ�͉񕜂��Ȃ�
        if (life >= MAX_LIFEPOINT)
            return;

        life += RECOVERY_LIFEPOINT;

        //���C�t�A�C�R���_��
        lifesManager?.SetLife(life);

        //���C�t�A�C�R���폜
        OnComplate();
    }

    /// <summary>
    /// ���S����
    /// </summary>
    public void Dead()
    {
        //���S�A�j���[�V����
        animator.SetTrigger("Dead");
        //�X�^�~�i�����炷
        //�X�^�~�i�Ȃ����GAMEOVER
    }
   
}
 
