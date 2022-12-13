using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NCMB;
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
    private GameObject success_txt;

    [SerializeField]
    private Button btn_SighUp;

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
        caution_txt.SetActive(false);
        success_txt.SetActive(false);
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
    /// �T�C���A�b�v
    /// </summary>
    public void OnSignUp()
    {
        if (user == null)
            return;
        user.signUp(id, pass, FailureCallback, SuccessCallback);
    }

    /// <summary>
    /// �T�C���C�����s���̃R�[���o�b�N
    /// </summary>
    public void FailureSignUp()
    {
        if (!caution_txt.activeSelf)
        {
            caution_txt.SetActive(true);
            success_txt.SetActive(false);
        }
    }

    /// <summary>
    /// �T�C���A�b�v�������̃R�[���o�b�N
    /// </summary>
    public void SuccessSignUp()
    {

        if (!success_txt.activeSelf)
        {
            caution_txt.SetActive(false);
            success_txt.SetActive(true);
        }
    }

   
}
