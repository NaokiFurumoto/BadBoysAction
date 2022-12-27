using UnityEngine;
using System.Collections;
using NCMB;
using System.Collections.Generic;
using System;

public class UserAuth : Singleton<UserAuth>
{
    /// <summary> プレイヤー名 <summary>
    [SerializeField]
    private string currentPlayerName;

    /// <summary> パスワード <summary>
    [SerializeField]
    private string currentPassward;

    [SerializeField]
    private bool isSighUp = false;

    [SerializeField]
    private bool isLogin = false;

    public bool IsSignUp { get { return isSighUp; } set { isSighUp = value; } }
    public bool IsLogin { get { return isLogin; } set { isLogin = value; } }
    // 現在のプレイヤー名を返す --------------------
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

    // mobile backendに接続してログイン ------------------------
    public void logIn(string id, string pw, Action failCallback = null, Action successCallback = null)
    {
        var loadData = SaveManager.Instance.Load();
        NCMBUser.LogInAsync(id, pw, (NCMBException e) =>
        {
            // 接続成功したら
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
                //ログイン失敗コールバック
                if (failCallback != null)
                {
                    failCallback();
                }
            }
        });
    }

    // mobile backendに接続して新規会員登録 ------------------------
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
                //サインアップ完了している
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

    // mobile backendに接続してログアウト ------------------------
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
