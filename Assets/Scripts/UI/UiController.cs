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
    /// 生成器親管理クラス
    /// </summary>
    private GeneratorManager generatorManager;

    /// <summary>
    /// 自身の初期化
    /// </summary>
    private void Awake() { }

    #region
    public LifesManager LifesManager => lifesManager;
    public StaminasManager StaminasManager => staminasManager;
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
        //開始時は0にする
        killsNum = 0;
        text_Kills.text = killsNum.ToString();
        generatorManager = GameObject.FindGameObjectWithTag("GeneratorRoot").
                                      GetComponent<GeneratorManager>();
    }

    /// <summary>
    /// 撃破数の設定
    /// </summary>
    /// <param name="_number"></param>
    public void SetTextKillsNumber()
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
        text_Kills.text = "0";
    }
}
