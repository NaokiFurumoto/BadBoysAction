using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GlobalValue;
using System;

/// <summary>
/// View�Ɋւ��鐧��N���X
/// </summary>
public partial class GameController : MonoBehaviour
{
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

    /// <summary>
    /// �Q�[���I�[�o�[���
    /// </summary>
    [SerializeField]
    private GameObject gameOverView;

    /// <summary>
    /// Fade���
    /// </summary>
    [SerializeField]
    private GameFade fadeView;

    //�R�[���o�b�N
    private Action FadeinAction;
    private Action RetryAction;

    /// <summary>
    /// View������
    /// </summary>
    private void InitializeView()
    {
        FadeinAction = RetryFadein;
        RetryAction = RetryGame;
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
        else if (State == INGAME_STATE.PLAYING)
        {
            //�J��
            GameStop();
            optionView.SetActive(true);
        }
    }

    /// <summary>
    /// ���g���C�{�^���������ꂽ�Ƃ�
    /// </summary>
    public void OnClickRetryButtin()
    {
        //�Ó]��ɍĊJ
        fadeView.gameObject.SetActive(true);
        fadeView.Play(GameFade.Mode.OUT, Color.black, FADETIME, FadeinAction);

        if (uiController.StaminasManager.IsCheckRecovery())
        {
            //�X�^�~�i���g�p���čĊJ
            uiController.StaminasManager.UseStamina();
        }
        else
        {
            //�X�^�~�i�m�F�_�C�A���O�\��
        }
    }

    /// <summary>
    /// FadeIn����
    /// </summary>
    private void RetryFadein()
    {
        fadeView.Play(GameFade.Mode.IN, new Color(1.0f,1.0f,1.0f,0.0f), FADETIME, RetryAction);

        gameOverView.gameObject.SetActive(false);
        RetryStatus();
    }

}
