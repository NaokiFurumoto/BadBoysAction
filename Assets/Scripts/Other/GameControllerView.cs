using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GlobalValue;
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

    /// <summary>
    /// Fade画面
    /// </summary>
    [SerializeField]
    private GameFade fadeView;

    //コールバック
    private Action FadeinAction;
    private Action RetryAction;

    /// <summary>
    /// View初期化
    /// </summary>
    private void InitializeView()
    {
        FadeinAction = RetryFadein;
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
        //暗転後に再開
        fadeView.gameObject.SetActive(true);
        fadeView.Play(GameFade.Mode.OUT, Color.black, FADETIME, FadeinAction);

        if (uiController.StaminasManager.IsCheckRecovery())
        {
            //スタミナを使用して再開
            uiController.StaminasManager.UseStamina();
        }
        else
        {
            //スタミナ確認ダイアログ表示
        }
    }

    /// <summary>
    /// FadeIn処理
    /// </summary>
    private void RetryFadein()
    {
        fadeView.Play(GameFade.Mode.IN, new Color(1.0f,1.0f,1.0f,0.0f), FADETIME, RetryAction);

        gameOverView.gameObject.SetActive(false);
        RetryStatus();
    }

}
