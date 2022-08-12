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
    NONE = 0,//���ݒ�
    START = 1,//�J�n
    PLAYING = 2,//�Q�[����
    STOP = 3,//�X�g�b�v
    RESULT = 4,//����
}
public class GameController : MonoBehaviour
{
    /// <summary>
    /// �Q�[�����̏��
    /// </summary>
    [SerializeField]
    private INGAME_STATE state;

    /// <summary>
    /// �X�^�[�g���
    /// </summary>
    [SerializeField]
    private GameObject startView;

    /// <summary>
    /// �I�v�V�������
    /// </summary>
    [SerializeField]
    private GameObject optionView;

    public INGAME_STATE State
    {
        get { return state; }
        set { state = value; }
    }

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
        state = INGAME_STATE.RESULT;
        //��~����
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
    /// View�̔�\��
    /// </summary>
    public void DisableView(ViewBase _view)
    {
        _view.gameObject.SetActive(false);
    }

    /// <summary>
    /// View�̕\��
    /// </summary>
    public void EnableView(ViewBase _view)
    {
        _view.gameObject.SetActive(true);
    }

    #region View
    /// <summary>
    /// �I�v�V�����{�^���������ꂽ�Ƃ��ɌĂ�
    /// </summary>
    public void OnClickOptionButton()
    {
        if (State == INGAME_STATE.STOP)
        {
            //����
            GameResume();
            optionView.SetActive(false);
        }
        else if(State == INGAME_STATE.PLAYING)
        {
            //�J��
            GameStop();
            optionView.SetActive(true);
        }
    }
    #endregion
}