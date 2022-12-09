using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using NCMB;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class SignUpMenu : LoginMenuBase
{
    [SerializeField]
    private TMP_InputField inputFieldID;

    [SerializeField]
    private TMP_InputField inputFieldPASS;

    [SerializeField]
    private GameObject caution_txt;

    [SerializeField]
    private Button btn_SighUp;

    public Action FailureCallback;

    public override void OnEndEdit_ID()
    {
        id = inputFieldID.text;
    }

    public override void OnEndEdit_Pass()
    {
        pass = inputFieldPASS.text;
    }

    public void OnSignUp()
    {
        //NCMBUser currentUser = NCMBUser.CurrentUser;
        //user.signUp(id, pass, FailureCallback);
    }

    /// <summary>
    /// サインイン失敗時のコールバック
    /// </summary>
    public void FailureSignUp()
    {
        
        if (!caution_txt.activeSelf)
        {
            caution_txt.SetActive(true);
        }
    }

    protected override void OnEnable()
    {
        FailureCallback = FailureSignUp;
    }

}
