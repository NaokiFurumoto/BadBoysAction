using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GlobalValue;
/// <summary>
/// �G�U���N���X
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    /// <summary>
    /// �U����
    /// </summary>
    [SerializeField]
    private int damageAmount;

    /// <summary>
    /// �U������
    /// </summary>
    [SerializeField]
    private bool isAttacked;

    /// <summary>
    /// �N�[���_�E������
    /// </summary>
    [SerializeField]
    private float damageCoolDown;

    /// <summary>
    /// �N�[���_�E���v���p
    /// </summary>
    private float damageCoolDownTimer;

    /// <summary>
    /// �X�e�[�^�X����
    /// </summary>
    private EnemyStatusController enemyStatusController;

    /// <summary>
    /// �ړ�����
    /// </summary>
    private EnemyMovement enemyMovement;

    /// <summary>
    /// ���g��Transform
    /// </summary>
    private Transform trans;


    private void Start()
    {
        Initialize();   
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        enemyStatusController = GetComponent<EnemyStatusController>();
        enemyMovement         = GetComponent<EnemyMovement>();
        isAttacked            = false;
        damageCoolDownTimer   = 0;
        trans                 = this.gameObject.transform;
    }

    #region �v���p�e�B
    public bool IsAttacked { get { return isAttacked; } 
                             set { isAttacked = value; } }
    /// <summary>
    /// �N�[���_�E��������
    /// </summary>
    public bool IsDamageCoolDown => Time.time < damageCoolDownTimer;
    #endregion

    /// <summary>
    /// �ڐG���̏���
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���[���[�̏ꍇ
        if (collision.CompareTag("Player"))
        {
            //�_���[�W��
            if (enemyStatusController.IsDamage)
                return;

            //�N�[���_�E����
            if (IsDamageCoolDown)
                return;

            if (!isAttacked)
            {
                //�N�[���_�E�����͍U��������Ȃ�
                damageCoolDownTimer = Time.time + damageCoolDown;
                isAttacked = true;
                collision.GetComponent<PlayerStatusController>().Damage(damageAmount);

                //�m�b�N�o�b�N
                NockBack();

                return;
            }
        }//�G�̏ꍇ
        else if (collision.CompareTag("Enemy"))
        {
            var enemyCtrl = collision.gameObject?.
                            GetComponent<EnemyStatusController>();
            if (enemyCtrl.State == ENEMY_STATE.DEATH)
                return;

            if(enemyStatusController.State == ENEMY_STATE.DAMAGE)
            {
                enemyCtrl.EnemyDamage();
            }
        }
    }

    /// <summary>
    /// �m�b�N�o�b�N
    /// </summary>
    private void NockBack()
    {
        var nockbackPos = new Vector3();
        var delta = -(enemyMovement.MoveDelta);
        var distance = new Vector3(Mathf.Sign(delta.x), Mathf.Sign(delta.y), 0);

        nockbackPos.x = trans.position.x + (NOCKBACK_DIFF * distance.x);
        nockbackPos.y = trans.position.y + (NOCKBACK_DIFF * distance.y);

        this.transform.DOMove(nockbackPos, NOCKBACK_TIME);
    }
}
