using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;
using NCMB;

/// <summary>
/// �C���Q�[���̏�ԊǗ�
/// </summary>
//�Q�[�����
public enum INGAME_STATE
{
    NONE = 0,//���ݒ�
    START = 1,//�J�n
    PLAYING = 2,//�Q�[����
    STOP = 3,//�X�g�b�v
    RESULT = 4,//����
}
public partial class GameController : MonoBehaviour
{
    /// <summary>
    /// Unity�f�o�b�O�p
    /// </summary>
    [SerializeField]
    private bool IsUseSaveSystem = true;

    /// <summary>
    /// �Q�[�����̏��
    /// </summary>
    [SerializeField]
    private INGAME_STATE state;

    /// <summary>
    /// UI�Ǘ��N���X
    /// </summary>
    private UiController uiController;

    /// <summary>
    /// �v���C���[�̐���
    /// </summary>
    private PlayerStatusController playerStatusController;

    /// <summary>
    /// �v���C���[�̐���
    /// </summary>
    private NewGenerateManager generatorManager;

    /// <summary>
    /// �G�̐���
    /// </summary>
    private NewEnemyGenerator enemyGenerator;

    /// <summary>
    /// �L���b�V���p
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;

    /// <summary>
    /// �J�n��Fadein�̃R�[���o�b�N
    /// </summary>
    private UnityAction startFadeinCallBack;

    #region �v���p�e�B
    public INGAME_STATE State
    {
        get { return state; }
        set { state = value; }
    }
    #endregion

    void Start()
    {
        Initialize();
        InitializeView();
        SaveManager.Instance?.InitializeThis();

        //���[�h����
        StartCoroutine("LoadingGameInfo");
    }


    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        state = INGAME_STATE.NONE;

        uiController = GameObject.FindGameObjectWithTag("UI").
                                     GetComponent<UiController>();

        playerStatusController = GameObject.FindGameObjectWithTag("Player").
                                            GetComponent<PlayerStatusController>();

        generatorManager = GameObject.FindGameObjectWithTag("GeneratorRoot").
                                            GetComponent<NewGenerateManager>();

        startFadeinCallBack = OpenFirstView;

        enemyGenerator = GameObject.FindGameObjectWithTag("EnemyGenerator").
                                      GetComponent<NewEnemyGenerator>();
        FM = SoundManager.Instance;
        appSound = AppSound.Instance;
    }

    /// <summary>
    /// ���[�h����
    /// </summary>
    private IEnumerator LoadingGameInfo()
    {
        //�Z�[�u�V�X�e���g�����F�Ńo�b�N�p
        if (!IsUseSaveSystem)
            yield break;

        SaveData loadData;
        do
        {
            yield return null;
            loadData = SaveManager.Instance.Load();

        }
        while (loadData == null);
        
        //���f���A���A���U���g�\������Ȃ��A�̗͂�0�ł͂Ȃ�
        if (loadData.IsBreak && loadData.gameState != INGAME_STATE.RESULT && loadData.LifeNumber != 0 )
        {
            SetStatus(loadData);
        }
        else
        {
            //���X�^�[�g�f�[�^�̔��f
            var changeData = SaveManager.Instance.ChangeRestartDate(loadData);
            SetStatus(changeData);
        }

        //�Ó]������ɃQ�[���J�n:PlayInGame()
        FadeFilter.Instance.FadeIn(Color.black, FADETIME, startFadeinCallBack);
    }

   

    /// <summary>
    /// �C���Q�[���J�n
    /// </summary>
    public void PlayInGame()
    {
        ///�ۋ����Ă�����X�^�~�i���t���񕜂��āA�X�^�[�g
        if (uiController.GetIsAds())
        {
            StaminasManager.Instance.FullRecovery(false);
            GameStart();
            return;
        }

        //�X�^�~�i�`�F�b�N
        if (StaminasManager.Instance.IsCheckRecovery())
        {
            var aa = uiController.GetIsBreak();
            if (!uiController.GetIsBreak())
            {
                //���f���A����Ȃ��ꍇ�̓X�^�~�i���g�p����
                StaminasManager.Instance.UseStamina();
            }

            GameStart();
        }
        else
        {
            //�X�^�~�i�m�F�_�C�A���O�\��
            var dialog =
                CommonDialog.ShowDialog
                (
                    STAMINA_LESS_TITLE,
                    STAMINA_LESS_DESC,
                    MOVIECHECK,
                    CLOSE,

                    () => UnityAdsManager.Instance.ShowRewarded(result =>
                    {
                        //�X�^�~�i�S��
                        if (result == ShowResult.Finished)
                        {
                            StaminasManager.Instance.FullRecovery(true, () =>
                            {
                                CommonDialogManager.Instance.DeleteDialogAll();
                                GameResume();
                                GameStart();
                            });
                        }
                    }
                    ),

                    () =>//�X�^�~�i���񕜂�����g�p���ăQ�[���J�n�B�񕜂��Ă��Ȃ�������^�C�g���ɖ߂�
                    {
                        if (StaminasManager.Instance.IsCheckRecovery())
                        {
                            //StaminasManager.Instance.UseStamina();
                            CommonDialogManager.Instance.DeleteDialogAll();
                            GameStart();
                        }
                        else
                        {
                            //�^�C�g����ʂɑJ��
                            StartCoroutine("FadeTitle");
                        }
                    }
                 );

            //���X�g�ɒǉ�
            CommonDialogManager.Instance.AddList(dialog);
        }
    }

    /// <summary>
    /// �^�C�g����ʂɑJ��
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeTitle()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        GameResume();
        LoadScene.Load("StartScene");
    }

    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void GameStart()
    {
        //�X�^�~�i��K���g�p���Ă��ԂȂ̂Ńe�L�X�g�͕\��������:����Ȃ��ƂȂ�
        StaminasManager.Instance.ActiveTextRecovery(true);

        //�G����
        generatorManager.StartGenerate();

        state = INGAME_STATE.START;
        FM.PlayOneShot(appSound.SE_GAMESTART);
        startView.gameObject.SetActive(true);
        Invoke("SetPlayGame", START_PLAYINGTIME);
    }

    /// <summary>
    /// �v���C���ݒ�
    /// </summary>
    public void SetPlayGame()
    {
        //���[�hVolume�ɕύX
        //FM.SetVolume(appSound.BGM_STAGE, 1.0f);
        appSound.BGM_STAGE.Play();
        FM.FadeInVolume(appSound.BGM_STAGE, FM.GetVolume(appSound.BGM_STAGE), 2.0f,true);
        appSound.BGM_STAGE.loop = true;
        
        state = INGAME_STATE.PLAYING;
    }

    /// <summary>
    /// �ꎞ��~
    /// </summary>
    public void GameStop()
    {
        state = INGAME_STATE.STOP;
        TimeManager.Instance.SetSlow(STOP_TIME, 0.0f);
    }

    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    public void GameResult()
    {
        //FM.FadeOutVolume(appSound.BGM_STAGE, 0.0f, 0.5f, false);
        //�v���C�񐔒ǉ�
        uiController.AddPlayTime();

        //��~����
        TimeManager.Instance.SetSlow(STOP_TIME, 0.0f);
        state = INGAME_STATE.RESULT;

        //�Z�[�u����:
        SaveManager.Instance.GamePlaingSave();

        //���̃^�C�~���O�ōL���\���B�Q��ɂP��L���\���F
        if (uiController.PlayTime % 2 == 0 && !uiController.GetIsAds())
        {
            //�L���I����̃R�[���o�b�N
            Action<ShowResult> call = (result) =>
            {
                //�Q�[�����U���g��ʂ�\��
                uiController.SetIsBreak(false);
                gameOverView.gameObject.SetActive(true);
            };

            UnityAdsManager.Instance.ShowInterstitial(call);

        }
        else
        {
            //�Q�[�����U���g��ʂ�\��
            uiController.SetIsBreak(false);
            gameOverView.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// �ĊJ
    /// </summary>
    public void GameResume()
    {
        state = INGAME_STATE.PLAYING;
        TimeManager.Instance.ResetSlow();
    }

    /// <summary>
    /// ���g���C
    /// </summary>
    public void RetryGame()
    {
        //�Z�[�u����
        TimeManager.Instance.ResetSlow();
        GameStart();
    }

    /// <summary>
    /// �X�e�[�^�X�̍X�V
    /// </summary>
    private void RetryStatus()
    {
        playerStatusController?.RetryPlayer();
        generatorManager?.RetryGenerator();
        uiController.UpdateScore();
        ItemController.Instance.Retry();
        PlayerEffectManager.Instance.Retry();
    }

    

    /// <summary>
    /// �X�e�[�^�X�ݒ�
    /// </summary>
    /// <param name="loadData"></param>
    private void SetStatus(SaveData loadData)
    {
        IsOpenFirstview = loadData.IsFirstViewOpen;

        uiController.SetLoadingKillsNumber(loadData.KillsNumber);
        uiController.SetHiScore(loadData.HiScoreNumber);
        uiController.SetGameLevel(loadData.GemeLevel);
        uiController.SetPlayTime(loadData.PlayTime);
        uiController.SetStamina(loadData.StaminaNumber);
        uiController.SetLife(loadData.LifeNumber);
        uiController.SetIsBreak(loadData.IsBreak);
        uiController.SetIsAds(loadData.IsShowAds);
        uiController.SetLoadStamina(loadData.saveTime);

        generatorManager.GameLevel = loadData.GemeLevel;
        generatorManager.IsInterval = false;
        generatorManager.SetChangeKillCount(loadData.changeKillCount);
        generatorManager.SetLevelupNeedCount(loadData.levelupNeedCount);

        enemyGenerator.SetCreateDelayTime(loadData.createDelayTime);
        enemyGenerator.SetEnemyScreenDisplayIndex(loadData.enemyScreenDisplayIndex);
        enemyGenerator.SetLoadedEnemyEncounts(loadData.GemeLevel);
        UserAuth.Instance.CurrentPlayer = loadData.UserName;
        UserAuth.Instance.CurrentPassward = loadData.Passward;
        UserAuth.Instance.IsLogin = loadData.IsLogin;
        UserAuth.Instance.IsSignUp = loadData.IsSighin;

        //BGM/SE�{�����[���ݒ�
        SoundManager.Instance.SetVolume("BGM", loadData.BGM_Volume);
        SoundManager.Instance.SetVolume("SE", loadData.SE_Volume);
        SoundManager.Instance.Bgm_SeVolume = (loadData.BGM_Volume, loadData.SE_Volume);
    }

}

