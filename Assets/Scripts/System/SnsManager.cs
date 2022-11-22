using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocialConnector;
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
        StartCoroutine(_Tweet());
    }

    public IEnumerator _Tweet()
    {
        const string fileName = "image.png";
        string imgPath = Path.Combine(Application.persistentDataPath, fileName);

        // 前回のデータを削除
        if (File.Exists(imgPath)) File.Delete(imgPath);

        //スクリーンショットを撮影
        ScreenCapture.CaptureScreenshot(fileName);

        // スクショ撮影画像の保存が完了するまで待機する
        while (true)
        {
            //whileで繰り返し待機処理
            if (File.Exists(imgPath)) break;
            yield return null;
        }

        // 投稿する
        string tweetText = "ハイスコア獲得！！";
        string tweetURL = "ゲームアプリURL";

        try
        {
            SocialConnector.SocialConnector.Share(tweetText, tweetURL, imgPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
