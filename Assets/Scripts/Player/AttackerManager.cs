using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �U���̊Ǘ� / �ؑ�
/// </summary>
public class AttackerManager : MonoBehaviour
{
    /// <summary>
    /// ���ׂĂ̍U���N���X
    /// </summary>
    [SerializeField]
    private Attacker[] attackers;

    /// <summary>
    /// �I�𒆂̍U���N���X
    /// </summary>
    private Attacker currentAttacker;

    /// <summary>
    /// �U���F�O���Q�Ɨp
    /// </summary>
    public Attacker CurrentAttacker => currentAttacker;

    private void Start()
    {
        DeActivateAllAttackers();
        Initialize();
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    private void Initialize()
    {
        //�ݒ肳��Ă��Ȃ����
        if(attackers.Length == 0)
        {
            int childCount = transform.childCount; 
            attackers = new Attacker[childCount];
            for (int i = 0; childCount > i; i++)
            {
                attackers[i] = transform.GetChild(i).gameObject.GetComponent<Attacker>();
            }
        }

        //������Front
        currentAttacker = attackers[0];
        currentAttacker.gameObject.SetActive(true);
    }

    /// <summary>
    /// ���ׂĂ̍U����Ԃ��\����
    /// </summary>
    public void DeActivateAllAttackers()
    {
        for (int i = 0; i < attackers.Length; i++)
        {
            attackers[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ����̍U����\������
    /// </summary>
    /// <param name="direction"></param>
    public void ActivateAttacker(ATTACK_DIRECTION direction)
    {
        if (direction == ATTACK_DIRECTION.NONE)
            return;

        for (int i = 0; i < attackers.Length; i++)
        {
            if(attackers[i].AttackType == direction)
            {
                currentAttacker.gameObject.SetActive(false);
                currentAttacker = attackers[i];
                currentAttacker.gameObject.SetActive(true);
            }
        }
    }

    
    /// <summary>
    /// �SAttack�̃A�j���[�V�����̃��Z�b�g
    /// </summary>
    public void SetAllAnimatorIdle()
    {
        foreach(var anim in this.attackers)
        {
            anim?.SetAnimationIdle();
        }
    }
}
