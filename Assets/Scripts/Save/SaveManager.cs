using UnityEngine;
using System.IO;
using static GlobalValue;

/// <summary>
/// �Z�[�u�E���[�h�������s���N���X
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
        //�A�v�������s���ɕێ�����f�[�^���i�[�ł���f�B���N�g���p�X
        filePath = Application.persistentDataPath + "/" + ".savedata.json";
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
    /// ���[�h
    /// </summary>
    public SaveData Load()
    {
        //File�����݂��邩
        if (File.Exists(filePath))
        {
            StreamReader reader;
            using(reader = new StreamReader(filePath))
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
        data.GemeLevel = GAMELEVEL.LEVEL_1;
        data.PlayTime = 0;
        data.BGM_Volume = 0.5f;
        data.SE_Volume = 0.5f;
        //data.IsGameOver = false;
        return data;
    }
}
