using UnityEngine;
using System.Collections;

public class AppSound : Singleton<AppSound>
{

    // === 外部パラメータ ======================================
    // BGM
    [System.NonSerialized] public SoundManager FM;

    [System.NonSerialized] public AudioSource BGM_TITLE;
    [System.NonSerialized] public AudioSource BGM_STAGE;

    // SE
    [System.NonSerialized] public AudioSource SE_MENU_OK;
    [System.NonSerialized] public AudioSource SE_SELECT;
    [System.NonSerialized] public AudioSource SE_MENU_CANCEL;
    [System.NonSerialized] public AudioSource SE_TITLE;
    [System.NonSerialized] public AudioSource SE_SLIDE_CHANGE;

    [System.NonSerialized] public AudioSource SE_GAMESTART;
    [System.NonSerialized] public AudioSource SE_TAPSTART;
    [System.NonSerialized] public AudioSource SE_GAMEOVER;
    [System.NonSerialized] public AudioSource SE_HISCORE;

    [System.NonSerialized] public AudioSource SE_PL_ATK;
    [System.NonSerialized] public AudioSource SE_PL_MUTEKI;
    [System.NonSerialized] public AudioSource SE_PL_DAMAGE;
    [System.NonSerialized] public AudioSource SE_PL_DEATH;

    [System.NonSerialized] public AudioSource SE_EN_DAMAGE;
    [System.NonSerialized] public AudioSource SE_EN_DEATH;
    [System.NonSerialized] public AudioSource SE_EN_WALL;

    [System.NonSerialized] public AudioSource SE_ITEM_LIFE;
    [System.NonSerialized] public AudioSource SE_ITEM_MUTEKI;

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
        BGM_TITLE = FM.LoadResourcesSound("BGM", "BGM_Title");
        BGM_STAGE = FM.LoadResourcesSound("BGM", "BGM_Stage");

        // SE
        FM.CreateGroup("SE");
        FM.SoundFolder = "Sounds/SE/";

        SE_MENU_OK = FM.LoadResourcesSound("SE", "SE_Menu_Ok");
        SE_MENU_CANCEL = FM.LoadResourcesSound("SE", "SE_Menu_Cancel");
        SE_SELECT = FM.LoadResourcesSound("SE", "SE_Select");
        SE_SLIDE_CHANGE = FM.LoadResourcesSound("SE", "SE_Slide_Change");

        SE_GAMESTART = FM.LoadResourcesSound("SE", "SE_GameStart");
        SE_TAPSTART = FM.LoadResourcesSound("SE", "SE_TapStart");
        SE_GAMEOVER = FM.LoadResourcesSound("SE", "SE_GameOver");
        SE_HISCORE = FM.LoadResourcesSound("SE", "SE_HiScore");

        SE_PL_ATK = FM.LoadResourcesSound("SE", "SE_Pl_Atk");
        SE_PL_MUTEKI = FM.LoadResourcesSound("SE", "SE_Pl_Muteki_Atk");
        SE_PL_DAMAGE = FM.LoadResourcesSound("SE", "SE_Pl_Damage");
        SE_PL_DEATH = FM.LoadResourcesSound("SE", "SE_Pl_Death");

        SE_EN_DAMAGE = FM.LoadResourcesSound("SE", "SE_En_Damage");
        SE_EN_DEATH = FM.LoadResourcesSound("SE", "SE_En_Death");
        SE_EN_WALL = FM.LoadResourcesSound("SE", "SE_En_Wall");

        SE_ITEM_LIFE = FM.LoadResourcesSound("SE", "SE_Item_life");
        SE_ITEM_MUTEKI = FM.LoadResourcesSound("SE", "SE_Item_Muteki");
    }
}
