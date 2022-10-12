using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static GlobalValue;

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

    /// <summary>
    /// ダイアログが開いた後に呼ばれる関数
    /// </summary>
    public void EndAnimationCallBack()
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

    public static StaminaRicoveryDialog ShowDialog()
    {
        if(prefab == null)
        {
            prefab = Resources.Load(STAMINADIALOG_PREFAB_NAME) as GameObject;
        }

        var instance = Instantiate(prefab);
        var dialog = instance?.GetComponent<StaminaRicoveryDialog>();
        var rootParent = GameObject.FindGameObjectWithTag("DialogRoot").transform;
        if(rootParent != null)
        {
            instance.transform.parent = rootParent;
        }

        dialog.closeButton.onClick.AddListener(() => Destroy(dialog.gameObject));
        //dialog.okButton.onClick.AddListener();

        return dialog;
    }

    //広告処理：Unity Ads//https://www.youtube.com/watch?v=FWSQaTDnS0o


}
