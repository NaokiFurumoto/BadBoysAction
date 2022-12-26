using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
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
    /// ログインボタン
    /// </summary>
    [SerializeField]
    private Button loginBtn;

    /// <summary>
    /// サインインボタン
    /// </summary>
    [SerializeField]
    private Button sighinBtn;

    /// <summary>
    /// ログアウトボタン
    /// </summary>
    [SerializeField]
    private Button logoutBtn;

    /// <summary>
    /// ランクボタン
    /// </summary>
    [SerializeField]
    private Button rankBtn;

    private UiController uiController;

    /// <summary>
    /// キャッシュ用
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;

    /// <summary>
    /// 初期化
    /// </summary>
    private IEnumerator Start()
    {
        FM = SoundManager.Instance;
        appSound = AppSound.Instance;
        uiController = GameObject.FindGameObjectWithTag("UI").
                                 GetComponent<UiController>();
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
        FM.PlayOneShot(appSound.SE_MENU_OK);
        //スタミナ状態を保存
        CommonDialog.ShowDialog(OPTION_GOSTART_TITLE, OPTION_GOSTART_DESC, YES, NO, GoTitleCallback);
    }

    /// <summary>
    /// ランキング画面の表示
    /// </summary>
    public void OnClickRankBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        //FM.FadeOutVolume(appSound.BGM_STAGE, 0.0f, 0.5f, false);
        SceneManager.sceneLoaded += KeepScore;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(uiController.GetKillsNumber());
    }

    /// <summary>
    /// サインアップ画面の表示
    /// </summary>
    public void OnClickSignUpBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.NEW);
    }

    /// <summary>
    /// ログイン画面の表示
    /// </summary>
    public void OnClickLoginBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.LOGIN);
    }

    /// <summary>
    /// ログアウト画面の表示
    /// </summary>
    public void OnClickLogoutBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.LOGOUT);
    }

    /// <summary>
    /// タイトル画面に以降
    /// </summary>
    private void GoTitleCallback()
    {
        FM.FadeOutVolume(appSound.BGM_STAGE, 0.0f, 1.0f, false);
        StartCoroutine("FadeTitle");
    }

    private IEnumerator FadeTitle()
    {

        gameController.GameResume();
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        LoadScene.Load("StartScene");
    }

    /// <summary>
    /// レイアウトの変更
    /// </summary>
    public void ChangeLayout()
    {
        sighup.SetActive(!user.IsSignUp);
        login.SetActive(user.IsSignUp);
        loginBtn.interactable = !(user.IsLogin);
        logoutBtn.interactable = user.IsLogin;
        sighinBtn.interactable = !(user.IsLogin);
        rankBtn.interactable = user.IsLogin;
    }

    /// <summary>
    /// 有効時
    /// </summary>
    protected override void OnEnable()
    {
        //一度ロード
        var loadData = SaveManager.Instance.Load();
        if(user != null && loadData != null)
        {
            user.IsSignUp = loadData.IsSighin;
            user.IsLogin = loadData.IsLogin;
            var name = loadData.UserName;
            var pw = loadData.Passward;
            ChangeLayout();
        }
    }

    protected override void OnDisable()
    {
    }

    /// <summary>
    /// ランキングシーンに渡すもの
    /// </summary>
    /// <param name="nextScene"></param>
    /// <param name="mode"></param>
    private void KeepScore(Scene nextScene, LoadSceneMode mode)
    {
        var rank = GameObject.Find("RankingSceneManager").GetComponent<RankingScene>();
        rank.UserName = user.CurrentPlayer;
        rank.Score = uiController.GetKillsNumber();
        rank.Hiscore = uiController.GetHiScore();
        rank.IsGameOver = false;
        // イベントの削除
        SceneManager.sceneLoaded -= KeepScore;
    }
}
