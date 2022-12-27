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

    private GameController gameController;

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
        //参照が外れてるので初期化F
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
    /// ゲーム中の回復更新
    /// 1時間に１つ回復
    /// </summary>
    void Update()
    {
        //1つでも使用されていれば更新
        if (!IsCheckUsedStamina())
            return;

        //時間チェック：時間経過すれば1つ回復
        //var kesstime = STAMINA_RECOVERY_TIME - Time.deltaTime;

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
        if (staminaStatus == null) return;

        foreach (var stamina in staminaStatus)
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
            gameController?.GameStop();
            CommonDialogManager.Instance.AddList(dialog);
        }
    }

    /// <summary>
    /// スタミナ使用
    /// </summary>
    public void UseStamina()
    {
        //スタミナを1つ使用：若い順
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
    /// スタミナを１つ回復
    /// </summary>
    public void RecoveryOneStamina()
    {
        //最後からチェック
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
    ///プレイ中に1つでもスタミナが消費されてるかどうか
    ///プレイ中以外で呼ぶな！！
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
    /// プレイ中以外で呼ぶな！！
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
        //一度全部falseに設定する
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

    /// <summary>
    /// ロード後のスタミナ回復処理
    /// </summary>
    /// <param name="loadingTime">最後にセーブされた時間</param>
    public void loadRecoveryStaminas(long loadingTime)
    {
        // スタミナが全回なら実行しない
        if (!IsCheckUsedStamina())
            return;

        var nowTime = TimeManager.Instance.GetDayTimeInteger();
        var diffTime = nowTime - loadingTime;

        if (diffTime >= STAMINA_RECOVERY_LONGTIME)
        {
            //全開
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
