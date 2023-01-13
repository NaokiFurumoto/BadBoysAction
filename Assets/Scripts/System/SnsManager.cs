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
        StartCoroutine(Tweets());
    }

    public IEnumerator Tweets()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string TimeNum = DateTime.Now.ToString("yyyyMMddHHmmss");
        string filePath = Path.Combine(Application.temporaryCachePath, "shared_img" + TimeNum + ".png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        //後でURLなど更新
        new NativeShare().AddFile(filePath)
            .SetSubject("Subject goes here").SetText("バンチョ　ハイスコア更新!!!").SetUrl("https://github.com/yasirkula/UnityNativeShare")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }
}
