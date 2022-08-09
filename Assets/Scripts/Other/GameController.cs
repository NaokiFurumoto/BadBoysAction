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
    NONE,//未設定
    START,//開始
    PLAYING,//ゲーム中
    STOP,//ストップ
    RESULT,//結果
}
public class GameController : MonoBehaviour
{
    /// <summary>
    /// ゲーム中の状態
    /// </summary>
    [SerializeField]
    private INGAME_STATE state;
    private INGAME_STATE ss;

    /// <summary>
    /// スタート画面
    /// </summary>
    [SerializeField]
    private GameObject startView;

    public INGAME_STATE State => state;

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

    private void PlayGame()
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
    /// 一時停止
    /// </summary>
    public void GameResume()
    {
        state = INGAME_STATE.PLAYING;
        Time.timeScale = 1;
    }

    /// <summary>
    /// 開始アニメーション終了後に呼ばれる
    /// </summary>
    public void DisableStartView()
    {
        startView.gameObject.SetActive(false);
    }
}