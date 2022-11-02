using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using static GlobalValue;
using UnityEngine.Advertisements;
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

    ///// <summary>
    ///// Fade���
    ///// </summary>
    //[SerializeField]
    //private GameFade fadeView;

    //�R�[���o�b�N
    private UnityAction RetryAction;

    /// <summary>
    /// View������
    /// </summary>
    private void InitializeView()
    {
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
        if (uiController.StaminasManager.IsCheckRecovery())
        {
            //�X�^�~�i���g�p���čĊJ
            uiController.StaminasManager.UseStamina();

            FadeFilter.Instance.FadeIn(Color.black, FADETIME, RetryAction);
            RetryStatus();
            gameOverView.gameObject.SetActive(false);
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
                            StaminasManager.Instance.FullRecovery
                            (true, CommonDialogManager.Instance.DeleteDialogAll);
                        }
                    }
                ));

            //���X�g�ɒǉ�
            CommonDialogManager.Instance.AddList(dialog);
        }
    }
   
}
