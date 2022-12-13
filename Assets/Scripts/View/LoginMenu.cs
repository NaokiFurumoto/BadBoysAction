using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NCMB;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LoginMenu : LoginMenuBase
{
    [SerializeField]
    private TMP_InputField inputFieldID;

    [SerializeField]
    private TMP_InputField inputFieldPASS;

    [SerializeField]
    private Button btn_Login;

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

    public override void OnEndEdit_ID()
    {
        id = inputFieldID.text;
    }

    public override void OnEndEdit_Pass()
    {
        pass = inputFieldPASS.text;
    }

    /// <summary>
    /// ログイン
    /// </summary>
    public void OnClickLogin()
    {
        if (user == null)
            return;
        
        if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pass))
        {
            txt_Caution.SetActive(true);
            return;
        }

        txt_Caution.SetActive(false);
        user.logIn(id, pass, FailureCallback, SuccessCallback);
    }

    /// <summary>
    /// サインイン失敗時のコールバック
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
    /// サインアップ成功時のコールバック
    /// </summary>
    public void SuccessSignUp()
    {
        if (!txt_Success.activeSelf)
        {
            txt_Success.SetActive(true);
            txt_Caution.SetActive(false);
        }
    }
}
