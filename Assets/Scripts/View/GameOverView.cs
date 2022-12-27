using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverView : ViewBase
{
    /// <summary>
    /// UIコントローラー
    /// </summary>
    [SerializeField]
    private UiController uiController;

    /// <summary>
    /// 撃破数
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_Kills;

    /// <summary>
    /// ハイスコア
    /// </summary>
    [SerializeField]
    private GameObject hiScoreObject;

    /// <summary>
    /// クラッカー
    /// </summary>
    [SerializeField]
    private GameObject ClackerRightObject;

    /// <summary>
    /// クラッカー
    /// </summary>
    [SerializeField]
    private GameObject ClackerLeftObject;

    /// <summary>
    /// ランクボタン
    /// </summary>
    [SerializeField]
    private Button btn_Rank;

    /// <summary>
    /// ユーザー
    /// </summary>
    [SerializeField]
    private UserAuth user;

    /// <summary>
    /// キャッシュ用
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;


    protected override  void OnEnable()
    {
        base.OnEnable();
        FM = SoundManager.Instance;
        appSound = AppSound.Instance;

        btn_Rank.interactable = false;
        HiScoreEffect(false);
        uiController ??= GameObject.FindGameObjectWithTag("UI").
                                     GetComponent<UiController>();
        user ??= GameObject.FindGameObjectWithTag("User").
                                     GetComponent<UserAuth>();
        //撃破数の表示
        text_Kills.text = uiController?.GetTextKillsNumber().ToString();
    }

    public void GameOverSE()
    {
        FM.PlayOneShot(appSound.SE_GAMEOVER);
    }

    protected override void OnDisable() { }

    /// <summary>
    /// アニメーション終了後に呼ばれる
    /// </summary>
    public override void OpenEndAnimation() 
    {
        //中断復帰解除
        uiController.SetIsBreak(false);

        //ハイスコア更新されていれば
        var score = uiController.GetKillsNumber();
        var hiScore = uiController.GetHiScore();

        if (score > hiScore)
        {
            //ハイスコア演出表示
            HiScoreEffect(true);
            uiController.SetHiScore(score);
            FM.PlayOneShot(appSound.SE_HISCORE);
            btn_Rank.interactable = user.IsLogin ? true : false;
        }

        //更新後に保存
        var saveData = SaveManager.Instance.Load();
        saveData.HiScoreNumber = uiController.GetHiScore();
        SaveManager.Instance.Save(saveData);
    }

    /// <summary>
    /// ハイスコア演出切替
    /// </summary>
    /// <param name="enable"></param>
    public void HiScoreEffect(bool enable)
    {
        hiScoreObject.SetActive(enable);
        ClackerRightObject.SetActive(enable);
        ClackerLeftObject.SetActive(enable);
    }

    /// <summary>
    /// ハイスコアをシェアする
    /// </summary>
    public void SnsShare()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        SnsManager.Instance?.Tweet();
    }

    /// <summary>
    /// ランキングボタンに遷移
    /// </summary>
    public void OnClickRank()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        SceneManager.sceneLoaded += KeepScore;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(uiController.GetKillsNumber());
    }

    /// <summary>
    /// ランキングシーンに渡すもの
    /// </summary>
    /// <param name="nextScene"></param>
    /// <param name="mode"></param>
    private void KeepScore(Scene nextScene, LoadSceneMode mode)
    {
        var rank = GameObject.Find("RankingSceneManager").GetComponent<RankingScene>();
        rank.UserName = user.CurrentPlayer;
        rank.Score = uiController.GetKillsNumber();
        rank.Hiscore = uiController.GetHiScore();
        rank.IsGameOver = true;
        // イベントの削除
        SceneManager.sceneLoaded -= KeepScore;
    }

}
