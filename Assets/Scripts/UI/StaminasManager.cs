using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
using System;

public class StaminasManager : MonoBehaviour
{
    /// <summary>
    /// �X�^�~�i���X�g
    /// </summary>
    [SerializeField]
    private List<StaminaStatus> staminaStatus = new List<StaminaStatus>();

    /// <summary>
    /// �c��g�p�X�^�~�i��
    /// </summary>
    [SerializeField]
    private int useStaminaNumber;

    /// <summary>
    /// �v������
    /// </summary>
    private float progressTime;

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        if (staminaStatus == null)
        {
            var array = this.gameObject.GetComponentsInChildren<StaminaStatus>();
            staminaStatus.AddRange(array);
        }
        progressTime = 0;
    }

    /// <summary>
    /// �񕜍X�V
    /// </summary>
    void Update()
    {
        if (IsCheckRecovery())
            return;

        //���ԃ`�F�b�N�F���Ԍo�߂����1��
        progressTime += Time.deltaTime;
        if (progressTime >= STAMINA_RECOVERY_TIME)
        {
            RecoveryOneStamina();
            progressTime = 0;
        }
    }

    /// <summary>
    /// �S��
    /// </summary>
    public void FullRecovery()
    {
        if (staminaStatus == null)
            return;
        foreach(var stamina in staminaStatus)
        {
            stamina.ChangeStaminaImage(true);
            useStaminaNumber = STAMINA_MAXNUMBER;
        }
    }

    /// <summary>
    /// �X�^�~�i�g�p
    /// </summary>
    public bool UseStamina()
    {
        //�X�^�~�i��1�g�p�F�Ⴂ��
        foreach(var stamina in staminaStatus)
        {
            if (stamina.IsRecovery)
            {
                stamina.ChangeStaminaImage(false);
                return true;
            }
        }

        //�g�p����Ȃ����
        return false;
    }

    /// <summary>
    /// �X�^�~�i���P��
    /// </summary>
    private void RecoveryOneStamina()
    {
        //�񕜂���Ă�Ύ��s���Ȃ�
        if (IsCheckRecovery())
            return;

        //�񕜂���K�v�����邩�H
        foreach(var stamina in staminaStatus)
        {
            if (!stamina.IsRecovery)
            {
                stamina.ChangeStaminaImage(true);
                return;
            }
        }
    }

    /// <summary>
    /// �񕜃`�F�b�N
    /// </summary>
    /// <returns></returns>
    private bool IsCheckRecovery()
    {
        useStaminaNumber = 0;
        foreach (var stamina in staminaStatus)
        {
            if (stamina.IsRecovery)
            {
                useStaminaNumber++;
            }
        }

        //true:�񕜂���Ă���
        return useStaminaNumber >= STAMINA_MAXNUMBER;
    }

    /// <summary>
    /// ���݂̓������擾����
    /// </summary>
    /// <returns></returns>
    private DateTime GetTimeNow()
    {
        return DateTime.Now;
    }
}
