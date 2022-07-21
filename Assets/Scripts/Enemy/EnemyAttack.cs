using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        isAttacked = false;
        damageCoolDownTimer = 0;
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        
    }

    #region �v���p�e�B
    public bool IsAttacked { get { return isAttacked; } 
                             set { isAttacked = value; } }
    /// <summary>
    /// �N�[���_�E�����o�߂������ǂ���
    /// </summary>
    public bool IsDamageCoolDown => Time.time > damageCoolDownTimer;
    #endregion

    /// <summary>
    /// �v���C���[�ƐڐG���̏���
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsDamageCoolDown)
            return;

        //�v���C���[�ɐڐG:�U�����ĂȂ��ꍇ
        if (collision.CompareTag("Player") && !isAttacked)
        {
            //�N�[���_�E�����͍U��������Ȃ�
            damageCoolDownTimer = Time.time + damageCoolDown;
            isAttacked = true;
            collision.GetComponent<PlayerStatusController>().Damage(damageAmount);
            //�m�b�N�o�b�N������H
            return;
        }
    }
}
