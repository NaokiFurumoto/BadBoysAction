using UnityEngine;
using System.Collections;

public class AppSound : MonoBehaviour
{

    // === 外部パラメータ ======================================
    public static AppSound instance = null;

    // BGM
    [System.NonSerialized] public SoundManager FM;

    [System.NonSerialized] public AudioSource BGM_LOGO;
    [System.NonSerialized] public AudioSource BGM_TITLE;
    [System.NonSerialized] public AudioSource BGM_STAGE;
    [System.NonSerialized] public AudioSource BGM_RANKING;

    // SE
    [System.NonSerialized] public AudioSource SE_MENU_OK;
    [System.NonSerialized] public AudioSource SE_SELECT;
    [System.NonSerialized] public AudioSource SE_MENU_CANCEL;
    [System.NonSerialized] public AudioSource SE_RESTART;
    [System.NonSerialized] public AudioSource SE_TITLE;

    [System.NonSerialized] public AudioSource SE_GAMESTART;
    [System.NonSerialized] public AudioSource SE_GAMEOVER;
    [System.NonSerialized] public AudioSource SE_HISCORE;
    [System.NonSerialized] public AudioSource SE_CRACKER;

    [System.NonSerialized] public AudioSource SE_PL_ATK;
    [System.NonSerialized] public AudioSource SE_PL_DAMAGE;

    [System.NonSerialized] public AudioSource SE_EN_ATK;
    [System.NonSerialized] public AudioSource SE_EN_DAMAGE;

    [System.NonSerialized] public AudioSource SE_ITEM_LIFE;
    [System.NonSerialized] public AudioSource SE_ITEM_MUTEKI;

    // === 内部パラメータ ======================================
    private string sceneName = "non";

    // === コード =============================================
    [System.Obsolete]
    private void Start()
    {
        // Sound
        FM = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        LoderSound();
        instance = this;
    }

    [System.Obsolete]
    private void LoderSound()
    {
        // BGM
        FM.CreateGroup("BGM");
        FM.SoundFolder = "Sounds/BGM/";
        BGM_LOGO = FM.LoadResourcesSound("BGM", "BGM_Logo");
        BGM_TITLE = FM.LoadResourcesSound("BGM", "BGM_Title");
        BGM_STAGE = FM.LoadResourcesSound("BGM", "BGM_Stage");
        BGM_STAGE = FM.LoadResourcesSound("BGM", "BGM_Ranking");

        // SE
        FM.CreateGroup("SE");
        FM.SoundFolder = "Sounds/SE/";

        SE_MENU_OK = FM.LoadResourcesSound("SE", "SE_Menu_Ok");
        SE_MENU_CANCEL = FM.LoadResourcesSound("SE", "SE_Menu_Cancel");
        SE_SELECT = FM.LoadResourcesSound("SE", "SE_Select");
        SE_RESTART = FM.LoadResourcesSound("SE", "SE_ReStart");
        SE_TITLE = FM.LoadResourcesSound("SE", "SE_Title");

        SE_GAMESTART = FM.LoadResourcesSound("SE", "SE_GameStart");
        SE_GAMEOVER = FM.LoadResourcesSound("SE", "SE_GameOver");
        SE_HISCORE = FM.LoadResourcesSound("SE", "SE_HiScore");
        SE_CRACKER = FM.LoadResourcesSound("SE", "SE_Cracker");

        SE_PL_ATK = FM.LoadResourcesSound("SE", "SE_Pl_Atk");
        SE_PL_DAMAGE = FM.LoadResourcesSound("SE", "SE_Pl_Damage");
        SE_EN_ATK = FM.LoadResourcesSound("SE", "SE_En_Atk");
        SE_EN_DAMAGE = FM.LoadResourcesSound("SE", "SE_En_Damage");

        SE_ITEM_LIFE = FM.LoadResourcesSound("SE", "SE_Item_life");
        SE_ITEM_MUTEKI = FM.LoadResourcesSound("SE", "SE_Item_Muteki");
    }

    [System.Obsolete]
    void Update()
    {
        // シーンチェンジをチェック
        if (sceneName != Application.loadedLevelName)
        {
            sceneName = Application.loadedLevelName;

            // ボリューム設定
            //FM.SetVolume("BGM",SaveData.SoundBGMVolume);
            //FM.SetVolume("SE" ,SaveData.SoundSEVolume);

            // BGM再生
            if (sceneName == "LogoScene")
            {
                BGM_LOGO.Play();
            }
            else
            if (sceneName == "StartScene")
            {
                if (!BGM_TITLE.isPlaying)
                {
                    FM.Stop("BGM");
                    BGM_TITLE.Play();
                    //FM.FadeInVolume(BGM_TITLE,SaveData.SoundBGMVolume,1.0f,true);
                }
            }
            else
            if (sceneName == "GameScene")
            {
                FM.Stop ("BGM");
                FM.FadeOutVolumeGroup("BGM", BGM_STAGE, 0.0f, 1.0f, false);
                //FM.FadeInVolume(BGM_TITLE,SaveData.SoundBGMVolume,1.0f,true);
                BGM_STAGE.loop = true;
                BGM_STAGE.Play();
            }
        }
    }
}
