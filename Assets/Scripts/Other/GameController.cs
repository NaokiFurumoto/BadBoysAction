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
    NONE    = 0,//未設定
    START   = 1,//開始
    PLAYING = 2,//ゲーム中
    STOP    = 3,//ストップ
    RESULT  = 4,//結果
}
public partial class GameController : MonoBehaviour
{
    /// <summary>
    /// ゲーム中の状態
    /// </summary>
    [SerializeField]
    private INGAME_STATE state;

    /// <summary>
    /// UI管理クラス
    /// </summary>
    private UiController uiController;

    /// <summary>
    /// プレイヤーの制御
    /// </summary>
    private PlayerStatusController playerStatusController;

    /// <summary>
    /// プレイヤーの制御
    /// </summary>
    private GeneratorManager generatorManager;

    public INGAME_STATE State
    {
        get { return state; }
        set { state = value; }
    }

    void Start()
    {
        Initialize();
        InitializeView();
        GameStart();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        state = INGAME_STATE.NONE;

        uiController = GameObject.FindGameObjectWithTag("UI").
                                     GetComponent<UiController>();

        playerStatusController = GameObject.FindGameObjectWithTag("Player").
                                            GetComponent<PlayerStatusController>();

        generatorManager = GameObject.FindGameObjectWithTag("GeneratorRoot").
                                            GetComponent<GeneratorManager>();
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

    /// <summary>
    /// プレイ中設定
    /// </summary>
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
        Time.timeScale = 0;
        state = INGAME_STATE.RESULT;
        gameOverView.gameObject.SetActive(true);
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
    /// リトライ
    /// </summary>
    public void RetryGame()
    {
        fadeView.gameObject.SetActive(false);
        Time.timeScale = 1;
        GameStart();
    }

    private void RetryStatus()
    {
        playerStatusController?.RetryPlayer();
        generatorManager?.RetryGenerator();
        uiController.RetryUI();
    }
}

