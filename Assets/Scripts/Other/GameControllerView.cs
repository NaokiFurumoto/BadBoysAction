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
    private ViewBase FirstGameInfoView;


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

    //コールバック
    private UnityAction RetryAction;

    /// <summary>
    /// ゲーム説明画面を表示させるかどうか
    /// </summary>
    public bool IsOpenFirstview;

    /// <summary>
    /// ADS
    /// </summary>
    private Action<ShowResult> adsRewardViewCallBack;

    /// <summary>
    /// View初期化
    /// </summary>
    private void InitializeView()
    {
        RetryAction = RetryGame;
        adsRewardViewCallBack = AdsRewardViewCallBack;
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
        if (FM == null || appSound == null)
            return;

        FM.PlayOneShot(appSound.SE_MENU_OK);
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
    /// ゲーム説明画面の表示
    /// </summary>
    public void OpenFirstView()
    {
        if (IsOpenFirstview)
        {
            PlayInGame();
        }
        else
        {
            EnableView(FirstGameInfoView);
            var firstView = FirstGameInfoView as FirstGameInfoView;
            firstView.CallBack = PlayInGame;
        }
    }


    /// <summary>
    /// リトライボタンが押されたとき
    /// </summary>
    public void OnClickRetryButtin()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        //広告の表示　広告終了後にリトライ処理
        //このタイミングで広告表示。2回に１回広告表示：
        if (uiController.PlayTime % 2 == 0 && !uiController.GetIsAds())
        {
            UnityAdsManager.Instance.ShowInterstitial(adsInterCallBack);
        }
        else
        {
            //リトライ処理
            RetryAdsCallBack();
        }


        ///課金していたらそのままリトライ
        //if (uiController.GetIsAds())
        //{
        //    FadeFilter.Instance.FadeIn(Color.black, FADETIME, RetryAction);
        //    RetryStatus();
        //    gameOverView.gameObject.SetActive(false);
        //    return;
        //}

        //if (uiController.StaminasManager.IsCheckRecovery())
        //{
        //    //スタミナを使用して再開
        //    uiController.StaminasManager.UseStamina();

        //    FadeFilter.Instance.FadeIn(Color.black, FADETIME, RetryAction);
        //    RetryStatus();
        //    gameOverView.gameObject.SetActive(false);
        //}
        //else
        //{
        //    //スタミナ確認ダイアログ表示
        //    var dialog =
        //        CommonDialog.ShowDialog
        //        (
        //            STAMINA_LESS_TITLE,
        //            STAMINA_LESS_DESC,
        //            MOVIECHECK,
        //            CLOSE,
        //            () => UnityAdsManager.Instance.ShowRewarded(adsRewardViewCallBack
        //        ));

        //    //リストに追加
        //    CommonDialogManager.Instance.AddList(dialog);
        //}
    }

    /// <summary>
    /// タイトル画面に以降
    /// </summary>
    public void GoTitle()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        SaveManager.Instance.GamePlaingSave();
        StartCoroutine("GoFadeTitle");
    }

    private IEnumerator GoFadeTitle()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        GameResume();
        LoadScene.Load("StartScene");
    }

    //広告コールバック
    public void AdsRewardViewCallBack(ShowResult result)
    {
        if (StaminasManager.Instance == null)
        {
            StaminasManager.Instance = FindObjectOfType<StaminasManager>();
        }

        StaminasManager.Instance.FullRecovery
        (true, CommonDialogManager.Instance.DeleteDialogAll);
    }

    /// <summary>
    /// 広告表示後コールバック
    /// リトライボタン押した後のタイミング
    /// </summary>
    public void RetryAdsCallBack(ShowResult result = ShowResult.Finished)
    {
        ///課金していたらそのままリトライ
        if (uiController.GetIsAds())
        {
            FadeFilter.Instance.FadeIn(Color.black, FADETIME, RetryAction);
            RetryStatus();
            gameOverView.gameObject.SetActive(false);
            return;
        }

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
                    () => UnityAdsManager.Instance.ShowRewarded(adsRewardViewCallBack
                ));

            //リストに追加
            CommonDialogManager.Instance.AddList(dialog);
        }
    }
}
