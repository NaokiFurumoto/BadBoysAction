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
    NOCKBACK,//�m�b�N�o�b�N��
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
    //[SerializeField]
    //private bool isDamage;

    /// <summary>
    /// �{��
    /// </summary>
    [SerializeField]
    private Transform body;

    /// <summary>
    /// �{��
    /// </summary>
    [SerializeField]
    private GameObject spriteBody;

    /// <summary>
    /// �G�̃^�C�v
    /// </summary>
    [SerializeField]
    private ENEMY_MOVETYPE moveType;

    /// <summary>
    /// �ړ������N���X
    /// </summary>
    private EnemyMovement enemyMovement;

    /// <summary>
    /// �F�ύX�p
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// �F�ύX�p
    /// </summary>
    private TrailRenderer trail;

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
    /// Collider2D
    /// </summary>
    private CircleCollider2D collider;

    /// <summary>
    /// UIController
    /// </summary>
    private UiController uiController;

    /// <summary>
    /// Effect
    /// </summary>
    [SerializeField]
    private GameObject effect;

    /// <summary>
    /// Shadow
    /// </summary>
    [SerializeField]
    private GameObject shadow;

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
    //public bool IsDamage
    //{
    //    get { return isDamage; }
    //    set { isDamage = value; }
    //}
    public bool HasPlayerTarget => hasPlayerTarget;
    public Rigidbody2D Rigid2D => rigid2D;
    public int Life => life;
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

        state = ENEMY_STATE.NONE;

        //isDamage = false;
        isDead = false;

        trail.enabled = false;

        //�̗͐ݒ�
        life = enemyLifeAc.SetCreateLife();
        enemyLifeAc.SetLifeText(life);

        wallDamageTimes = 0;

        //��������
        hasPlayerTarget = true;
    }

    private void InitializeComponent()
    {
        rigid2D = GetComponent<Rigidbody2D>();

        collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;

        animator = spriteBody.gameObject.GetComponent<Animator>();
        sprite = spriteBody.GetComponent<SpriteRenderer>();

        trail = spriteBody.gameObject.GetComponent<TrailRenderer>();

        uiController = GameObject.FindGameObjectWithTag("UI").
                                  GetComponent<UiController>();

        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        moveType = enemyMovement.MoveType;
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
    /// �v���C���[����U�����󂯂���
    /// </summary>
    public void PlayerDamage(Vector2 direction, float power)
    {
        sprite.color = Color.red;
        body.DOPunchScale(
            SHAKESTRENGTH,
            SHAKETIME
        ).OnComplete(() =>
        {
            collider.isTrigger = false;
            sprite.color = Color.white;
            trail.enabled = true;
            animator.SetTrigger("Damage");
            shadow.SetActive(false);

            rigid2D.AddForce(direction * power, ForceMode2D.Impulse);
        });
    }

    /// <summary>
    /// �G����U�����󂯂���
    /// </summary>
    public void EnemyDamage(int _damage)
    {
        life -= _damage;
        enemyLifeAc.SetLifeText(life);

        if (life <= 0)
        {
            state = ENEMY_STATE.DEATH;
            rigid2D.simulated = false;
            shadow.SetActive(false);

            EnemyDead();
        }
        else
        {
            if (state == ENEMY_STATE.DAMAGE)
                return;

            state = ENEMY_STATE.NOCKBACK;
            sprite.color = Color.red;

            body.DOPunchPosition(
                STAY_SHAKESTRENGTH,
                STAY_SHAKETIME
            ).OnComplete(() =>
            {
                sprite.color = Color.white;
                state = ENEMY_STATE.MOVE;
            });
        }
    }

    /// <summary>
    /// �ǂƏՓ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && state == ENEMY_STATE.DAMAGE)
        {
            wallDamageTimes++;
        }

        if (life > 0 && !isDead)
        {
            if (wallDamageTimes >= life)
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
        animator.SetTrigger("Dead");
    }

    /// <summary>
    /// ���S�A�j���[�V����������ɌĂ΂��
    /// </summary>
    public void DeadEndCallback()
    {
        //�폜�F�X�V����܂ł͎c��
        Destroy(this.gameObject);
        uiController?.SetPlayKillsNumber();
    }

    /// <summary>
    /// �G�t�F�N�g�\��
    /// </summary>
    public void PlayEffect()
    {
        effect?.SetActive(true);
    }


    #region �p�����[�^�[�ύX
    /// <summary>
    /// �_���[�W���̃X�e�[�^�X�ύX
    /// </summary>
    public void SetDamageStatus()
    {
        this.gameObject.layer = 11;
        state = ENEMY_STATE.DAMAGE;
    }

    /// <summary>
    /// ���S���̃X�e�[�^�X�ύX
    /// </summary>
    public void SetDeadStatus()
    {
        life = 0;
        isDead = true;
        state = ENEMY_STATE.DEATH;
        rigid2D.simulated = false;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); 

        enemyLifeAc.ChangeActiveLifeImage(false);
        trail.enabled = false;
    }

    //�������̗̑͐ݒ�
    public void SetCreateLife()
    {
        //���C�t��1����10�܂�
        //�^�C�v�ɂ���ă��C�t�̕ύX
        //���C�t�����X�e�[�^�X�N���X������
    }
    #endregion
}
