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
        //1�ł��g�p����Ă���΍X�V
        if (!IsCheckUsedStamina())
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
    public void UseStamina()
    {
        //�X�^�~�i��1�g�p�F�Ⴂ��
        foreach(var stamina in staminaStatus)
        {
            if (stamina.IsRecovery)
            {
                stamina.ChangeStaminaImage(false);
                return;
            }
        }
    }

    /// <summary>
    /// �X�^�~�i���P��
    /// </summary>
    private void RecoveryOneStamina()
    {
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
    /// �g�p�ł���X�^�~�i�����邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsCheckRecovery()
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
        return useStaminaNumber > 0;
    }


    /// <summary>
    ///1�ł��X�^�~�i�������Ă邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsCheckUsedStamina()
    {
        foreach (var stamina in staminaStatus)
        {
            if (!stamina.IsRecovery)
            {
                return true;
            }
        }

        return false;
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
