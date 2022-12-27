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

    /// <summary>
    /// キャッシュ用
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;

    protected override void OnEnable() { }

    private void Start()
    {
        FM = SoundManager.Instance;
        appSound = AppSound.Instance;
    }

    public void OnClickCloseBtn()
    {
        FM.PlayOneShot(appSound.SE_MENU_CANCEL);
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
