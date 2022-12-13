using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;
using System;

public class UserAuth : MonoBehaviour
{
    /// <summary> �v���C���[�� <summary>
    [SerializeField]
    private string currentPlayerName;

    /// <summary> �p�X���[�h <summary>
    [SerializeField]
    private string currentPassward;

    [SerializeField]
    private bool isSighUp = false;

    [SerializeField]
    private bool isLogin = false;

    public bool IsSignUp { get { return isSighUp; } set { isSighUp = value; } }
    public bool IsLogin { get { return isLogin; } set { isLogin = value; } }
    // ���݂̃v���C���[����Ԃ� --------------------
    public string CurrentPlayer
    {
        get { return currentPlayerName; }
        set { currentPlayerName = value; }
    }

    public string CurrentPassward
    {
        get { return currentPassward; }
        set { currentPassward = value; }
    }

    // mobile backend�ɐڑ����ă��O�C�� ------------------------
    public void logIn(string id, string pw, Action failCallback = null, Action successCallback = null)
    {
        var loadData = SaveManager.Instance.Load();
        NCMBUser.LogInAsync(id, pw, (NCMBException e) =>
        {
            // �ڑ�����������
            if (e == null)
            {
                currentPlayerName = id;
                currentPassward = pw;
                isLogin = true;
                if (successCallback != null)
                {
                    successCallback();
                }

                loadData.IsLogin = true;
                loadData.UserName = id;
                loadData.Passward = pw;
                SaveManager.Instance.Save(loadData);
            }
            else
            {
                //���O�C�����s�R�[���o�b�N
                if (failCallback != null)
                {
                    failCallback();
                }
            }
        });
    }

    // mobile backend�ɐڑ����ĐV�K����o�^ ------------------------
    public void signUp(string id, string pw, Action failCallback = null, Action successCallback = null)
    {
        NCMBUser user = new NCMBUser();
        user.UserName = id;
        user.Password = pw;
        user.SignUpAsync((NCMBException e) =>
        {
            var loadData = SaveManager.Instance.Load();
            if (e == null)
            {
                //�T�C���A�b�v�������Ă���
                isSighUp = true;
                if (successCallback != null)
                {
                    successCallback();
                }
                loadData.IsSighin = true;
                SaveManager.Instance.Save(loadData);
            }
            else
            {
                if (failCallback != null)
                {
                    failCallback();
                }
            }
        });
    }

    // mobile backend�ɐڑ����ă��O�A�E�g ------------------------
    public void logOut(Action failCallback = null, Action successCallback = null)
    {
        NCMBUser.LogOutAsync((NCMBException e) =>
        {
            if (e == null)
            {
                currentPlayerName = "";
                currentPassward = "";
                isLogin = false;
                successCallback();
            }
            else
            {
                failCallback();
            }
        });
    }


}
