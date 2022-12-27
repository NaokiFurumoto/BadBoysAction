﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static GlobalValue;
using System;

public class CommonDialog : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txt_Title;

    [SerializeField]
    private TextMeshProUGUI txt_Desc;

    //[SerializeField]
    //private Button closeBackButton;

    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Button okButton;

    //単純にvoid delegateの事。１行でできる。
    public UnityAction onDestroyed;

    private static GameObject prefab;

   
    private void OnDestroy()
    {
        onDestroyed?.Invoke();
    }

    /// <summary>
    /// ダイアログを作成して表示
    /// </summary>
    /// <param name="title">タイトルテキスト</param>
    /// <param name="desc">文言テキスト</param>
    /// <param name="ok">OKボタン名</param>
    /// <param name="ng">NGボタン名</param>
    /// <param name="okCallBack">コールバック</param>
    /// <returns></returns>
    public static CommonDialog ShowDialog(
        string title,
        string desc,
        string ok = null,
        string ng = null,
        UnityAction okCallBack = null,
        UnityAction ngCallBack = null
    )
    {
        if(prefab == null)
        {
            prefab = Resources.Load(COMMONDIALOG_PREFAB_NAME) as GameObject;
        }

        var rootParent = GameObject.FindGameObjectWithTag("DialogRoot").transform;
        var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity, rootParent);
        var comDialog = instance.GetComponent<CommonDialog>();

        comDialog.txt_Title.text = title;
        comDialog.txt_Desc.text = desc;

        ///
        if (string.IsNullOrEmpty(ok))
        {
            Destroy(comDialog.okButton.gameObject);
            comDialog.okButton = null;
        }
        else
        {
            comDialog.okButton.GetComponentInChildren<TextMeshProUGUI>().text = ok;
            comDialog.okButton.onClick.AddListener(()=> { AppSound.Instance.SE_MENU_OK.Play(); 
                                                          okCallBack(); 
                                                        });
        }

        ///
        if (string.IsNullOrEmpty(ng))
        {
            Destroy(comDialog.okButton.gameObject);
            comDialog.okButton = null;
        }
        else
        {
            comDialog.closeButton.GetComponentInChildren<TextMeshProUGUI>().text = ng;
            if(ngCallBack == null)
            {
                comDialog.closeButton.onClick.AddListener(() => { AppSound.Instance.SE_MENU_CANCEL.Play(); 
                                                                  Destroy(comDialog.gameObject);
                                                                });
            }
            else
            {
                comDialog.closeButton.onClick.AddListener(()=> { AppSound.Instance.SE_MENU_CANCEL.Play(); 
                                                                 ngCallBack(); 
                                                               });
            }
        }


        okCallBack ??= () =>
        {
            AppSound.Instance.SE_MENU_OK.Play();
            Destroy(comDialog.gameObject);
        };

        return comDialog;
    }

}
