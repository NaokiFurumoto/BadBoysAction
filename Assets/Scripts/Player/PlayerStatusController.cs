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
    /// ���GRoot
    /// </summary>
    [SerializeField]
    private GameObject mutekiRoot;

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
    /// �N�[���^�C������
    /// </summary>
    private bool isCoolTimeCheck;

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

    /// <summary>
    /// �g���t�B�[���G���[�h
    /// </summary>
    private bool isMuteki;

    /// <summary>
    /// ���G���[�h
    /// </summary>
    [SerializeField]
    protected bool MUTEKI = false;


    #region �v���p�e�B
    public bool IsDead => isDead;
    public bool IsMuteki => isMuteki;

    public Transform PlayerCenter => playerCenter;

    public SpriteRenderer Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }

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

        spriteTransform = sprite?.transform;
        sprite.color = Color.red;
        isCoolTimeCheck = false;
        isDead   = false;
        isMuteki = false;

#if UNITY_EDITOR
        //���G�Ȃ��
        if (MUTEKI)
        {
            life = 1000;
        }
#endif

        lifesManager?.SetLife(life);
        startPosition = transform.position;
        attackRoot.SetActive(true);
        mutekiRoot.SetActive(false);
        //animator.SetTrigger("Play");
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
    /// ���G�J�n����
    /// �A�C�e���擾��Ă΂��
    /// </summary>
    /// <param name="isMuteki">���G����</param>
    public void MutekiAttack()
    {
        //�擾�A�C�e���폜
        OnComplate();

        if (isMuteki)
        {
            StopCoroutine("MutekiActions");
            PlayerEffectManager.Instance.DeleteSelectEffects(EFFECT_TYPE.MUTEKI);
        }

        SwitchAttackType(true);
        var effect = PlayerEffectManager.Instance.EffectPlay(EFFECT_TYPE.MUTEKI);
        StartCoroutine("MutekiActions", effect);
        isMuteki = true;
    }

    /// <summary>
    /// ���G�A�N�V������
    /// </summary>
    /// <returns></returns>
    private IEnumerator MutekiActions(GameObject effect)
    {
        yield return new WaitForSeconds(MUTEKI_TIMES);

        SwitchAttackType(false);
        PlayerEffectManager.Instance.DeleteEffect(effect);
        isMuteki = false;
    }

    /// <summary>
    /// �U�����@�ؑ�
    /// </summary>
    /// <param name="isMuteki">���G����</param>
    private void SwitchAttackType(bool isMuteki)
    {
        //�U���A�j���[�V�����̏�����
        attackerManager.SetAllAnimatorIdle();

        attackRoot.SetActive(!isMuteki);
        mutekiRoot.SetActive(isMuteki);
    }

    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        if (life <= 0 || isDead || isCoolTimeCheck)
            return;

        isCoolTimeCheck = true;
        life -= damage;
        lifesManager.SetLife(life);

        //�J�����A�N�V����
        CameraAction.PlayerDamage();

        if ( life <= 0) //���S����
        {
            isCoolTimeCheck = false;
            Dead();
        }
        else//�_���[�W����
        {
            animator.SetTrigger("Damage");

            //���A�N�V����
            spriteTransform.DOPunchScale(
                PLAYER_SHAKESTRENGTH,
                PLAYER_SHAKETIME).OnComplete(() =>
                {
                    spriteTransform.localScale = INIT_SCALE;
                    isCoolTimeCheck = false;
                });
        }
    }

    /// <summary>
    /// ���C�t��
    /// </summary>
    public void RecoveryLife()
    {
        PlayerEffectManager.Instance.EffectPlay(EFFECT_TYPE.LIFE_RECOVERY);

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

        attackerManager?.ActivateAttacker(ATTACK_DIRECTION.FRONT);

        isDead = false;
        life = 1;
        lifesManager.SetLife(life);

        attackRoot.SetActive(true);
    }
   
}
 
