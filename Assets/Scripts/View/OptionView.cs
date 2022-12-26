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
    /// ���O�C�����
    /// </summary>
    [SerializeField]
    private GameObject loginViewObj;
    private LoginView loginView;

    /// <summary>
    /// �T�C���A�b�v�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject sighup;

    /// <summary>
    /// ���O�C��/�A�E�g�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject login;

    /// <summary>
    /// ���[�U�[
    /// </summary>
    [SerializeField]
    private UserAuth user;

    /// <summary>
    /// ���O�C���{�^��
    /// </summary>
    [SerializeField]
    private Button loginBtn;

    /// <summary>
    /// �T�C���C���{�^��
    /// </summary>
    [SerializeField]
    private Button sighinBtn;

    /// <summary>
    /// ���O�A�E�g�{�^��
    /// </summary>
    [SerializeField]
    private Button logoutBtn;

    /// <summary>
    /// �����N�{�^��
    /// </summary>
    [SerializeField]
    private Button rankBtn;

    private UiController uiController;

    /// <summary>
    /// �L���b�V���p
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;

    /// <summary>
    /// ������
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
        ///�C�x���g�̓o�^
        var eventTrigger = BG.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(eventData => { gameController?.OnClickOptionButton(); });
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// �^�C�g����ʂɈړ�
    /// </summary>
    public void OnClickTitleBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        //�X�^�~�i��Ԃ�ۑ�
        CommonDialog.ShowDialog(OPTION_GOSTART_TITLE, OPTION_GOSTART_DESC, YES, NO, GoTitleCallback);
    }

    /// <summary>
    /// �����L���O��ʂ̕\��
    /// </summary>
    public void OnClickRankBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        //FM.FadeOutVolume(appSound.BGM_STAGE, 0.0f, 0.5f, false);
        SceneManager.sceneLoaded += KeepScore;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(uiController.GetKillsNumber());
    }

    /// <summary>
    /// �T�C���A�b�v��ʂ̕\��
    /// </summary>
    public void OnClickSignUpBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.NEW);
    }

    /// <summary>
    /// ���O�C����ʂ̕\��
    /// </summary>
    public void OnClickLoginBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.LOGIN);
    }

    /// <summary>
    /// ���O�A�E�g��ʂ̕\��
    /// </summary>
    public void OnClickLogoutBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.LOGOUT);
    }

    /// <summary>
    /// �^�C�g����ʂɈȍ~
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
    /// ���C�A�E�g�̕ύX
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
    /// �L����
    /// </summary>
    protected override void OnEnable()
    {
        //��x���[�h
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
    /// �����L���O�V�[���ɓn������
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
        // �C�x���g�̍폜
        SceneManager.sceneLoaded -= KeepScore;
    }
}
