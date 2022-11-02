using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Advertisements;
using static GlobalValue;
using System;

/// <summary>
/// スタミナ回復ダイアログ
/// 作成したprefabを読み込んで表示
/// </summary>
public class StaminaRicoveryDialog : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Button okButton;

    [SerializeField]
    private Button closeBackButton;

    //delegate
    public UnityAction OnDestroyedCallBack;

    private static GameObject prefab;

    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1);
        OpenDialogCallBack();
    }

    /// <summary>
    /// ダイアログが開いた後に呼ばれる関数
    /// </summary>
    public void OpenDialogCallBack()
    {
        //バックボタンの有効
        var eventTrigger = closeBackButton?.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(eventData => { Destroy(this.gameObject); });
        eventTrigger.triggers.Add(entry);
    }

    private void OnDestroy()
    {
        OnDestroyedCallBack?.Invoke();
    }

    public static void ShowDialog()
    {
        if(prefab == null)
        {
            prefab = Resources.Load(STAMINADIALOG_PREFAB_NAME) as GameObject;
        }

        var rootParent = GameObject.FindGameObjectWithTag("DialogRoot").transform;
        var instance = Instantiate(prefab, Vector3.zero, Quaternion.identity, rootParent);
        var dialog = instance?.GetComponent<StaminaRicoveryDialog>();

        dialog.closeButton.onClick.AddListener(() => Destroy(dialog.gameObject));
        dialog.okButton.onClick.AddListener(() => 
                UnityAdsManager.Instance.ShowRewarded( result => 
                {
                    //スタミナ全回復
                    if(result == ShowResult.Finished)
                    {
                        StaminasManager.Instance.FullRecovery( true );
                    }
                }));
    }
   
}
