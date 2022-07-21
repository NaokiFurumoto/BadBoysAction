using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

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
        //����Ȃ��
        life    = startLife 
                = START_LIFEPOINT;
        isDead  = false;
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
        if( life <= 0)
        {
            isDead = true;
            //���S����
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
        //�X�^�~�i�����炷
        //�X�^�~�i�Ȃ����GAMEOVER
    }
   
}
 
