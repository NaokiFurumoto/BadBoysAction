using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �L�����N�^�[�̍U���Ɋւ���N���X
/// </summary>

//�U������
public enum ATTACK_DIRECTION
{
    NONE,
    FRONT,      //����
    UP,         //��
    SIDE,       //��
    SIDEUP,     //�΂ߏ�
    SIDEDOWN    //�΂߉�
}

public class Attacker : MonoBehaviour
{
    /// <summary>
    /// �Z���T�[
    /// </summary>
    [SerializeField]
    private CircleCollider2D collider;

    /// <summary>
    /// Sprite
    /// </summary>
    [SerializeField]
    private GameObject sprite;

    /// <summary>
    /// �A�j���[�V����
    /// </summary>
    private Animator animator;

    /// <summary>
    /// �U���̕���
    /// </summary>
    [SerializeField]
    private ATTACK_DIRECTION attackType;

    /// <summary>
    /// �U�������̎擾
    /// </summary>
    public ATTACK_DIRECTION AttackType => attackType;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        animator = sprite?.GetComponent<Animator>();
    }

    /// <summary>
    /// �Z���T�[�ɏՓ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //TODO:��x�U�����Ă�Ǝ��s���Ȃ�
            animator.SetTrigger("Attack");
            //������΂��F�b��ł������H
        }
    }

    /// <summary>
    /// �Z���T�[�ɏՓ˒�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    /// <summary>
    /// �Z���T�[���甲����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //�U�������L��������������
        }
    }

    //������΂�����

    
}
