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

    private GameController gameController;

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
        //�Q�Ƃ��O��Ă�̂ŏ�����F
        Instance ??= this;

        if (Instance == null)
        {
            Instance = GameObject.FindGameObjectWithTag("StaminaRoot").
                                    GetComponent<StaminasManager>();
        }

        progressTime = 0;
        gameController = GameObject.FindGameObjectWithTag("GameController").
                                        GetComponent<GameController>();
    }

    /// <summary>
    /// �Q�[�����̉񕜍X�V
    /// 1���ԂɂP��
    /// </summary>
    void Update()
    {
        //1�ł��g�p����Ă���΍X�V
        if (!IsCheckUsedStamina())
            return;

        //���ԃ`�F�b�N�F���Ԍo�߂����1��
        //var kesstime = STAMINA_RECOVERY_TIME - Time.deltaTime;

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
        if (staminaStatus == null) return;

        foreach (var stamina in staminaStatus)
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
            gameController?.GameStop();
            CommonDialogManager.Instance.AddList(dialog);
        }
    }

    /// <summary>
    /// �X�^�~�i�g�p
    /// </summary>
    public void UseStamina()
    {
        //�X�^�~�i��1�g�p�F�Ⴂ��
        foreach (var stamina in staminaStatus)
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
    public void RecoveryOneStamina()
    {
        //�Ōォ��`�F�b�N
        for(var i = staminaStatus.Count()-1; i >= 0; i--)
        {
            if (!staminaStatus[i].IsRecovery)
            {
                staminaStatus[i].ChangeStaminaImage(true);
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
    ///�v���C����1�ł��X�^�~�i�������Ă邩�ǂ���
    ///�v���C���ȊO�ŌĂԂȁI�I
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
    /// �v���C���ȊO�ŌĂԂȁI�I
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
        //��x�S��false�ɐݒ肷��
        staminaStatus.ForEach(status => status.ChangeStaminaImage(false));
        if (num == 0) return;
        //0/1/2 = 1/2 1 =2
        for (var i = (staminaStatus.Count()-1); num > 0; i--)
        {
            num--;
            staminaStatus[i].ChangeStaminaImage(true);
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

    /// <summary>
    /// ���[�h��̃X�^�~�i�񕜏���
    /// </summary>
    /// <param name="loadingTime">�Ō�ɃZ�[�u���ꂽ����</param>
    public void loadRecoveryStaminas(long loadingTime)
    {
        // �X�^�~�i���S��Ȃ���s���Ȃ�
        if (!IsCheckUsedStamina())
            return;

        var nowTime = TimeManager.Instance.GetDayTimeInteger();
        var diffTime = nowTime - loadingTime;

        if (diffTime >= STAMINA_RECOVERY_LONGTIME)
        {
            //�S�J
            FullRecovery(true, () =>
            {
                CommonDialogManager.Instance.DeleteDialogAll();
                gameController.GameResume();
                //GameStart();
            });
        }
        else
        {
            diffTime -= STAMINA_RECOVERY_ONE_LONGTIME;
            while (diffTime >= 0)
            {
                RecoveryOneStamina();
                if (!IsCheckUsedStamina())
                    break;
                diffTime -= STAMINA_RECOVERY_ONE_LONGTIME;
            }
        }
    }

}
