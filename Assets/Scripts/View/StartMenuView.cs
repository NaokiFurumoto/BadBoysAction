using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    /// ロードデータ
    /// </summary>
    private SaveData loadingData;

    /// <summary>
    /// セーブクリアコールバック
    /// </summary>
    private UnityAction saveDataClearCallback;

    /// <summary>
    /// ダイアログ
    /// </summary>
    private CommonDialog dialog;

    /// <summary>
    /// 画面有効時
    /// </summary>
    protected override void OnEnable()
    {
        loadingData = SaveManager.Instance.Load();
        slider_BGM.SetValue();
        slider_SE.SetValue();

        saveDataClearCallback = SaveDataClearCallback;
        SetSliderVolume();
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
        dialog = CommonDialog.ShowDialog(SAVEDATA_CLEAR_TITlE, SAVEDATA_CLEAR_DESC, YES, NO, saveDataClearCallback, null);
    }

    /// <summary>
    /// セーブデータクリア
    /// </summary>
    private void SaveDataClearCallback()
    {
        var data = SaveManager.Instance.ChangeCleartDate(loadingData);
        SaveManager.Instance.Save(data);

        slider_BGM.SetPosition(new Vector2(0.5f, 0.0f));
        slider_SE.SetPosition(new Vector2(0.5f, 0.0f));
        Destroy(dialog.gameObject);
    }

}
