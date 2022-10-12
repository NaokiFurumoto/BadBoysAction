using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static GlobalValue;

public class CommonDialog : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI txt_Title;

    [SerializeField]
    private TextMeshProUGUI txt_Desc;

    [SerializeField]
    private Button closeBackButton;

    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Button okButton;


    [SerializeField]
    private Image backGround;

    //単純にvoid delegateの事。１行でできる。
    public UnityAction onDestroyed;

    private static GameObject prefab;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Start()
    {
        ///イベントの登録
        var eventTrigger = backGround.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(eventData => { Destroy(this.gameObject); });
        eventTrigger.triggers.Add(entry);
    }

    private void OnDestroy()
    {
        onDestroyed?.Invoke();
    }

    public static CommonDialog ShowDialog(
        string title,
        string desc,
        string ng = null,
        string ok = null
    )
    {
        if(prefab == null)
        {
            prefab = Resources.Load(COMMONDIALOG_PREFAB_NAME) as GameObject;
        }

        var instance = Instantiate(prefab);
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
            comDialog.okButton.onClick.AddListener(() => Destroy(comDialog.gameObject));
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
            comDialog.closeButton.onClick.AddListener(() => Destroy(comDialog.gameObject));
        }

        return comDialog;
    }
}
