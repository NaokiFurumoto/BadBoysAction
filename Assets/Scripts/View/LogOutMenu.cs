using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NCMB;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LogOutMenu : LoginMenuBase
{
    [SerializeField]
    private Button btn_Logout;

    [SerializeField]
    private GameObject txt_Caution;

    [SerializeField]
    private GameObject txt_Success;

    private Action FailureCallback;
    private Action SuccessCallback;

    protected override void OnEnable()
    {
        FailureCallback = FailureSignUp;
        SuccessCallback = SuccessSignUp;
    }

    protected override void OnDisable()
    {
        FailureCallback = null;
        SuccessCallback = null;
        txt_Caution.SetActive(false);
        txt_Success.SetActive(false);
    }

    /// <summary>
    /// �T�C���C�����s���̃R�[���o�b�N
    /// </summary>
    public void FailureSignUp()
    {
        if (!txt_Caution.activeSelf)
        {
            txt_Caution.SetActive(true);
            txt_Success.SetActive(false);
        }
    }

    /// <summary>
    /// �T�C���A�b�v�������̃R�[���o�b�N
    /// </summary>
    public void SuccessSignUp()
    {
        if (!txt_Success.activeSelf)
        {
            txt_Success.SetActive(true);
            txt_Caution.SetActive(false);
        }
    }

    /// <summary>
    /// ���O�A�E�g
    /// </summary>
    public void OnClickLogout()
    {
        if (user == null)
            return;
        SoundManager.Instance.PlayOneShot(AppSound.Instance.SE_MENU_OK);
        user.logOut(FailureCallback, SuccessCallback);
    }
}
