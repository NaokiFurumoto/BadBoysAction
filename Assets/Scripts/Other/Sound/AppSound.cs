using UnityEngine;
using System.Collections;

public class AppSound : MonoBehaviour
{

    // === 外部パラメータ ======================================
    public static AppSound instance = null;

    // BGM
    [System.NonSerialized] public SoundManager fm;
    [System.NonSerialized] public AudioSource BGM_LOGO;
    [System.NonSerialized] public AudioSource BGM_TITLE;
    [System.NonSerialized] public AudioSource BGM_STAGE;

    // SE
    [System.NonSerialized] public AudioSource SE_MENU_OK;
    [System.NonSerialized] public AudioSource SE_MENU_CANCEL;

    [System.NonSerialized] public AudioSource SE_ATK;
    [System.NonSerialized] public AudioSource SE_HIT;

    [System.NonSerialized] public AudioSource SE_ITEM_LIFE;

    [System.NonSerialized] public AudioSource SE_OBJ_EXIT;

    // === 内部パラメータ ======================================
    private string sceneName = "non";

    // === コード =============================================
    [System.Obsolete]
    private void Start()
    {
        // Sound
        fm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        LoderSound();
        instance = this;
    }

    [System.Obsolete]
    private void LoderSound()
    {
        // BGM
        fm.CreateGroup("BGM");
        fm.SoundFolder = "Sounds/BGM/";
        BGM_LOGO = fm.LoadResourcesSound("BGM", "Logo");
        BGM_TITLE = fm.LoadResourcesSound("BGM", "Title");
        BGM_STAGE = fm.LoadResourcesSound("BGM", "Stage");

        // SE
        fm.CreateGroup("SE");
        fm.SoundFolder = "Sounds/SE/";

        SE_MENU_OK = fm.LoadResourcesSound("SE", "SE_Menu_Ok");
        SE_MENU_CANCEL = fm.LoadResourcesSound("SE", "SE_Menu_Cancel");

        SE_ATK = fm.LoadResourcesSound("SE", "SE_ATK");
        SE_HIT = fm.LoadResourcesSound("SE", "SE_HIT");

        SE_OBJ_EXIT = fm.LoadResourcesSound("SE", "SE_OBJ_Exit");
    }

    [System.Obsolete]

    void Update()
    {
        // シーンチェンジをチェック
        if (sceneName != Application.loadedLevelName)
        {
            sceneName = Application.loadedLevelName;

            // ボリューム設定
            //fm.SetVolume("BGM",SaveData.SoundBGMVolume);
            //fm.SetVolume("SE" ,SaveData.SoundSEVolume);

            // BGM再生
            if (sceneName == "Menu_Logo")
            {
                BGM_LOGO.Play();
            }
            else
            if (sceneName == "Menu_Title")
            {
                if (!BGM_TITLE.isPlaying)
                {
                    fm.Stop("BGM");
                    BGM_TITLE.Play();
                    //fm.FadeInVolume(BGM_TITLE,SaveData.SoundBGMVolume,1.0f,true);
                }
            }
            else
            if (sceneName == "Stage")
            {
                //fm.Stop ("BGM");
                fm.FadeOutVolumeGroup("BGM", BGM_STAGE, 0.0f, 1.0f, false);
                //fm.FadeInVolume(BGM_TITLE,SaveData.SoundBGMVolume,1.0f,true);
                BGM_STAGE.loop = true;
                BGM_STAGE.Play();
            }
        }
    }
}
