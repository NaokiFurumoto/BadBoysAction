using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using System;
using System.Linq;
using TMPro;

public class StaminasManager : MonoBehaviour
{
    public static StaminasManager Instance;

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

    /// <summary>
    /// �c��񕜎��ԃe�L�X�g�\��
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_RecoveryTime;

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        Instance ??= this;
        
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
        var kesstime = STAMINA_RECOVERY_TIME - Time.deltaTime;

        progressTime += Time.deltaTime;

        //UI�X�V
        SetRecoveryTime(progressTime);

        if (progressTime >= STAMINA_RECOVERY_TIME)
        {
            RecoveryOneStamina();
           
            //�񕜌�̃X�^�~�i�������^���Ȃ�e�L�X�g��\��
            var isMax = GetUseStaminaNumber() == STAMINA_MAXNUMBER ? false : true;
            ActiveTextRecovery(isMax);

            progressTime = 0;
        }
    }

    /// <summary>
    /// �S��
    /// </summary>
    /// <param name="isDialog">�񕜌�Ƀ_�C�A���O��\�����邩�H</param>
    public void FullRecovery(bool isDialog, Action callback = null)
    {
        if (staminaStatus == null)
            return;
        foreach(var stamina in staminaStatus)
        {
            stamina.ChangeStaminaImage(true);
            useStaminaNumber = STAMINA_MAXNUMBER;
        }
        ActiveTextRecovery(false);

        if (isDialog)
        {
            ///�S�񕜂��m�点�_�C�A���O
            var dialog = 
                CommonDialog.ShowDialog
                (
                    STAMINA_FULLRECOVERY_TITLE,
                    STAMINA_FULLRECOVERY_DESC,
                    null,
                    CLOSE,
                    null,
                    () => callback()
                );

            CommonDialogManager.Instance.AddList(dialog);
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
    /// �c��X�^�~�i�����擾
    /// </summary>
    /// <returns></returns>
    public int GetUseStaminaNumber()
    {
        var index = 0;
        foreach (var stamina in staminaStatus)
        {
            if (stamina.IsRecovery)
            {
                index++;
            }
        }

        return index;
    }

    /// <summary>
    /// �X�^�~�i�����Őݒ�
    /// </summary>
    /// <param name="num"></param>
    public void SetStaminaNumber(int num)
    {
        for (var i = 0; i < num; i++)
        {
            var useIndex = GetUseStaminaNumber();

            if (useIndex >= num)
            {
                return;
            }
            else
            {   //1��
                RecoveryOneStamina();
            }
        }
    }

    /// <summary>
    /// �X�^�~�i��S�Ďg�p�s�Ƃ���
    /// </summary>
    public void SetAllDisable()
    {
        staminaStatus.ForEach(stamina => stamina.ChangeStaminaImage(false));

    }

    /// <summary>
    /// ���݂̓������擾����
    /// </summary>
    /// <returns></returns>
    private DateTime GetTimeNow()
    {
        return DateTime.Now;
    }

    /// <summary>
    /// �c��X�^�~�i�񕜎��Ԃ̕\��
    /// </summary>
    /// <param name="progress"></param>
    private void SetRecoveryTime(float progress)
    {
        float recoveryTime = STAMINA_RECOVERY_TIME - progress;
        var span = new TimeSpan(0, 0, (int)recoveryTime);

        text_RecoveryTime.text = span.ToString(@"mm\:ss");
    }

    /// <summary>
    /// �񕜎��Ԃ̕\���ؑ�
    /// </summary>
    /// <param name="ismax"></param>
    public void ActiveTextRecovery(bool ismax)
    {
        text_RecoveryTime.gameObject.SetActive(ismax);
    }
   
}
