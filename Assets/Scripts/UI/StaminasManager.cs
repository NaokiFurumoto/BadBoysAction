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
    /// スタミナリスト
    /// </summary>
    [SerializeField]
    private List<StaminaStatus> staminaStatus = new List<StaminaStatus>();

    /// <summary>
    /// 残り使用スタミナ数
    /// </summary>
    [SerializeField]
    private int useStaminaNumber;

    /// <summary>
    /// 計測時間
    /// </summary>
    private float progressTime;

    /// <summary>
    /// 残り回復時間テキスト表示
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_RecoveryTime;

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
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
    /// 回復更新
    /// </summary>
    void Update()
    {
        //1つでも使用されていれば更新
        if (!IsCheckUsedStamina())
            return;

        //時間チェック：時間経過すれば1つ回復
        var kesstime = STAMINA_RECOVERY_TIME - Time.deltaTime;

        progressTime += Time.deltaTime;

        //UI更新
        SetRecoveryTime(progressTime);

        if (progressTime >= STAMINA_RECOVERY_TIME)
        {
            RecoveryOneStamina();
           
            //回復後のスタミナ数が満タンならテキスト非表示
            var isMax = GetUseStaminaNumber() == STAMINA_MAXNUMBER ? false : true;
            ActiveTextRecovery(isMax);

            progressTime = 0;
        }
    }

    /// <summary>
    /// 全回復
    /// </summary>
    /// <param name="isDialog">回復後にダイアログを表示するか？</param>
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
            ///全回復お知らせダイアログ
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
    /// スタミナ使用
    /// </summary>
    public void UseStamina()
    {
        //スタミナを1つ使用：若い順
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
    /// スタミナを１つ回復
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
    /// 回復チェック
    /// 使用できるスタミナがあるかどうか
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

        //true:回復されている
        return useStaminaNumber > 0;
    }


    /// <summary>
    ///1つでもスタミナが消費されてるかどうか
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
    /// 残りスタミナ数を取得
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
    /// スタミナを個数で設定
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
            {   //1つ回復
                RecoveryOneStamina();
            }
        }
    }

    /// <summary>
    /// スタミナを全て使用不可とする
    /// </summary>
    public void SetAllDisable()
    {
        staminaStatus.ForEach(stamina => stamina.ChangeStaminaImage(false));

    }

    /// <summary>
    /// 現在の日時を取得する
    /// </summary>
    /// <returns></returns>
    private DateTime GetTimeNow()
    {
        return DateTime.Now;
    }

    /// <summary>
    /// 残りスタミナ回復時間の表示
    /// </summary>
    /// <param name="progress"></param>
    private void SetRecoveryTime(float progress)
    {
        float recoveryTime = STAMINA_RECOVERY_TIME - progress;
        var span = new TimeSpan(0, 0, (int)recoveryTime);

        text_RecoveryTime.text = span.ToString(@"mm\:ss");
    }

    /// <summary>
    /// 回復時間の表示切替
    /// </summary>
    /// <param name="ismax"></param>
    public void ActiveTextRecovery(bool ismax)
    {
        text_RecoveryTime.gameObject.SetActive(ismax);
    }
   
}
