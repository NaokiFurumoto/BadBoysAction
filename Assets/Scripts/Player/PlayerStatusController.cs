using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �v���[���[�̃X�e�[�^�X�Ǘ�
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    //���ϐ�
    //�ő�̗�
    //���݂̗̑�
    //�������C�t
    //������΂���
    //���G���
    //��������

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
    }




    //���֐�
    //���C�t�����炷
    //���C�t�𑝂₷
    //���S����
    //�����Ă�����H
}
 
