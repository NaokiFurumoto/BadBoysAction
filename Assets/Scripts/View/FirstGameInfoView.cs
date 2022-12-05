using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FirstGameInfoView : ViewBase
{
    [SerializeField]
    private Toggle toggle;

    public UnityAction CallBack;

    protected override void OnEnable() { }

    public void OnClickCloseBtn()
    {
        //チェックボックスデータの保存
        var loadData = SaveManager.Instance.Load();
        gameController.IsOpenFirstview =
        loadData.IsFirstViewOpen = toggle.isOn;

        //セーブする
        SaveManager.Instance.Save(loadData);
        CallBack?.Invoke();

        this.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
