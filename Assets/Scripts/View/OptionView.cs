using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static GlobalValue;

public class OptionView : ViewBase
{
    [SerializeField]
    private GameObject BG;

    /// <summary>
    /// ログイン画面
    /// </summary>
    [SerializeField]
    private GameObject loginViewObj;
    private LoginView loginView;

    /// <summary>
    /// サインアップオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject sighup;

    /// <summary>
    /// ログイン/アウトオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject login;

    /// <summary>
    /// ユーザー
    /// </summary>
    [SerializeField]
    private UserAuth user;

    /// <summary>
    /// 初期化
    /// </summary>
    private IEnumerator Start()
    {
        loginView = loginViewObj?.GetComponent<LoginView>();
        yield return new WaitForSecondsRealtime(1);
        OpenDialogCallBack();
    }


    private void OpenDialogCallBack()
    {
        ///イベントの登録
        var eventTrigger = BG.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(eventData => { gameController?.OnClickOptionButton(); });
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// タイトル画面に移動
    /// </summary>
    public void OnClickTitleBtn()
    {
        //現在のバトルを終了してタイトルに戻ります。よろしいでしょうか？
        //スタミナ状態を保存
        CommonDialog.ShowDialog(OPTION_GOSTART_TITLE, OPTION_GOSTART_DESC, YES, NO, GoTitleCallback);
    }

    /// <summary>
    /// ランキング画面の表示
    /// </summary>
    public void OnClickRankBtn()
    {
    }

    /// <summary>
    /// サインアップ画面の表示
    /// </summary>
    public void OnClickSignUpBtn()
    {
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.NEW);
    }

    /// <summary>
    /// ログイン画面の表示
    /// </summary>
    public void OnClickLoginBtn()
    {
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.LOGIN);
    }

    /// <summary>
    /// ログアウト画面の表示
    /// </summary>
    public void OnClickLogoutBtn()
    {
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.LOGOUT);
    }

    /// <summary>
    /// タイトル画面に以降
    /// </summary>
    private void GoTitleCallback()
    {
        StartCoroutine("FadeTitle");
    }

    private IEnumerator FadeTitle()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        gameController.GameResume();
        LoadScene.Load("StartScene");
    }

    /// <summary>
    /// 有効時
    /// </summary>
    protected override void OnEnable()
    {
        //sighup.SetActive(!user.IsSignUp);
        //login.SetActive(user.IsSignUp);
    }

    protected override void OnDisable()
    {
    }


}
