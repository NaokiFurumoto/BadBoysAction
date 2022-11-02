using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;

/// <summary>
/// Viewに関する制御クラス
/// </summary>
public partial class GameController : MonoBehaviour
{
    /// <summary>
    /// スタート画面
    /// </summary>
    [SerializeField]
    private GameObject startView;

    /// <summary>
    /// オプション画面
    /// </summary>
    [SerializeField]
    private GameObject optionView;

    /// <summary>
    /// ゲームオーバー画面
    /// </summary>
    [SerializeField]
    private GameObject gameOverView;

    ///// <summary>
    ///// Fade画面
    ///// </summary>
    //[SerializeField]
    //private GameFade fadeView;

    //コールバック
    private UnityAction RetryAction;

    /// <summary>
    /// View初期化
    /// </summary>
    private void InitializeView()
    {
        RetryAction = RetryGame;
    }

    /// <summary>
    /// Viewの非表示
    /// </summary>
    public void DisableView(ViewBase _view)
    {
        _view.gameObject.SetActive(false);
    }

    /// <summary>
    /// Viewの表示
    /// </summary>
    public void EnableView(ViewBase _view)
    {
        _view.gameObject.SetActive(true);
    }

    /// <summary>
    /// オプションボタンが押されたときに呼ぶ
    /// </summary>
    public void OnClickOptionButton()
    {
        if (State == INGAME_STATE.STOP)
        {
            //閉じる
            GameResume();
            optionView.SetActive(false);
        }
        else if (State == INGAME_STATE.PLAYING)
        {
            //開く
            GameStop();
            optionView.SetActive(true);
        }
    }

    /// <summary>
    /// リトライボタンが押されたとき
    /// </summary>
    public void OnClickRetryButtin()
    {
        if (uiController.StaminasManager.IsCheckRecovery())
        {
            //スタミナを使用して再開
            uiController.StaminasManager.UseStamina();

            FadeFilter.Instance.FadeIn(Color.black, FADETIME, RetryAction);
            RetryStatus();
            gameOverView.gameObject.SetActive(false);
        }
        else
        {
            //スタミナ確認ダイアログ表示
            var dialog = 
                CommonDialog.ShowDialog
                (
                    STAMINA_LESS_TITLE,
                    STAMINA_LESS_DESC,
                    MOVIECHECK,
                    CLOSE,
                    () => UnityAdsManager.Instance.ShowRewarded(result =>
                    {
                        //スタミナ全回復
                        if (result == ShowResult.Finished)
                        {
                            StaminasManager.Instance.FullRecovery
                            (true, CommonDialogManager.Instance.DeleteDialogAll);
                        }
                    }
                ));

            //リストに追加
            CommonDialogManager.Instance.AddList(dialog);
        }
    }
   
}
