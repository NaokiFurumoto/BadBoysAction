using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GlobalValue;
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

    public INGAME_STATE State
    {
        get { return state; }
        set { state = value; }
    }

    void Start()
    {
        Initialize();
        InitializeView();
        GameStart();
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
    }

    /// <summary>
    /// �Q�[���J�n
    /// </summary>
    public void GameStart()
    {
        state = INGAME_STATE.START;
        startView.gameObject.SetActive(true);
        Invoke("PlayGame", START_PLAYINGTIME);
    }

    /// <summary>
    /// �v���C���ݒ�
    /// </summary>
    public void PlayGame()
    {
        state = INGAME_STATE.PLAYING;
    }

    /// <summary>
    /// �ꎞ��~
    /// </summary>
    public void GameStop()
    {
        state = INGAME_STATE.STOP;
        Time.timeScale = 0;
    }

    /// <summary>
    /// �Q�[���I�[�o�[
    /// </summary>
    public void GameResult()
    {
        Time.timeScale = 0;
        state = INGAME_STATE.RESULT;
        gameOverView.gameObject.SetActive(true);
    }

    /// <summary>
    /// �ĊJ
    /// </summary>
    public void GameResume()
    {
        state = INGAME_STATE.PLAYING;
        Time.timeScale = 1;
    }

    /// <summary>
    /// ���g���C
    /// </summary>
    public void RetryGame()
    {
        fadeView.gameObject.SetActive(false);
        Time.timeScale = 1;
        GameStart();
    }

    private void RetryStatus()
    {
        playerStatusController?.RetryPlayer();
        generatorManager?.RetryGenerator();
        uiController.RetryUI();
    }
}

