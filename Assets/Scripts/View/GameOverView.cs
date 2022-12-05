using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    protected override  void OnEnable()
    {
        base.OnEnable();

        HiScoreEffect(false);
        uiController ??= GameObject.FindGameObjectWithTag("UI").
                                     GetComponent<UiController>();
        //撃破数の表示
        text_Kills.text = uiController?.GetTextKillsNumber().ToString();
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
        SnsManager.Instance?.Tweet();
    }

    

}
