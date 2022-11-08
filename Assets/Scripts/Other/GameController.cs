using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
using System;

/// <summary>
/// �C���Q�[���̏�ԊǗ�
/// </summary>
//�Q�[�����
public enum INGAME_STATE
{
    NONE    = 0,//���ݒ�
    START   = 1,//�J�n
    PLAYING = 2,//�Q�[����
    STOP    = 3,//�X�g�b�v
    RESULT  = 4,//����
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
    private GeneratorManager generatorManager;

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
                                            GetComponent<GeneratorManager>();

        startFadeinCallBack = PlayInGame;
    }

    /// <summary>
    /// �C���Q�[���J�n
    /// </summary>
    public void PlayInGame()
    {
        //�X�^�~�i�`�F�b�N
        if (uiController.StaminasManager.IsCheckRecovery())
        {
            //�X�^�~�i���g�p���čĊJ
            uiController.StaminasManager.UseStamina();

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
                            StaminasManager.Instance.FullRecovery(true,() => { CommonDialogManager.Instance.DeleteDialogAll();
                                GameStart();
                            });
                        }
                    }
                    ),

                    () =>//�X�^�~�i���񕜂�����g�p���ăQ�[���J�n�B�񕜂��Ă��Ȃ�������^�C�g���ɖ߂�
                    {
                        if (StaminasManager.Instance.IsCheckRecovery())
                        {
                            StaminasManager.Instance.UseStamina();
                            CommonDialogManager.Instance.DeleteDialogAll();
                            GameStart();
                        }
                        else
                        {
                            //�^�C�g���ɖ߂�
                        }
                    }

                 );

            //���X�g�ɒǉ�
            CommonDialogManager.Instance.AddList(dialog);
        }

    }

    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void GameStart()
    {
        //�X�^�~�i��K���g�p���Ă��ԂȂ̂Ńe�L�X�g�͕\��������
        StaminasManager.Instance.ActiveTextRecovery(true);
        //�X�R�A�̏�����

        state = INGAME_STATE.START;
        startView.gameObject.SetActive(true);
        Invoke("SetPlayGame", START_PLAYINGTIME);
    }

    /// <summary>
    /// �v���C���ݒ�
    /// </summary>
    public void SetPlayGame()
    {
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
        TimeManager.Instance.SetSlow(STOP_TIME, 0.0f);
        state = INGAME_STATE.RESULT;
        uiController.SetIsGameOver(true);

        //�Z�[�u����
        SavegameInfo();

        gameOverView.gameObject.SetActive(true);
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
        SavegameInfo();

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
        uiController.RetryUI();
    }

    /// <summary>
    /// ���񃍁[�h����
    /// </summary>
    private IEnumerator LoadingGameInfo()
    {
        if (!IsUseSaveSystem)
            yield break;

        yield return null;

        SaveData loadData;
        do
        {
            loadData = SaveManager.Instance.Load();
        }
        while (loadData == null);

        {
            uiController.SetLoadingKillsNumber(loadData.KillsNumber);
            uiController.SetHiScore(loadData.HiScoreNumber);
            uiController.SetGameLevel(loadData.GemeLevel);
            uiController.SetPlayTime(loadData.PlayTime);
            uiController.SetStamina(loadData.StaminaNumber);
            uiController.SetLife(loadData.LifeNumber);
        }

        //�\���X�V
        uiController.UpdateLoadedUI();

        //�Ó]������ɃQ�[���J�n
        FadeFilter.Instance.FadeIn(Color.black, FADETIME, startFadeinCallBack);
    }

    /// <summary>
    /// �Z�[�u����
    /// GameOver��/
    /// </summary>
    public void SavegameInfo()
    {
        if (!IsUseSaveSystem)
            return;

        SaveData saveData = new SaveData();

        {
            //saveData.IsGameOver = uiController.GetIsGameOver();
            saveData.StaminaNumber = uiController.GetStamina();
            saveData.KillsNumber = uiController.GetKillsNumber();
            saveData.HiScoreNumber = uiController.GetHiScore();
            saveData.LifeNumber = uiController.GetLifeNum();
            saveData.GemeLevel = uiController.GetGameLevel();
            saveData.PlayTime = uiController.GetPlayTime();
        }

        SaveManager.Instance.Save(saveData);
    }

}

