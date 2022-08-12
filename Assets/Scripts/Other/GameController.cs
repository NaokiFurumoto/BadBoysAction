using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GlobalValue;
using System;

/// <summary>
/// インゲームの状態管理
/// </summary>
//ゲーム状態
public enum INGAME_STATE
{
    NONE = 0,//未設定
    START = 1,//開始
    PLAYING = 2,//ゲーム中
    STOP = 3,//ストップ
    RESULT = 4,//結果
}
public class GameController : MonoBehaviour
{
    /// <summary>
    /// ゲーム中の状態
    /// </summary>
    [SerializeField]
    private INGAME_STATE state;

    /// <summary>
    /// スタート画面
    /// </summary>
    [SerializeField]
    private GameObject startView;

    /// <summary>
    /// オプション画面
    /// </summary>
    [SerializeField]
    private GameObject optionView;

    public INGAME_STATE State
    {
        get { return state; }
        set { state = value; }
    }

    void Start()
    {
        Initialize();
        GameStart();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        state = INGAME_STATE.NONE;
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void GameStart()
    {
        state = INGAME_STATE.START;
        startView.gameObject.SetActive(true);
        Invoke("PlayGame", START_PLAYINGTIME);
    }

    public void PlayGame()
    {
        state = INGAME_STATE.PLAYING;
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public void GameStop()
    {
        state = INGAME_STATE.STOP;
        Time.timeScale = 0;
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameResult()
    {
        state = INGAME_STATE.RESULT;
        //停止処理
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void GameResume()
    {
        state = INGAME_STATE.PLAYING;
        Time.timeScale = 1;
    }

    /// <summary>
    /// Viewの非表示
    /// </summary>
    public void DisableView(ViewBase _view)
    {
        _view.gameObject.SetActive(false);
    }

    /// <summary>
    /// Viewの表示
    /// </summary>
    public void EnableView(ViewBase _view)
    {
        _view.gameObject.SetActive(true);
    }

    #region View
    /// <summary>
    /// オプションボタンが押されたときに呼ぶ
    /// </summary>
    public void OnClickOptionButton()
    {
        if (State == INGAME_STATE.STOP)
        {
            //閉じる
            GameResume();
            optionView.SetActive(false);
        }
        else if(State == INGAME_STATE.PLAYING)
        {
            //開く
            GameStop();
            optionView.SetActive(true);
        }
    }
    #endregion
}