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
    NONE,//���ݒ�
    START,//�J�n
    PLAYING,//�Q�[����
    STOP,//�X�g�b�v
    RESULT,//����
}
public class GameController : MonoBehaviour
{
    /// <summary>
    /// �Q�[�����̏��
    /// </summary>
    [SerializeField]
    private INGAME_STATE state;
    private INGAME_STATE ss;

    /// <summary>
    /// �X�^�[�g���
    /// </summary>
    [SerializeField]
    private GameObject startView;

    public INGAME_STATE State => state;

    void Start()
    {
        Initialize();
        GameStart();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        state = INGAME_STATE.NONE;
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

    private void PlayGame()
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
        state = INGAME_STATE.RESULT;
        //��~����
    }

    /// <summary>
    /// �ꎞ��~
    /// </summary>
    public void GameResume()
    {
        state = INGAME_STATE.PLAYING;
        Time.timeScale = 1;
    }

    /// <summary>
    /// �J�n�A�j���[�V�����I����ɌĂ΂��
    /// </summary>
    public void DisableStartView()
    {
        startView.gameObject.SetActive(false);
    }
}