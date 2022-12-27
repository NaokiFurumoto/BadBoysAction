﻿using UnityEngine;
using System.IO;
using static GlobalValue;
using UnityEngine.SceneManagement;

/// <summary>
/// セーブ・ロード処理を行うクラス
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string filePath;

    private UiController uiController;

    private NewGenerateManager generatorManager;

    private NewEnemyGenerator enemyGenerator;

    private GameController gameController;

    private UserAuth user;


    private void Awake() { InitializeAwake(); }
    private void InitializeAwake()
    {
        Instance ??= this;
        //アプリが実行中に保持するデータを格納できるディレクトリパス
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
    }

    private void Start() { InitializeThis(); }
    public void InitializeThis()
    {
        if (SceneManager.GetActiveScene().name == GAMESCENENAME)
        {
            uiController = GameObject.FindGameObjectWithTag("UI")
                                   .GetComponent<UiController>();

            generatorManager = GameObject.FindGameObjectWithTag("GeneratorRoot").
                                          GetComponent<NewGenerateManager>();

            enemyGenerator = GameObject.FindGameObjectWithTag("EnemyGenerator").
                                          GetComponent<NewEnemyGenerator>();

            gameController = GameObject.FindGameObjectWithTag("GameController")
                                   .GetComponent<GameController>();

            user = GameObject.FindGameObjectWithTag("User")
                                   .GetComponent<UserAuth>();
        }
    }



    /// <summary>
    /// セーブ
    /// </summary>
    public void Save(SaveData savedata)
    {
        string json = JsonUtility.ToJson(savedata);

        //テキストファイルにデータを書き込むことができる
        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
        }
    }

    /// <summary>
    /// ロード:
    //while (true)
    //{
    //    whileで繰り返し待機処理
    //    if (File.Exists(Path)) break;
    //    yield return null;
    //}
    /// </summary>
    public SaveData Load()
    {
        //Fileが存在するか
        if (File.Exists(filePath))
        {
            StreamReader reader;
            using (reader = new StreamReader(filePath))
            {
                string data = reader.ReadToEnd();
                return JsonUtility.FromJson<SaveData>(data);
            }

        }

        //初回はfileが存在しないのでこちら
        return GetInitSaveData();
    }

    /// <summary>
    /// 初回時のセーブデータを取得
    /// </summary>
    /// <returns></returns>
    public SaveData GetInitSaveData()
    {
        SaveData data = new SaveData();
        data.StaminaNumber = STAMINA_MAXNUMBER;
        data.KillsNumber = 0;
        data.HiScoreNumber = 0;
        data.LifeNumber = START_LIFEPOINT;
        data.GemeLevel = 1;
        data.PlayTime = 0;
        data.BGM_Volume = 0.5f;
        data.SE_Volume = 1.0f;
        data.IsBreak = false;
        data.IsShowAds = false;
        data.saveTime = TimeManager.Instance.GetDayTimeInteger();
        data.changeKillCount = 0;
        data.levelupNeedCount = LEVELUP_COUNT;
        data.createDelayTime = FIRST_CREATETIME;
        data.enemyScreenDisplayIndex = ENEMY_SCREEN_MAXCOUNT;
        data.IsFirstViewOpen = false;
        data.IsSighin = false;
        data.IsLogin = false;
        data.UserName = "";
        data.Passward = "";
        return data;
    }

    /// <summary>
    /// ロードデータをクリアデータに変更
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public SaveData ChangeCleartDate(SaveData data)
    {
        data.KillsNumber = 0;
        data.HiScoreNumber = 0;
        data.LifeNumber = START_LIFEPOINT;
        data.GemeLevel = 1;
        data.PlayTime = 0;
        data.IsBreak = false;
        data.changeKillCount = 0;
        data.levelupNeedCount = LEVELUP_COUNT;
        data.createDelayTime = FIRST_CREATETIME;
        data.enemyScreenDisplayIndex = ENEMY_SCREEN_MAXCOUNT;
        data.IsFirstViewOpen = false;
        data.IsLogin = false;
        data.UserName = "";
        data.Passward = "";
        return data;
    }

    /// <summary>
    /// ロードデータをリスタートデータに変更
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public SaveData ChangeRestartDate(SaveData data)
    {
        data.KillsNumber = 0;
        data.LifeNumber = START_LIFEPOINT;
        data.GemeLevel = 1;
        data.IsBreak = false;
        data.changeKillCount = 0;
        data.levelupNeedCount = LEVELUP_COUNT;
        data.createDelayTime = FIRST_CREATETIME;
        data.enemyScreenDisplayIndex = ENEMY_SCREEN_MAXCOUNT;
        return data;
    }

    /// <summary>
    /// ゲーム途中セーブ
    /// ボリューム状態の保存
    /// </summary>
    public void GamePlaingSave()
    {
        SaveData saveData = new SaveData();
        {
            saveData.StaminaNumber = uiController.GetStamina();
            saveData.KillsNumber = uiController.GetKillsNumber();
            saveData.HiScoreNumber = uiController.GetHiScore();
            saveData.LifeNumber = uiController.GetLifeNum();
            saveData.GemeLevel = uiController.GetGameLevel();
            saveData.PlayTime = uiController.GetPlayTime();
            saveData.IsBreak = uiController.GetIsBreak();
            saveData.IsShowAds = uiController.GetIsAds();
            saveData.saveTime = TimeManager.Instance.GetDayTimeInteger();
            saveData.changeKillCount = generatorManager.GetChangeKillCount();
            saveData.levelupNeedCount = generatorManager.GetLevelupNeedCount();
            saveData.createDelayTime = enemyGenerator.GetCreateDelayTime();
            saveData.enemyScreenDisplayIndex = enemyGenerator.GetEnemyScreenDisplayIndex();
            saveData.gameState = gameController.State;
            saveData.IsFirstViewOpen = gameController.IsOpenFirstview;
            saveData.IsSighin = user.IsSignUp;
            saveData.IsLogin = user.IsLogin;
            saveData.UserName = user.CurrentPlayer;
            saveData.Passward = user.CurrentPassward;
            saveData.BGM_Volume = SoundManager.Instance.Bgm_SeVolume.Item1;
            saveData.SE_Volume = SoundManager.Instance.Bgm_SeVolume.Item2;
        }

        SaveManager.Instance.Save(saveData);
    }
}
