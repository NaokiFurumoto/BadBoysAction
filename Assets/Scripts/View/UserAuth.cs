using UnityEngine;
using System.Collections;
//using NCMB;
using System.Collections.Generic;
using System;

public class UserAuth : MonoBehaviour
{
    ///// <summary> �v���C���[�� <summary>
    //[SerializeField]
    //private string currentPlayerName;

    //[SerializeField]
    //private bool isSighUp = false;

    //public bool IsSignUp { get { return isSighUp; } set { isSighUp = value; } }

    //// mobile backend�ɐڑ����ă��O�C�� ------------------------
    //public void logIn(string id, string pw)
    //{

    //    NCMBUser.LogInAsync(id, pw, (NCMBException e) => {
    //        // �ڑ�����������
    //        if (e == null)
    //        {
    //            currentPlayerName = id;
    //        }
    //    });
    //}

    //// mobile backend�ɐڑ����ĐV�K����o�^ ------------------------
    //public void signUp(string id,  string pw, Action callback)
    //{

    //    NCMBUser user = new NCMBUser();
    //    user.UserName = id;
    //    user.Password = pw;
    //    user.SignUpAsync((NCMBException e) => {

    //        if (e == null)
    //        {
    //            //�T�C���A�b�v�������Ă���
    //            isSighUp = true;
    //            //�T�C���A�b�v����ۑ�
    //            currentPlayerName = id;
    //        }
    //        else
    //        {
    //            callback();
    //        }
    //    });
    //}

    //// mobile backend�ɐڑ����ă��O�A�E�g ------------------------
    //public void logOut()
    //{

    //    NCMBUser.LogOutAsync((NCMBException e) => {
    //        if (e == null)
    //        {
    //            currentPlayerName = null;
    //        }
    //    });
    //}

    //// ���݂̃v���C���[����Ԃ� --------------------
    //public string currentPlayer() => currentPlayerName;
}
