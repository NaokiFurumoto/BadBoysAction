using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
/// <summary>
/// SNSマネージャー
/// </summary>

public class SnsManager : MonoBehaviour
{
    public static SnsManager Instance;
    void Start()
    {
        Instance ??= this;
    }

    //シェア 
    public void Tweet()
    {
        //StartCoroutine(_Tweet());
        StartCoroutine(_Tweets());
    }

    public IEnumerator _Tweet()
    {
        string imgPath = Application.persistentDataPath + "/image.png";

        // 前回のデータを削除
        File.Delete(imgPath);

        //スクリーンショットを撮影
        ScreenCapture.CaptureScreenshot("image.png");

        // 撮影画像の保存が完了するまで待機
        while (true)
        {
            if (File.Exists(imgPath)) break;
            yield return null;
        }

        // 投稿する
        string tweetText = "ハイスコア獲得！！";
        //ゲームのURL　Android/IOSで分ける？
        string tweetURL = "twitter://post?message=";

        try
        {
            //SocialConnector.SocialConnector.Share(tweetText, tweetURL, imgPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public IEnumerator _Tweets()
    {
        string imagePass = Application.persistentDataPath + "/image.png";
        //前回の画像を消す
        File.Delete(imagePass);
        //新しいスクリーンショット撮影
        ScreenCapture.CaptureScreenshot("image.png");

        //なんかスクショ撮影のラグがあるから終わるまで待機
        while (true)
        {
            if (File.Exists(imagePass))
                break; yield return null;
        }
        //投稿
        string tweetText = "ハイスコア獲得！";
        string tweetURL = "アプリのURL";
        SocialConnector.PostMessage(SocialConnector.ServiceType.Twitter, tweetText, tweetURL, imagePass);
    }
}
