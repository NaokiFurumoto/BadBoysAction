using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using DG.Tweening;

/// <summary>
/// �v���[���[�̃X�e�[�^�X�Ǘ�
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    /// <summary>
    /// �ő僉�C�t
    /// </summary>
    [SerializeField]
    private int maxLife;

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

    #region �v���p�e�B
    public bool IsDead => isDead;

    public Transform PlayerCenter => playerCenter;
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
        life     = startLife 
                 = START_LIFEPOINT;
        isDead   = false;
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
        if( life <= 0) //���S����
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
        if (life >= maxLife)
            return;

        life += RECOVERY_LIFEPOINT;
        //���C�t�A�C�R���_��
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
 
