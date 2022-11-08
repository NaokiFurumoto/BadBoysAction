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
using Cysharp.Threading.Tasks;
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
        menu.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        EnableTap().Forget();
    }

    /// <summary>
    /// タップボタンの有効
    /// </summary>
    /// <returns></returns>
    private async UniTask EnableTap()
    {
        await UniTask.Delay(1000);
        FadeFilter.Instance.FadeIn(Color.black, 1.0f);
        await UniTask.Delay(2000);
        menu.SetActive(true);
        await UniTask.Delay(500);
        btnBG.interactable = true;
    }

    /// <summary>
    /// GameSceneに遷移する
    /// </summary>
    public void OnClickTapBG()
    {

        GoGameScene().Forget();
        
    }

    private async UniTask GoGameScene()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        await UniTask.Delay(1000);
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
        menu.gameObject.SetActive(true);
    }

    /// <summary>
    /// メニュー画面の開閉
    /// </summary>
    /// <param name="isActive"></param>
    public void StartMenuViewActivate(bool isActive)
    {
        startMenuView?.SetActive(isActive);
    }
}
