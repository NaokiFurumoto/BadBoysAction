using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;
using System.Threading;
/// <summary>
/// スタート画面に関する対応
/// </summary>
public class StartScene : MonoBehaviour
{
    /// <summary>
    /// タップオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Button btnBG;

    [SerializeField]
    private GameObject startMenuView;


    void Awake()
    {
        btnBG.interactable = false;
        Time.timeScale = 1.0f;
        menu.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        //ここでデータのロード
        var data = SaveManager.Instance.Load();
        if (data.IsBreak && data.StaminaNumber != 0)
        {
            //戦闘中！！
            //中断された戦闘データがあります。再開しますか？
            //再開する、最初から
            //ダイアログを表示
            var dialog = CommonDialog.ShowDialog
                (
                    LOADBATTLE_TITLE,
                    LOADBATTLE_DESC,
                    LOADBATTLE_YES,
                    LOADBATTLE_NO,
                    () => OnClickTapBG(),
                    () => StartCoroutine("InitDataStart")
                );
            yield break;
        }

        yield return null;
        StartCoroutine("EnableTap");
    }


    /// <summary>
    /// タップボタンの有効
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableTap()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        FadeFilter.Instance.FadeIn(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(2.0f);
        menu.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        btnBG.interactable = true;
    }

   
    /// <summary>
    /// GameSceneに遷移する
    /// </summary>
    public void OnClickTapBG()
    {
        StartCoroutine("GoGameScene");
    }


    private IEnumerator GoGameScene()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        LoadScene.Load("GameScene");
    }

    /// <summary>
    /// ランキング表示
    /// </summary>
    public void OnClickRank()
    {
        Debug.Log("開始");
    }

    /// <summary>
    /// メニュ－表示
    /// </summary>
    public void OnClickMenu()
    {
        StartMenuViewActivate(true);
    }

    /// <summary>
    /// メニュー画面の開閉
    /// </summary>
    /// <param name="isActive"></param>
    public void StartMenuViewActivate(bool isActive)
    {
        startMenuView?.SetActive(isActive);
    }

    //初期データで更新
    private IEnumerator InitDataStart()
    {
        var loadData = SaveManager.Instance.Load();
        loadData.IsBreak = false;

        SaveManager.Instance.Save(loadData);

        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        var dloadData = SaveManager.Instance.Load();
        LoadScene.Load("GameScene");
    }
}
