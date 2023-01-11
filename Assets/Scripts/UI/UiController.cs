using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static GlobalValue;
/// <summary>
/// インゲームUIに関する操作/更新など
/// </summary>
public class UiController : MonoBehaviour
{
    /// <summary>
    /// ゲームレベル表示
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_gameLevel;

    /// <summary>
    /// レベル数
    /// </summary>
    [SerializeField]
    private int gameLevel;

    /// <summary>
    /// レベルMAXオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject levelMaxObject;

    /// <summary>
    /// ライフ管理
    /// </summary>
    [SerializeField]
    private LifesManager lifesManager;

    /// <summary>
    /// スタミナ管理
    /// </summary>
    [SerializeField]
    private StaminasManager staminasManager;

    /// <summary>
    /// 撃破数テキスト表示
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_Kills;

    /// <summary>
    /// 撃破数
    /// </summary>
    [SerializeField]
    private int killsNum;

    /// <summary>
    /// ハイスコア
    /// </summary>
    private int hiScore;

    /// <summary>
    /// ハイスコアテキスト表示
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_HiScore;

    /// <summary>
    /// 通常スタミナ残り表示
    /// </summary>
    [SerializeField]
    private GameObject NormalStTimes;

    /// <summary>
    /// 課金スタミナ表示
    /// </summary>
    [SerializeField]
    private GameObject adsStTimes;

    /// <summary>
    /// プレイ回数
    /// </summary>
    [SerializeField]
    private int playTime;

    /// <summary>
    /// レベルアップ演出
    /// </summary>
    [SerializeField]
    private GameObject levelUp;

    /// <summary>
    /// 中断復帰判定
    /// </summary>
    private bool isBreak;

    /// <summary>
    /// 課金判定
    /// </summary>
    private bool isAds;

    /// <summary>
    /// 生成器親管理クラス
    /// </summary>
    private NewGenerateManager generatorManager;

    /// <summary>
    /// 自身の初期化
    /// </summary>
    private void Awake() { }

    #region プロパティ
    public LifesManager LifesManager => lifesManager;
    public StaminasManager StaminasManager => staminasManager;
    public int KillsNUM { get { return killsNum; } set { killsNum = value; } }

    public int HiScore => hiScore;

    public int PlayTime { get { return playTime; } set { playTime = value; } }

    #endregion

    /// <summary>
    /// 外部初期化
    /// </summary>
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        generatorManager = GameObject.FindGameObjectWithTag("GeneratorRoot").
                                      GetComponent<NewGenerateManager>();
        isBreak = false;
    }

    /// <summary>
    /// 課金していたらスタミナ表示を切り替える
    /// </summary>
    /// <param name="isAds"></param>
    public void SwitchAdsStamina(bool isAds)
    {
        NormalStTimes.SetActive(!isAds);
        adsStTimes.SetActive(isAds);
    }

    /// <summary>
    /// 敵死亡時の撃破数の設定
    /// </summary>
    /// <param name="_number"></param>
    public void SetPlayKillsNumber()
    {
        if (text_Kills == null)
            return;
        killsNum++;
        text_Kills.text = killsNum.ToString();
        generatorManager?.ChangeUpdateGenerator();
    }


    /// <summary>
    /// 撃破数の取得
    /// </summary>
    public int GetTextKillsNumber()
    {
        int num;
        if (int.TryParse(text_Kills.text, out num))
            return num;

        return 0;
    }

    /// <summary>
    /// スコアのクリア
    /// </summary>
    public void UpdateScore()
    {
        killsNum = 0;
        text_Kills.text = "0";
        text_HiScore.text = hiScore.ToString();
        LifesManager.SetLife(1);
        SetGameLevel(1);
    }

    /// <summary>
    /// スコアのクリア
    /// </summary>
    public void UpdateLoadedScore(SaveData data)
    {
        //text_Kills.text = GetTextKillsNumber().ToString();
        //text_HiScore.text = hiScore.ToString();
        //SetGameLevel(GetGameLevel());
        //LifesManager.SetLife(GetLifeNum());
       // generatorManager.InitializeLoaded(data);
    }

    //-------------------------------Save・Load----------------------------------------//
    #region Load関連
    ////// <summary>
    /// 初回ロード後の撃破数設定
    /// </summary>
    /// <param name="_number"></param>
    public void SetLoadingKillsNumber(int num)
    {
        if (text_Kills == null)
            return;

        killsNum = num;
        text_Kills.text = killsNum.ToString();
    }

    /// <summary>
    /// ハイスコアの設定
    /// </summary>
    public void SetHiScore(int score)
    {
        hiScore = score;
        text_HiScore.text = hiScore.ToString();
    }

    /// <summary>
    /// ゲームレベル変更
    /// </summary>
    public void SetGameLevel(int level)
    {
        gameLevel = level;
        gameLevel = Mathf.Clamp(gameLevel, 1, MAX_GAMELEVEL);
        text_gameLevel.text = gameLevel.ToString();

        if (level >= MAX_GAMELEVEL)
        {
            text_gameLevel.gameObject.SetActive(false);
            levelMaxObject.gameObject.SetActive(true);
        }
        else
        {
            text_gameLevel.gameObject.SetActive(true);
            levelMaxObject.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// レベルアップ演出表示
    /// </summary>
    public void LevelUpEffect()
    {
        SoundManager.Instance.PlayOneShot(AppSound.Instance.SE_LEVELUP);
        levelUp.SetActive(true);
    }

    /// <summary>
    /// プレイ回数の設定
    /// </summary>
    /// <param name="playTime"></param>
    public void SetPlayTime(int playTime)
    {
        this.playTime = playTime;
    }

    /// <summary>
    /// プレイ回数追加
    /// </summary>
    /// <param name="playTime"></param>
    public void AddPlayTime()
    {
        this.playTime++;
    }

    /// <summary>
    /// スタミナ設定
    /// </summary>
    /// <param name="num"></param>
    public void SetStamina(int num)
    {
        if (num <= 0)
        {
            //スタミナを全て使用不可とする
            staminasManager.SetAllDisable();
        }
        else
        {
            staminasManager.SetStaminaNumber(num);
        }
    }

    /// <summary>
    /// スタミナ設定
    /// </summary>
    /// <param name="num"></param>
    public void SetLoadStamina(long loadedTimes)
    {
        staminasManager.loadRecoveryStaminas(loadedTimes);
    }

    /// <summary>
    /// ライフ設定
    /// </summary>
    /// <param name="num"></param>
    public void SetLife(int num)
    {
        if (num <= START_LIFEPOINT)
        {
            num = START_LIFEPOINT;
        }
        //ライフを数値で回復
        lifesManager.SetLife(num);
    }

    /// <summary>
    /// 中断復帰の設定
    /// </summary>
    /// <param name="num"></param>
    public void SetIsBreak(bool judge)
    {
        isBreak = judge;
    }

    /// <summary>
    /// 課金判定
    /// </summary>
    /// <param name="num"></param>
    public void SetIsAds(bool judge)
    {
        isAds = judge;
        SwitchAdsStamina(isAds);
    }
    #endregion

    #region Save関連
    /// <summary>
    /// 残りスタミナ数取得
    /// </summary>
    /// <param name="num"></param>
    public int GetStamina() => staminasManager.GetUseStaminaNumber();

    /// <summary>
    /// 撃破数取得
    /// </summary>
    /// <param name="num"></param>
    public int GetKillsNumber() => killsNum;

    /// <summary>
    /// ハイスコア取得
    /// </summary>
    /// <returns></returns>
    public int GetHiScore() => hiScore;

    /// <summary>
    /// ライフ数取得：途中用
    /// </summary>
    /// <returns></returns>
    public int GetLifeNum() => lifesManager.GetLifeNum();

    /// <summary>
    /// ゲームレベルの取得
    /// </summary>
    /// <returns></returns>
    public int GetGameLevel() => gameLevel;

    /// <summary>
    /// プレイ回数の取得
    /// </summary>
    /// <returns></returns>
    public int GetPlayTime() => playTime;

    /// <summary>
    ///  中断復帰判定取得
    /// </summary>
    public bool GetIsBreak() => isBreak;

    /// <summary>
    /// 課金判定取得
    /// </summary>
    public bool GetIsAds() => isAds;
    //public bool GetIsAds() => true;
    #endregion
}
