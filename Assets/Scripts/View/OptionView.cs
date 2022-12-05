using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static GlobalValue;

public class OptionView : ViewBase
{
    [SerializeField]
    private GameObject BG;

    /// <summary>
    /// 初期化
    /// </summary>
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1);
        OpenDialogCallBack();
    }


    private void OpenDialogCallBack()
    {
        ///イベントの登録
        var eventTrigger = BG.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(eventData => { gameController?.OnClickOptionButton(); });
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// タイトル画面に移動
    /// </summary>
    public void OnClickTitleBtn()
    {
        //現在のバトルを終了してタイトルに戻ります。よろしいでしょうか？
        //スタミナ状態を保存
        CommonDialog.ShowDialog(OPTION_GOSTART_TITLE, OPTION_GOSTART_DESC, YES, NO, GoTitleCallback);
    }

    /// <summary>
    /// ランキング画面の表示
    /// </summary>
    public void OnClickRankBtn()
    {
    }

    /// <summary>
    /// タイトル画面に以降
    /// </summary>
    private void GoTitleCallback()
    {
        ////クリアデータの取得:リスタートデータの取得
        //var loadingData = SaveManager.Instance.GetClearSaveData();
        //if (loadingData == null)
        //    return;

        //SaveManager.Instance.Save(loadingData);
        StartCoroutine("FadeTitle");
    }

    private IEnumerator FadeTitle()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        gameController.GameResume();
        LoadScene.Load("StartScene");
    }

    protected override void OnEnable()
    {
    }

    protected override void OnDisable()
    {
    }


}
