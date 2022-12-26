using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;
using NCMB;

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
    /// 敵の制御
    /// </summary>
    private NewEnemyGenerator enemyGenerator;

    /// <summary>
    /// キャッシュ用
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;

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
        SaveManager.Instance?.InitializeThis();

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

        startFadeinCallBack = OpenFirstView;

        enemyGenerator = GameObject.FindGameObjectWithTag("EnemyGenerator").
                                      GetComponent<NewEnemyGenerator>();
        FM = SoundManager.Instance;
        appSound = AppSound.Instance;
    }

    /// <summary>
    /// ロード処理
    /// </summary>
    private IEnumerator LoadingGameInfo()
    {
        //セーブシステム使うか：でバック用
        if (!IsUseSaveSystem)
            yield break;

        SaveData loadData;
        do
        {
            yield return null;
            loadData = SaveManager.Instance.Load();

        }
        while (loadData == null);
        
        //中断復帰かつ、リザルト表示じゃない、体力が0ではない
        if (loadData.IsBreak && loadData.gameState != INGAME_STATE.RESULT && loadData.LifeNumber != 0 )
        {
            SetStatus(loadData);
        }
        else
        {
            //リスタートデータの反映
            var changeData = SaveManager.Instance.ChangeRestartDate(loadData);
            SetStatus(changeData);
        }

        //暗転解除後にゲーム開始:PlayInGame()
        FadeFilter.Instance.FadeIn(Color.black, FADETIME, startFadeinCallBack);
    }

   

    /// <summary>
    /// インゲーム開始
    /// </summary>
    public void PlayInGame()
    {
        ///課金していたらスタミナをフル回復して、スタート
        if (uiController.GetIsAds())
        {
            StaminasManager.Instance.FullRecovery(false);
            GameStart();
            return;
        }

        //スタミナチェック
        if (StaminasManager.Instance.IsCheckRecovery())
        {
            var aa = uiController.GetIsBreak();
            if (!uiController.GetIsBreak())
            {
                //中断復帰じゃない場合はスタミナを使用する
                StaminasManager.Instance.UseStamina();
            }

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
                            StaminasManager.Instance.FullRecovery(true, () =>
                            {
                                CommonDialogManager.Instance.DeleteDialogAll();
                                GameResume();
                                GameStart();
                            });
                        }
                    }
                    ),

                    () =>//スタミナが回復したら使用してゲーム開始。回復していなかったらタイトルに戻る
                    {
                        if (StaminasManager.Instance.IsCheckRecovery())
                        {
                            //StaminasManager.Instance.UseStamina();
                            CommonDialogManager.Instance.DeleteDialogAll();
                            GameStart();
                        }
                        else
                        {
                            //タイトル画面に遷移
                            StartCoroutine("FadeTitle");
                        }
                    }
                 );

            //リストに追加
            CommonDialogManager.Instance.AddList(dialog);
        }
    }

    /// <summary>
    /// タイトル画面に遷移
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeTitle()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        GameResume();
        LoadScene.Load("StartScene");
    }

    /// <summary>
    /// ゲーム開始
    /// </summary>
    public void GameStart()
    {
        //スタミナを必ず使用してる状態なのでテキストは表示させる:そんなことない
        StaminasManager.Instance.ActiveTextRecovery(true);

        //敵生成
        generatorManager.StartGenerate();

        state = INGAME_STATE.START;
        FM.PlayOneShot(appSound.SE_GAMESTART);
        startView.gameObject.SetActive(true);
        Invoke("SetPlayGame", START_PLAYINGTIME);
    }

    /// <summary>
    /// プレイ中設定
    /// </summary>
    public void SetPlayGame()
    {
        //ロードVolumeに変更
        //FM.SetVolume(appSound.BGM_STAGE, 1.0f);
        appSound.BGM_STAGE.Play();
        FM.FadeInVolume(appSound.BGM_STAGE, FM.GetVolume(appSound.BGM_STAGE), 2.0f,true);
        appSound.BGM_STAGE.loop = true;
        
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
        //FM.FadeOutVolume(appSound.BGM_STAGE, 0.0f, 0.5f, false);
        //プレイ回数追加
        uiController.AddPlayTime();

        //停止処理
        TimeManager.Instance.SetSlow(STOP_TIME, 0.0f);
        state = INGAME_STATE.RESULT;

        //セーブする:
        SaveManager.Instance.GamePlaingSave();

        //このタイミングで広告表示。２回に１回広告表示：
        if (uiController.PlayTime % 2 == 0 && !uiController.GetIsAds())
        {
            //広告終了後のコールバック
            Action<ShowResult> call = (result) =>
            {
                //ゲームリザルト画面を表示
                uiController.SetIsBreak(false);
                gameOverView.gameObject.SetActive(true);
            };

            UnityAdsManager.Instance.ShowInterstitial(call);

        }
        else
        {
            //ゲームリザルト画面を表示
            uiController.SetIsBreak(false);
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
        ItemController.Instance.Retry();
        PlayerEffectManager.Instance.Retry();
    }

    

    /// <summary>
    /// ステータス設定
    /// </summary>
    /// <param name="loadData"></param>
    private void SetStatus(SaveData loadData)
    {
        IsOpenFirstview = loadData.IsFirstViewOpen;

        uiController.SetLoadingKillsNumber(loadData.KillsNumber);
        uiController.SetHiScore(loadData.HiScoreNumber);
        uiController.SetGameLevel(loadData.GemeLevel);
        uiController.SetPlayTime(loadData.PlayTime);
        uiController.SetStamina(loadData.StaminaNumber);
        uiController.SetLife(loadData.LifeNumber);
        uiController.SetIsBreak(loadData.IsBreak);
        uiController.SetIsAds(loadData.IsShowAds);
        uiController.SetLoadStamina(loadData.saveTime);

        generatorManager.GameLevel = loadData.GemeLevel;
        generatorManager.IsInterval = false;
        generatorManager.SetChangeKillCount(loadData.changeKillCount);
        generatorManager.SetLevelupNeedCount(loadData.levelupNeedCount);

        enemyGenerator.SetCreateDelayTime(loadData.createDelayTime);
        enemyGenerator.SetEnemyScreenDisplayIndex(loadData.enemyScreenDisplayIndex);
        enemyGenerator.SetLoadedEnemyEncounts(loadData.GemeLevel);
        UserAuth.Instance.CurrentPlayer = loadData.UserName;
        UserAuth.Instance.CurrentPassward = loadData.Passward;
        UserAuth.Instance.IsLogin = loadData.IsLogin;
        UserAuth.Instance.IsSignUp = loadData.IsSighin;

        //BGM/SEボリューム設定
        SoundManager.Instance.SetVolume("BGM", loadData.BGM_Volume);
        SoundManager.Instance.SetVolume("SE", loadData.SE_Volume);
        SoundManager.Instance.Bgm_SeVolume = (loadData.BGM_Volume, loadData.SE_Volume);
    }

}

