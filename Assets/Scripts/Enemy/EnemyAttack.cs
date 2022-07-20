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
    private int damageAmount = 1;

    /// <summary>
    /// �U������
    /// </summary>
    [SerializeField]
    private bool isAttacked = false;

    /// <summary>
    /// �N�[���_�E������
    /// </summary>
    [SerializeField]
    private float damageCoolDown = 1f;

    /// <summary>
    /// �N�[���_�E���v���p
    /// </summary>
    private float damageCoolDownTimer;

    #region �v���p�e�B
    public bool IsAttacked { get { return isAttacked; } 
                             set { isAttacked = value; } }
    /// <summary>
    /// �N�[���_�E�����o�߂������ǂ���
    /// </summary>
    public bool IsDamageCoolDown => Time.time > damageCoolDownTimer;
    #endregion

    /// <summary>
    /// �ڐG���̏���
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[�ɐڐG
        if (collision.CompareTag("Player") && !isAttacked)
        {
            //�U�����󂯂ĂȂ����
            damageCoolDownTimer = Time.time + damageCoolDown;
            isAttacked = true;
            collision.GetComponent<PlayerStatusController>().Damage(damageAmount);
        }
    }
}
