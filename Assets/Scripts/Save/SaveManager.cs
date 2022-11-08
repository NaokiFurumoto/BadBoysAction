using UnityEngine;
using System.IO;
using static GlobalValue;

/// <summary>
/// セーブ・ロード処理を行うクラス
/// </summary>
public class SaveManager : MonoBehaviour
{
    private string filePath;
    public static SaveManager Instance;

    private void Awake()
    {
        InitializeAwake();
    }


    private void InitializeAwake()
    {
        Instance ??= this;
        //アプリが実行中に保持するデータを格納できるディレクトリパス
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
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
    /// ロード
    /// </summary>
    public SaveData Load()
    {
        //Fileが存在するか
        if (File.Exists(filePath))
        {
            StreamReader reader;
            using(reader = new StreamReader(filePath))
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
        data.GemeLevel = GAMELEVEL.LEVEL_1;
        data.PlayTime = 0;
        data.BGM_Volume = 0.5f;
        data.SE_Volume = 0.5f;
        //data.IsGameOver = false;
        return data;
    }
}
