using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static GlobalValue;

/// <summary>
/// �X�^�~�i�񕜃_�C�A���O
/// �쐬����prefab��ǂݍ���ŕ\��
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
    /// �_�C�A���O���J������ɌĂ΂��֐�
    /// </summary>
    public void EndAnimationCallBack()
    {
        //�o�b�N�{�^���̗L��
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

    //�L�������FUnity Ads//https://www.youtube.com/watch?v=FWSQaTDnS0o


}
