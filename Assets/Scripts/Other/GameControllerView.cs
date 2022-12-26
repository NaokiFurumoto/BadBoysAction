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
    private ViewBase FirstGameInfoView;


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

    //�R�[���o�b�N
    private UnityAction RetryAction;

    /// <summary>
    /// �Q�[��������ʂ�\�������邩�ǂ���
    /// </summary>
    public bool IsOpenFirstview;

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
        FM.PlayOneShot(appSound.SE_MENU_OK);
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
    /// �Q�[��������ʂ̕\��
    /// </summary>
    public void OpenFirstView()
    {
        if (IsOpenFirstview)
        {
            PlayInGame();
        }
        else
        {
            EnableView(FirstGameInfoView);
            var firstView = FirstGameInfoView as FirstGameInfoView;
            firstView.CallBack = PlayInGame;
        }
    }


    /// <summary>
    /// ���g���C�{�^���������ꂽ�Ƃ�
    /// </summary>
    public void OnClickRetryButtin()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        ///�ۋ����Ă����炻�̂܂܃��g���C
        if (uiController.GetIsAds())
        {
            FadeFilter.Instance.FadeIn(Color.black, FADETIME, RetryAction);
            RetryStatus();
            gameOverView.gameObject.SetActive(false);
            return;
        }

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

    /// <summary>
    /// �^�C�g����ʂɈȍ~
    /// </summary>
    public void GoTitle()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        StartCoroutine("GoFadeTitle");
    }

    private IEnumerator GoFadeTitle()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        GameResume();
        LoadScene.Load("StartScene");
    }

}
