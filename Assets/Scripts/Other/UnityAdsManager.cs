using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Advertisements;

public class UnityAdsManager : MonoBehaviour,IUnityAdsListener
{
    private UiController uiController;

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

    //�L���\���I����̃R�[���o�b�N
    private Action<ShowResult> finish;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        uiController = GameObject.FindGameObjectWithTag("UI").
                                     GetComponent<UiController>();
        Advertisement.Initialize(GameID);
        //�L���֘A�̃C�x���g����������悤�ɓo�^(IUnityAdsListener�p�j
        Advertisement.AddListener(this);
        StartCoroutine(ShowBannerWhenReady());
    }

    /// <summary>
    /// �o�i�|�\��
    /// ver 3.7.5
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowBannerWhenReady()
    {
        //�o�i�[�L�����\���ł����Ԃ��ǂ����̔���
        while (!Advertisement.IsReady(BannerID))
        {
            yield return new WaitForSeconds(0.5f);
        }

        //�L���ۋ����Ă��Ȃ��ꍇ
        if (!uiController.GetIsAds())
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(BannerID);
        }
    }

    /// <summary>
    /// �C���^�[�X�e�C�V�����L���̕\��
    /// </summary>
    public void ShowInterstitial(Action<ShowResult> finish)
    {
        //�L�����Đ��ł�����
        if (Advertisement.IsReady(InterstitialID))
        {
            Advertisement.Show(InterstitialID);
            this.finish = finish;
        }
    }

    /// <summary>
    /// �����[�h�L���̕\��
    /// </summary>
    public void ShowRewarded(Action<ShowResult> finish)
    {
        //�L�����Đ��ł�����
        if (Advertisement.IsReady(RewardedID))
        {
            Advertisement.Show(RewardedID);
            this.finish = finish;
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
    /// ����L�����I�������ɌĂ΂��֐�
    /// </summary>
    /// <param name="placementId"></param>
    /// <param name="showResult">�X�L�b�v���ꂽ���A�Ō�܂Ŏ������ꂽ���̏�Ԃ�����</param>
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        this.finish?.Invoke(showResult);
    }
}
