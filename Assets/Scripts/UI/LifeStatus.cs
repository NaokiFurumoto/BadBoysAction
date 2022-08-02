using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;

/// <summary>
/// ���C�t�̃X�e�[�^�X�Ɋւ���N���X
/// </summary>
public class LifeStatus : MonoBehaviour
{
    /// <summary>
    /// ���C�t�ԍ��F1-10
    /// </summary>
    [SerializeField]
    private int number;

    /// <summary>
    /// ON�C���[�W
    /// </summary>
    [SerializeField]
    private GameObject image_On;

    /// <summary>
    /// �擾����Ă锻��
    /// </summary>
    [SerializeField]
    private bool isHave;

    #region �v���p�e�B
    public int Number => number;
    public bool IsHave => isHave;   
    #endregion

    /// <summary>
    /// ON�C���[�W�؂�ւ�
    /// </summary>
    /// <param name="_have"></param>
    public void ChangeLifeImage(bool _have)
    {
        isHave = _have;
        image_On.SetActive(_have);
    }

    private void Start()
    {
        Initialize();     
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        if(number == 1)
        {
            image_On.SetActive(true);
        }
        else
        {
            image_On.SetActive(false);
        }
    }
}
