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
    /// �U��Root
    /// </summary>
    [SerializeField]
    private GameObject attackRoot;

    /// <summary>
    /// �F�ύX�p
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// �F�ύX�p
    /// </summary>
    private Transform spriteTransform;

    /// <summary>
    /// �A�j���[�V����
    /// </summary>
    private Animator animator;

    /// <summary>
    /// �U���N���X
    /// </summary>
    private AttackerManager  attackerManager;

    /// <summary>
    /// ���S����
    /// </summary>
    private bool isDead;

    /// <summary>
    /// Life�Ǘ�
    /// </summary>
    private LifesManager lifesManager;

    /// <summary>
    /// �Q�[���Ǘ�
    /// </summary>
    private GameController gameController;

    /// <summary>
    /// �Q�[���J�n�ʒu
    /// </summary>
    private Vector2 startPosition;

#if UNITY_EDITOR
    /// <summary>
    /// ���G���[�h
    /// </summary>
    [SerializeField]
    protected bool MUTEKI = false;
#endif

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
        InitializeComponent();

        spriteTransform = sprite.transform;
        isDead   = false;

        //���G�Ȃ��
        if (MUTEKI)
        {
            life = 1000;
        }

        lifesManager?.SetLife(life);
        startPosition = transform.position;
        attackRoot.SetActive(true);
    }

    private void InitializeComponent()
    {
        sprite = body.GetComponent<SpriteRenderer>();
        animator = body.gameObject.GetComponent<Animator>();
        lifesManager = GameObject.FindGameObjectWithTag("LifesRoot").
                                  GetComponent<LifesManager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").
                                    GetComponent<GameController>();
        attackerManager = GameObject.FindGameObjectWithTag("PlayerAttack").
                                    GetComponent<AttackerManager>();
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
            Dead();
        }
        else//�_���[�W����
        {
            //�Z���T�[�̔�\��

            //���A�N�V����
            sprite.color = Color.red;
            spriteTransform.DOPunchScale(
                SHAKESTRENGTH,
                SHAKETIME).OnComplete(() =>
                {
                    sprite.color = Color.white;
                    spriteTransform.localScale = INIT_SCALE;
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
        {
            OnComplate();
            return;
        }

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
        isDead = true;
        attackRoot.SetActive(false);
        animator.SetTrigger("Dead");
    }

    /// <summary>
    /// ���g���C����
    /// </summary>
    public void RetryPlayer()
    {
        var player = this.gameObject;
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
        player.transform.position = startPosition;
        attackerManager.ActivateAttacker(ATTACK_DIRECTION.FRONT);

        isDead = false;
        life = 1;
        lifesManager.SetLife(life);

        attackRoot.SetActive(true);
    }
   
}
 
