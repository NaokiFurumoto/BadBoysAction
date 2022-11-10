using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
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
    /// Unityデバッグ用
    /// </summary>
    [SerializeField]
    private bool IsUseSaveSystem = true;

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
    private NewGenerateManager generatorManager;

    /// <summary>
    /// 開始時Fadeinのコールバック
    /// </summary>
    private UnityAction startFadeinCallBack;

    #region プロパティ
    public INGAME_STATE State
    {
        get { return state; }
        set { state = value; }
    }
    #endregion

    void Start()
    {
        Initialize();
        InitializeView();
        
        //ロード処理
        StartCoroutine("LoadingGameInfo");
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
                                            GetComponent<NewGenerateManager>();

        startFadeinCallBack = PlayInGame;
    }

    /// <summary>
    /// インゲーム開始
    /// </summary>
    public void PlayInGame()
    {
        //スタミナチェック
        if (uiController.StaminasManager.IsCheckRecovery())
        {
            //スタミナを使用して再開
            uiController.StaminasManager.UseStamina();

            GameStart();
        }
        else
        {
            //スタミナ確認ダイアログ表示
            var dialog =
                CommonDialog.ShowDialog
                (
                    STAMINA_LESS_TITLE,
                    STAMINA_LESS_DESC,
                    MOVIECHECK,
                    CLOSE,

                    () => UnityAdsManager.Instance.ShowRewarded(result =>
                    {
                        //スタミナ全回復
                        if (result == ShowResult.Finished)
                        {
                            StaminasManager.Instance.FullRecovery(true,() => { CommonDialogManager.Instance.DeleteDialogAll();
                                                                               GameStart();
                            });
                        }
                    }
                    ),

                    () =>//スタミナが回復したら使用してゲーム開始。回復していなかったらタイトルに戻る
                    {
                        if (StaminasManager.Instance.IsCheckRecovery())
                        {
                            StaminasManager.Instance.UseStamina();
                            CommonDialogManager.Instance.DeleteDialogAll();
                            GameStart();
                        }
                        else
                        {
                            //タイトルに戻る
                        }
                    }

                 );

            //リストに追加
            CommonDialogManager.Instance.AddList(dialog);
        }

    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void GameStart()
    {
        //スタミナを必ず使用してる状態なのでテキストは表示させる
        StaminasManager.Instance.ActiveTextRecovery(true);
        //スコアの初期化
        uiController.UpdateScore();

        //敵生成
        generatorManager.StartGenerate();

        state = INGAME_STATE.START;
        startView.gameObject.SetActive(true);
        Invoke("SetPlayGame", START_PLAYINGTIME);
    }

    /// <summary>
    /// プレイ中設定
    /// </summary>
    public void SetPlayGame()
    {
        state = INGAME_STATE.PLAYING;
    }

    /// <summary>
    /// 一時停止
    /// </summary>
    public void GameStop()
    {
        state = INGAME_STATE.STOP;
        TimeManager.Instance.SetSlow(STOP_TIME, 0.0f);
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    public void GameResult()
    {
        //プレイ回数追加
        uiController.AddPlayTime();

        //停止処理
        TimeManager.Instance.SetSlow(STOP_TIME, 0.0f);
        state = INGAME_STATE.RESULT;

        //セーブする
        SavegameInfo();

        //このタイミングで広告表示。２回に１回広告表示：
        if(uiController.PlayTime % 2  == 0)
        {
           //広告終了後のコールバック
            Action<ShowResult> call = (result) =>
            {
                //ゲームリザルト画面を表示
                uiController.SetIsGameOver(true);
                gameOverView.gameObject.SetActive(true);
            };

            UnityAdsManager.Instance.ShowInterstitial(call);

        }
        else
        {
            //ゲームリザルト画面を表示
            uiController.SetIsGameOver(true);
            gameOverView.gameObject.SetActive(true);
        }
        
    }

    /// <summary>
    /// 再開
    /// </summary>
    public void GameResume()
    {
        state = INGAME_STATE.PLAYING;
        TimeManager.Instance.ResetSlow();
    }

    /// <summary>
    /// リトライ
    /// </summary>
    public void RetryGame()
    {
        //セーブ処理
        SavegameInfo();

        TimeManager.Instance.ResetSlow();
        GameStart();
    }

    /// <summary>
    /// ステータスの更新
    /// </summary>
    private void RetryStatus()
    {
        playerStatusController?.RetryPlayer();
        generatorManager?.RetryGenerator();
        uiController.UpdateScore();
    }

    /// <summary>
    /// 初回ロード処理
    /// </summary>
    private IEnumerator LoadingGameInfo()
    {
        if (!IsUseSaveSystem)
            yield break;

        yield return null;

        SaveData loadData;
        do
        {
            loadData = SaveManager.Instance.Load();
        }
        while (loadData == null);

        {
            uiController.SetLoadingKillsNumber(loadData.KillsNumber);
            uiController.SetHiScore(loadData.HiScoreNumber);
            uiController.SetGameLevel(loadData.GemeLevel);
            uiController.SetPlayTime(loadData.PlayTime);
            uiController.SetStamina(loadData.StaminaNumber);
            uiController.SetLife(loadData.LifeNumber);
        }

        //表示更新：中断処理分け行う
        uiController.UpdateScore();

        //暗転解除後にゲーム開始
        FadeFilter.Instance.FadeIn(Color.black, FADETIME, startFadeinCallBack);
    }

    /// <summary>
    /// セーブ処理
    /// GameOver後/
    /// </summary>
    public void SavegameInfo()
    {
        if (!IsUseSaveSystem)
            return;

        SaveData saveData = new SaveData();

        {
            //saveData.IsGameOver = uiController.GetIsGameOver();
            saveData.StaminaNumber = uiController.GetStamina();
            saveData.KillsNumber = uiController.GetKillsNumber();
            saveData.HiScoreNumber = uiController.GetHiScore();
            saveData.LifeNumber = uiController.GetLifeNum();
            saveData.GemeLevel = uiController.GetGameLevel();
            saveData.PlayTime = uiController.GetPlayTime();
        }

        SaveManager.Instance.Save(saveData);
    }

}

