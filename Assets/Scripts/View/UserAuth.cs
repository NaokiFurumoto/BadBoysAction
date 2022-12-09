using UnityEngine;
using System.Collections;
//using NCMB;
using System.Collections.Generic;
using System;

public class UserAuth : MonoBehaviour
{
    ///// <summary> プレイヤー名 <summary>
    //[SerializeField]
    //private string currentPlayerName;

    //[SerializeField]
    //private bool isSighUp = false;

    //public bool IsSignUp { get { return isSighUp; } set { isSighUp = value; } }

    //// mobile backendに接続してログイン ------------------------
    //public void logIn(string id, string pw)
    //{

    //    NCMBUser.LogInAsync(id, pw, (NCMBException e) => {
    //        // 接続成功したら
    //        if (e == null)
    //        {
    //            currentPlayerName = id;
    //        }
    //    });
    //}

    //// mobile backendに接続して新規会員登録 ------------------------
    //public void signUp(string id,  string pw, Action callback)
    //{

    //    NCMBUser user = new NCMBUser();
    //    user.UserName = id;
    //    user.Password = pw;
    //    user.SignUpAsync((NCMBException e) => {

    //        if (e == null)
    //        {
    //            //サインアップ完了している
    //            isSighUp = true;
    //            //サインアップ情報を保存
    //            currentPlayerName = id;
    //        }
    //        else
    //        {
    //            callback();
    //        }
    //    });
    //}

    //// mobile backendに接続してログアウト ------------------------
    //public void logOut()
    //{

    //    NCMBUser.LogOutAsync((NCMBException e) => {
    //        if (e == null)
    //        {
    //            currentPlayerName = null;
    //        }
    //    });
    //}

    //// 現在のプレイヤー名を返す --------------------
    //public string currentPlayer() => currentPlayerName;
}
