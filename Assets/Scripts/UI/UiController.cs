using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
/// <summary>
/// インゲームUIに関する操作/更新など
/// </summary>
public class UiController : MonoBehaviour
{
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
    /// プレイ回数
    /// </summary>
    private int playTime;

    /// <summary>
    /// ゲームオーバー判定
    /// </summary>
    private bool isGameOver;


    /// <summary>
    /// 生成器親管理クラス
    /// </summary>
    private GeneratorManager generatorManager;

    /// <summary>
    /// 自身の初期化
    /// </summary>
    private void Awake() { }

    #region プロパティ
    public LifesManager LifesManager => lifesManager;
    public StaminasManager StaminasManager => staminasManager;
    public int KillsNUM => killsNum;

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
                                      GetComponent<GeneratorManager>();
        isGameOver = false;
    }

    /// <summary>
    /// ロード完了後のUI表示周りの設定対応
    /// </summary>
    public void UpdateLoadedUI()
    {

        text_Kills.text = killsNum.ToString();
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
        generatorManager?.ChangeGameLevel(killsNum);
        generatorManager?.ChangeUpdateGenerator();
    }

    
    /// <summary>
    /// 撃破数の取得
    /// </summary>
    public int GetTextKillsNumber()
    {
        int num;
        if (int.TryParse(text_Kills.text,out num))
            return num;

        return 0;
    }

    /// <summary>
    /// リトライ設定
    /// </summary>
    public void RetryUI()
    {
        killsNum = 0;
        text_Kills.text = "0";
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
    }

    /// <summary>
    /// ゲームレベル変更
    /// </summary>
    public void SetGameLevel(GAMELEVEL level)
    {
        generatorManager.LEVEL = level;
        generatorManager.ChangeUpdateGenerator();
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
    /// スタミナ設定
    /// </summary>
    /// <param name="num"></param>
    public void SetStamina(int num)
    {
        if(num <= 0)
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
    /// ゲームオーバー判定
    /// </summary>
    /// <param name="num"></param>
    public void SetIsGameOver(bool judge)
    {
        isGameOver = judge;
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
    public GAMELEVEL GetGameLevel() => generatorManager.LEVEL;

    /// <summary>
    /// プレイ回数の取得
    /// </summary>
    /// <returns></returns>
    public int GetPlayTime() => playTime;

    /// <summary>
    ///  ゲームオーバー判定
    /// </summary>
    public bool GetIsGameOver() => isGameOver;
    #endregion

    
}
