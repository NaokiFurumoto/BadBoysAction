using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �G�̏�ԊǗ��N���X
/// </summary>

///���
public enum ENEMY_STATUS
{
    NONE,//���ݒ�
    IDLE,//�ҋ@
    MOVE,//�ړ���
    ATTACK,//�U����
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
    /// �G�̏��
    /// </summary>
    [SerializeField]
    private ENEMY_STATUS status = ENEMY_STATUS.NONE;

    /// <summary>
    /// �_���[�W����
    /// </summary>
    [SerializeField]
    private bool isDamage;

   
}
