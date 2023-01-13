using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour, IUnityAdsListener
{
    private UiController uiController;
    private GameController gameController;

#if UNITY_IOS
    public const string GameID = "4969449";
    public const string BannerID = "Banner_iOS";
    public const string InterstitialID = "Interstitial_iOS";
    public const string RewardedID = "Rewarded_iOS";
#elif UNITY_ANDROID
    public const string GameID = "4969448";
    public const string BannerID = "Banner_Android";
    public const string InterstitialID = "Interstitial_Android";
    public const string RewardedID = "Rewarded_Android";
#elif UNITY_EDITOR
    public const string GameID = "4969449";
    public const string BannerID = "Banner_iOS";
    public const string InterstitialID = "Interstitial_iOS";
    public const string RewardedID = "Rewarded_iOS";
# endif

    public static UnityAdsManager Instance { get; private set; }

    //リワード広告表示終了後のコールバック
    private Action<ShowResult> finishReward;

    //インターステイシャル広告表示終了後のコールバック
    private Action<ShowResult> finishInter;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        finishReward = null;
        finishInter = null;
        uiController = GameObject.FindGameObjectWithTag("UI").
                                     GetComponent<UiController>();

        gameController = GameObject.FindGameObjectWithTag("GameController").
                                     GetComponent<GameController>();

        Advertisement.Initialize(GameID);
        //広告関連のイベントが発生するように登録(IUnityAdsListener用）
        Advertisement.AddListener(this);
        StartCoroutine(ShowBannerWhenReady());
    }

    /// <summary>
    /// バナ－表示
    /// ver 3.7.5
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowBannerWhenReady()
    {
        if(uiController == null)
        {
            uiController = FindObjectOfType<UiController>();
        }

        //バナー広告が表示できる状態かどうかの判定
        while (!Advertisement.IsReady(BannerID))
        {
            yield return new WaitForSeconds(0.5f);
        }

        //広告課金していない場合
        if (!uiController.GetIsAds())
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(BannerID);
        }
    }

    /// <summary>
    /// インターステイシャル広告の表示
    /// </summary>
    public void ShowInterstitial(Action<ShowResult> finish)
    {
        gameController.State = INGAME_STATE.ADS;
        finishInter = null;
        //広告が再生できる状態
        if (Advertisement.IsReady(InterstitialID))
        {
            Advertisement.Show(InterstitialID);
            finishInter = finish;
        }
    }

    /// <summary>
    /// リワード広告の表示
    /// </summary>
    public void ShowRewarded(Action<ShowResult> finish)
    {
        gameController.State = INGAME_STATE.ADS;
        finishReward = null;
        //広告が再生できる状態
        if (Advertisement.IsReady(RewardedID))
        {
            Advertisement.Show(RewardedID);
            finishReward = finish;
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    /// <summary>
    /// 動画広告が終わった後に呼ばれる関数
    /// </summary>
    /// <param name="placementId"></param>
    /// <param name="showResult">スキップされたか、最後まで視聴されたかの状態が入る</param>
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == RewardedID)
        {
            if (finishReward == null)
            {
                //回復してタイトルに遷移
                if(StaminasManager.Instance == null)
                {
                    StaminasManager.Instance = FindObjectOfType<StaminasManager>();
                }
                StaminasManager.Instance.FullRecovery(false);
                LoadScene.Load("StartScene");
                return;
            }
                
            finishReward(showResult);
        }
        else if (placementId == InterstitialID)
        {
            if (finishInter == null)
            {
                //タイトルに移行
                LoadScene.Load("StartScene");
                return;
            }
               
            finishInter(showResult);
        }
    }
}
