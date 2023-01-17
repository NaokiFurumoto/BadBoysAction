using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using static GlobalValue;
/// <summary>
/// スタートシーンのメニュー画面に関する処理
/// </summary>
public class StartMenuView : ViewBase
{
    /// <summary>
    /// BGMスライダー
    /// </summary>
    [SerializeField]
    private MenuObject_Sliderbar slider_BGM;

    /// <summary>
    /// SEスライダー
    /// </summary>
    [SerializeField]
    private MenuObject_Sliderbar slider_SE;

    /// <summary>
    /// ランキングボタン
    /// </summary>
    [SerializeField]
    private Button rankBtn;


    /// <summary>
    /// ロードデータ
    /// </summary>
    private SaveData loadingData;

    /// <summary>
    /// セーブクリアコールバック
    /// </summary>
    private UnityAction saveDataClearCallback;

    /// <summary>
    /// ADS購入コールバック
    /// </summary>
    private UnityAction adsBuyCallback;

    /// <summary>
    /// ダイアログ
    /// </summary>
    private CommonDialog dialog;

    /// <summary>
    /// キャッシュ用
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;

    /// <summary>
    /// 画面有効時
    /// </summary>
    protected override void OnEnable()
    {
        loadingData = SaveManager.Instance.Load();
        slider_BGM.SetValue();
        slider_SE.SetValue();

        saveDataClearCallback = SaveDataClearCallback;
        adsBuyCallback = OnClickAds;
        SetSliderVolume();
    }

    private void Start()
    {
        FM = SoundManager.Instance;
        appSound = AppSound.Instance;
    }

    protected override void OnDisable()
    {
        //データの成形
        loadingData.BGM_Volume = slider_BGM.CurosorPosition.x;
        loadingData.SE_Volume = slider_SE.CurosorPosition.x;

        //セーブする
        SaveManager.Instance.Save(loadingData);

        SetInitialize(false);
    }

    /// <summary>
    /// スライドバーの初期化
    /// ロードした値をセットする
    /// </summary>
    void SetSliderVolume()
    {
        slider_BGM.SetPosition(new Vector2(loadingData.BGM_Volume, 0.0f));
        slider_SE.SetPosition(new Vector2(loadingData.SE_Volume, 0.0f));

        SetInitialize(true);
    }

    /// <summary>
    /// スライダーの初期化状態設定
    /// </summary>
    /// <param name="isInit"></param>
    private void SetInitialize(bool isInit)
    {
        slider_BGM.Initialized = isInit;
        slider_SE.Initialized = isInit;
    }

    /// <summary>
    /// 閉じる
    /// </summary>
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// セーブデータダイアログの表示
    /// </summary>
    public void OpenSaveDataDialog()
    {
        AppSound.Instance.SE_MENU_OK.Play();
        dialog = CommonDialog.ShowDialog(SAVEDATA_CLEAR_TITlE, SAVEDATA_CLEAR_DESC, YES, NO, saveDataClearCallback, null);
    }

    /// <summary>
    /// 課金処理
    /// </summary>
    public void OnClickAds()
    {
        AppSound.Instance.SE_MENU_OK.Play();
        dialog = CommonDialog.ShowDialog(ADS_BUY_TITLE, ADS_BUY_DESC, YES, NO, adsBuyCallback, null);
    }

    /// <summary>
    /// セーブデータクリア
    /// </summary>
    private void SaveDataClearCallback()
    {
        var data = SaveManager.Instance.ChangeCleartDate(loadingData);
        SaveManager.Instance.Save(data);

        rankBtn.interactable = false;
        slider_BGM.SetPosition(new Vector2(0.5f, 0.0f));
        slider_SE.SetPosition(new Vector2(0.5f, 0.0f));
        Destroy(dialog.gameObject);
    }

    /// <summary>
    /// スライダーの更新時に呼ばれる
    /// </summary>
    public void SlidebarDrag()
    {
        //SE再生
        //FM.PlayOneShot(appSound.SE_SLIDE_CHANGE);
        //SE設定
        FM.SetVolume("BGM", slider_BGM.CurosorPosition.x);
        FM.SetVolume("SE", slider_SE.CurosorPosition.x);
    }

}
