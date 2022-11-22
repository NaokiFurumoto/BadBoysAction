using UnityEngine;
using System.IO;
using static GlobalValue;

/// <summary>
/// �Z�[�u�E���[�h�������s���N���X
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string filePath;

    private UiController uiController;

    private NewGenerateManager generatorManager;

    private NewEnemyGenerator enemyGenerator;

    private GameController gameController;


    private void Awake() { InitializeAwake(); }
    private void InitializeAwake()
    {
        Instance ??= this;
        //�A�v�������s���ɕێ�����f�[�^���i�[�ł���f�B���N�g���p�X
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
    }

    private void Start() { InitializeThis(); }
    private void InitializeThis()
    {

        uiController = GameObject.FindGameObjectWithTag("UI")
                               .GetComponent<UiController>();

        generatorManager = GameObject.FindGameObjectWithTag("GeneratorRoot").
                                      GetComponent<NewGenerateManager>();

        enemyGenerator = GameObject.FindGameObjectWithTag("EnemyGenerator").
                                      GetComponent<NewEnemyGenerator>();

        gameController = GameObject.FindGameObjectWithTag("GameController")
                               .GetComponent<GameController>();
    }



    /// <summary>
    /// �Z�[�u
    /// </summary>
    public void Save(SaveData savedata)
    {
        string json = JsonUtility.ToJson(savedata);

        //�e�L�X�g�t�@�C���Ƀf�[�^���������ނ��Ƃ��ł���
        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            streamWriter.Write(json);
            streamWriter.Flush();
        }
    }

    /// <summary>
    /// ���[�h:
    //while (true)
    //{
    //    while�ŌJ��Ԃ��ҋ@����
    //    if (File.Exists(Path)) break;
    //    yield return null;
    //}
    /// </summary>
    public SaveData Load()
    {
        //File�����݂��邩
        if (File.Exists(filePath))
        {
            StreamReader reader;
            using (reader = new StreamReader(filePath))
            {
                string data = reader.ReadToEnd();
                return JsonUtility.FromJson<SaveData>(data);
            }

        }

        //�����file�����݂��Ȃ��̂ł�����
        return GetInitSaveData();
    }

    /// <summary>
    /// ���񎞂̃Z�[�u�f�[�^���擾
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
        data.SE_Volume = 0.5f;
        data.IsBreak = false;
        data.IsShowAds = false;
        data.saveTime = TimeManager.Instance.GetDayTimeInteger();
        data.changeKillCount = 0;
        data.levelupNeedCount = LEVELUP_COUNT;
        data.createDelayTime = FIRST_CREATETIME;
        data.enemyScreenDisplayIndex = ENEMY_SCREEN_MAXCOUNT;
        return data;
    }

    /// <summary>
    /// �Q�[���r���Z�[�u
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
        }

        SaveManager.Instance.Save(saveData);
    }
}
