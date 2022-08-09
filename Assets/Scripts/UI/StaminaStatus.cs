using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;

public class StaminaStatus : MonoBehaviour
{
    /// <summary>
    /// �X�^�~�i�ԍ�
    /// </summary>
    [SerializeField]
    private int number;

    /// <summary>
    /// �񕜔���
    /// </summary>
    [SerializeField]
    private bool isRecovery;

    /// <summary>
    /// ON�C���[�W
    /// </summary>
    [SerializeField]
    private GameObject image_On;

    #region �v���p�e�B
    public int Number => number;
    public bool IsRecovery => isRecovery;
    #endregion


    //���֐�
    //�\���ؑ�
    //�񕜎��Ԍv���F����ꂽ�^�C�~���O��


    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
   private void Initialize()
    {

    }


    /// <summary>
    /// �C���[�W�؂�ւ�
    /// </summary>
    /// <param name="_have"></param>
    public void ChangeStaminaImage(bool _have)
    {
        isRecovery = _have;
        image_On.SetActive(_have);
    }




}
