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
    /// ������
    /// </summary>
    private IEnumerator Start()
    {
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
        //���݂̃o�g�����I�����ă^�C�g���ɖ߂�܂��B��낵���ł��傤���H
        //�X�^�~�i��Ԃ�ۑ�
        CommonDialog.ShowDialog(OPTION_GOSTART_TITLE, OPTION_GOSTART_DESC, YES, NO, GoTitleCallback);
    }

    /// <summary>
    /// �����L���O��ʂ̕\��
    /// </summary>
    public void OnClickRankBtn()
    {
    }

    /// <summary>
    /// �T�C���A�b�v��ʂ̕\��
    /// </summary>
    public void OnClickSignUpBtn()
    {
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.NEW);
    }

    /// <summary>
    /// ���O�C����ʂ̕\��
    /// </summary>
    public void OnClickLoginBtn()
    {
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.LOGIN);
    }

    /// <summary>
    /// ���O�A�E�g��ʂ̕\��
    /// </summary>
    public void OnClickLogoutBtn()
    {
        loginViewObj?.SetActive(true);
        loginView?.SwitchMenu(LoginView.LOGIN_TYPE.LOGOUT);
    }

    /// <summary>
    /// �^�C�g����ʂɈȍ~
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
    /// �L����
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
