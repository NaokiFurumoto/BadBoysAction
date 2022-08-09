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

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
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
    /// 回復更新
    /// </summary>
    void Update()
    {
        if (IsCheckRecovery())
            return;

        //時間チェック：時間経過すれば1つ回復
        progressTime += Time.deltaTime;
        if (progressTime >= STAMINA_RECOVERY_TIME)
        {
            RecoveryOneStamina();
            progressTime = 0;
        }
    }

    /// <summary>
    /// 全回復
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
    /// スタミナ使用
    /// </summary>
    public bool UseStamina()
    {
        //スタミナを1つ使用：若い順
        foreach(var stamina in staminaStatus)
        {
            if (stamina.IsRecovery)
            {
                stamina.ChangeStaminaImage(false);
                return true;
            }
        }

        //使用されない状態
        return false;
    }

    /// <summary>
    /// スタミナを１つ回復
    /// </summary>
    private void RecoveryOneStamina()
    {
        //回復されてれば実行しない
        if (IsCheckRecovery())
            return;

        //回復する必要があるか？
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

        //true:回復されている
        return useStaminaNumber >= STAMINA_MAXNUMBER;
    }

    /// <summary>
    /// 現在の日時を取得する
    /// </summary>
    /// <returns></returns>
    private DateTime GetTimeNow()
    {
        return DateTime.Now;
    }
}
