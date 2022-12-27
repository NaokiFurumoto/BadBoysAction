using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;
using System.Threading;
using UnityEngine.SceneManagement;
/// <summary>
/// スタート画面に関する対応
/// </summary>
public class StartScene : MonoBehaviour
{
    /// <summary>
    /// タップオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Button btnBG;

    [SerializeField]
    private GameObject startMenuView;

    [SerializeField]
    private UserAuth user;

    [SerializeField]
    private GameObject txt_Login;

    [SerializeField]
    private GameObject txt_NotLogin;

    [SerializeField]
    private Button rankBtn;

    //ランキング表示用
    private float score, hiscore;

    void Awake()
    {
        btnBG.interactable = false;
        Time.timeScale = 1.0f;
        menu.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        FadeFilter.Instance.FadeIn(Color.black, 0.5f);
        //ここでデータのロード
        var data = SaveManager.Instance.Load();
        if (data.IsBreak && data.StaminaNumber != 0)
        {
            //ダイアログを表示
            var dialog = CommonDialog.ShowDialog
                (
                    LOADBATTLE_TITLE,
                    LOADBATTLE_DESC,
                    LOADBATTLE_YES,
                    LOADBATTLE_NO,
                    () => OnClickTapBG(),
                    () => StartCoroutine("InitDataStart")
                );
            yield break;
        }
        
        //ログインしていたら、ログイン情報の表示してログイン
        var isLogin = data.IsLogin;
        if (isLogin)
        {
            var name = data.UserName;
            var pass = data.Passward;
            txt_Login.SetActive(true);
            txt_NotLogin.SetActive(false);

            if(!(String.IsNullOrEmpty(name)) && !(String.IsNullOrEmpty(pass)))
            {
                user.logIn(name, pass);
                user.IsLogin = true;
                user.IsSignUp = true;
            }

            rankBtn.interactable = true;
        }
        else
        {
            txt_Login.SetActive(false);
            txt_NotLogin.SetActive(true);
            rankBtn.interactable = false;
        }

        //BGM/SEボリューム設定
        SoundManager.Instance.SetVolume("BGM", data.BGM_Volume);
        SoundManager.Instance.SetVolume("SE", data.SE_Volume);
        SoundManager.Instance.Bgm_SeVolume = (data.BGM_Volume, data.SE_Volume);

        yield return new WaitForSeconds(0.5f);
        AppSound.Instance.BGM_TITLE.Play();
        AppSound.Instance.BGM_TITLE.loop = true;

        score = 0;
        hiscore = data.HiScoreNumber;
        StartCoroutine("EnableTap");
    }


    /// <summary>
    /// タップボタンの有効
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableTap()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        menu.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        btnBG.interactable = true;
    }

   
    /// <summary>
    /// GameSceneに遷移する
    /// </summary>
    public void OnClickTapBG()
    {
        StartCoroutine("GoGameScene");
    }

    [Obsolete]
    private IEnumerator GoGameScene()
    {
        SoundManager.Instance.Stop("BGM");
        AppSound.Instance.SE_TAPSTART.Play();

        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        LoadScene.Load("GameScene");
    }

    /// <summary>
    /// ランキング表示
    /// </summary>
    public void OnClickRank()
    {
        AppSound.Instance.SE_MENU_OK.Play();
        SceneManager.sceneLoaded += KeepScore;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(hiscore);
    }

    /// <summary>
    /// メニュ－表示
    /// </summary>
    public void OnClickMenu()
    {
        AppSound.Instance.SE_MENU_OK.Play();
        StartMenuViewActivate(true);
    }

    /// <summary>
    /// メニュー画面の開閉
    /// </summary>
    /// <param name="isActive"></param>
    public void StartMenuViewActivate(bool isActive)
    {
        startMenuView?.SetActive(isActive);
    }

    //初期データで更新
    private IEnumerator InitDataStart()
    {
        var loadData = SaveManager.Instance.Load();
        loadData.IsBreak = false;

        SaveManager.Instance.Save(loadData);

        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        var dloadData = SaveManager.Instance.Load();
        LoadScene.Load("GameScene");
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
        rank.Score = (int)score;
        rank.Hiscore = (int)hiscore;
        rank.IsGameOver = false;
        // イベントの削除
        SceneManager.sceneLoaded -= KeepScore;
    }
}
