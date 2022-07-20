using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �v���[���[�̃X�e�[�^�X�Ǘ�
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    /// <summary>
    /// �ő僉�C�t
    /// TODO:�Q�[���}�l�[�W���Ɏ�������H
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
    /// TODO:�Q�[���}�l�[�W���Ɏ�������H
    /// </summary>
    [SerializeField]
    private int startLife = 1;

    [SerializeField]
    private Transform playerCenter;

    /// <summary>
    /// TODO:�Q�[���}�l�[�W���Ɏ�������
    /// </summary>
    public static readonly int DAMAGE = 1;

    /// <summary>
    /// TODO:�Q�[���}�l�[�W���Ɏ�������
    /// </summary>
    public static readonly int LIFEPOINT = 1;

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
        life = startLife;
        isDead = false;
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

        life += LIFEPOINT;
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
 
