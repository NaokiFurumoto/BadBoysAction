using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private int Life = 1;

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
    private CircleCollider2D collider2D;


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
    public CircleCollider2D Collider2D => collider2D;
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
        rigid2D    = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CircleCollider2D>();
        collider2D.isTrigger = true;
       // state      = ENEMY_STATE.NONE;
        isDamage   = false;
        isDead     = false;
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
    /// �_���[�W���̃X�e�[�^�X�ύX
    /// </summary>
    public void SetDamageStatus()
    {
        isDamage = true;
        state = ENEMY_STATE.DAMAGE;
        collider2D.isTrigger = false;
    }

    /// <summary>
    /// �U�����󂯂���
    /// </summary>
    public void Damage(Vector2 direction,float power)
    {
        rigid2D.AddForce(direction * power, ForceMode2D.Impulse);
    }
}
