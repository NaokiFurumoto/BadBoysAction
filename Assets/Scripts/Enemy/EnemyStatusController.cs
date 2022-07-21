using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GlobalValue;
/// <summary>
/// �G�̏�ԊǗ��N���X
/// </summary>

///���
public enum ENEMY_STATE
{
    NONE,//���ݒ�
    IDLE,//�ҋ@
    MOVE,//�ړ���
    ATTACK,//�U����
    DAMAGE,//�_���[�W���󂯂Ă�
    POOL,//Pool����Ă�
    DEATH,//���S
}
public class EnemyStatusController : MonoBehaviour
{
    /// <summary>
    /// �̗�
    /// </summary>
    [SerializeField]
    private int life = 1;

    /// <summary>
    /// EnemyLifeAction
    /// </summary>
    [SerializeField]
    private EnemyLifeAction enemyLifeAc;

    /// <summary>
    /// �ǂɓ���������
    /// </summary>
    [SerializeField]
    private int wallDamageTimes;

    /// <summary>
    /// �^�[�Q�b�g�𔭌����Ă邩�̔���
    /// </summary>
    private bool hasPlayerTarget;

    /// <summary>
    /// �G�̏��
    /// </summary>
    [SerializeField]
    private ENEMY_STATE state;

    /// <summary>
    /// �_���[�W����
    /// </summary>
    [SerializeField]
    private bool isDamage;

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
    /// RigidBody2D
    /// </summary>
    private Rigidbody2D rigid2D;

    /// <summary>
    /// RigidBody2D
    /// </summary>
    private CircleCollider2D collider;

    #region �v���p�e�B
    public ENEMY_STATE State
    {
        get { return state; }
        set { state = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    public bool IsDamage
    {
        get { return isDamage; }
        set { isDamage = value; }
    }
    public bool HasPlayerTarget => hasPlayerTarget;
    public Rigidbody2D Rigid2D => rigid2D;
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
        rigid2D     = GetComponent<Rigidbody2D>();
        collider    = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;

        animator    = body.gameObject.GetComponent<Animator>();
        sprite      = body.GetComponent<SpriteRenderer>();

        state       = ENEMY_STATE.NONE;
        isDamage    = false;
        isDead      = false;

        enemyLifeAc.SetLifeText(life);

        wallDamageTimes = 0;
        //TODO:�Ƃ肠��������
        hasPlayerTarget = true;
    }

    /// <summary>
    /// ��Ԑݒ�
    /// </summary>
    /// <param name="state"></param>
    public void SetEnemyState(ENEMY_STATE state)
    {
        this.state = state;
    }

    /// <summary>
    /// �U�����󂯂���
    /// </summary>
    public void Damage(Vector2 direction,float power)
    {
        sprite.color = Color.red;

        body.DOShakeScale(
            duration: ENEMY_SHAKETIME,
            strength: ENEMY_SHAKESTRENGTH
        ).OnComplete(() =>
        {
            sprite.color = Color.white;
            rigid2D.AddForce(direction * power, ForceMode2D.Impulse);
        });
    }

    /// <summary>
    /// �ǂƏՓ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && isDamage)
        {
            wallDamageTimes++;
        }

        if (life > 0 && !isDead)
        {
            if (wallDamageTimes >= WALL_DAMAGETIMES)
            {
                EnemyDead();
            }
        }
    }

    /// <summary>
    /// ���S����
    /// </summary>
    public void EnemyDead()
    {
        SetDeadStatus();
        //���S�A�j���[�V����
        animator.SetTrigger("Dead");
    }

    /// <summary>
    /// ���S�A�j���[�V����������ɌĂ΂��
    /// </summary>
    public void DeadEndCallback()
    {
        //��U��\��
        this.gameObject.SetActive(false);
    }


    #region �p�����[�^�[�ύX
    /// <summary>
    /// �_���[�W���̃X�e�[�^�X�ύX
    /// </summary>
    public void SetDamageStatus()
    {
        isDamage = true;
        state = ENEMY_STATE.DAMAGE;
        collider.isTrigger = false;
    }

    /// <summary>
    /// ���S���̃X�e�[�^�X�ύX
    /// </summary>
    public void SetDeadStatus()
    {
        life    = 0;
        isDead  = true;
        state   = ENEMY_STATE.DEATH;
        rigid2D.simulated = false;  
    }
    #endregion
}
