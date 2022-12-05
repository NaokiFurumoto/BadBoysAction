using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static GlobalValue;

public class OptionView : ViewBase
{
    [SerializeField]
    private GameObject BG;

    /// <summary>
    /// ������
    /// </summary>
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(1);
        OpenDialogCallBack();
    }


    private void OpenDialogCallBack()
    {
        ///�C�x���g�̓o�^
        var eventTrigger = BG.gameObject.AddComponent<EventTrigger>();
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(eventData => { gameController?.OnClickOptionButton(); });
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// �^�C�g����ʂɈړ�
    /// </summary>
    public void OnClickTitleBtn()
    {
        //���݂̃o�g�����I�����ă^�C�g���ɖ߂�܂��B��낵���ł��傤���H
        //�X�^�~�i��Ԃ�ۑ�
        CommonDialog.ShowDialog(OPTION_GOSTART_TITLE, OPTION_GOSTART_DESC, YES, NO, GoTitleCallback);
    }

    /// <summary>
    /// �����L���O��ʂ̕\��
    /// </summary>
    public void OnClickRankBtn()
    {
    }

    /// <summary>
    /// �^�C�g����ʂɈȍ~
    /// </summary>
    private void GoTitleCallback()
    {
        ////�N���A�f�[�^�̎擾:���X�^�[�g�f�[�^�̎擾
        //var loadingData = SaveManager.Instance.GetClearSaveData();
        //if (loadingData == null)
        //    return;

        //SaveManager.Instance.Save(loadingData);
        StartCoroutine("FadeTitle");
    }

    private IEnumerator FadeTitle()
    {
        FadeFilter.Instance.FadeOut(Color.black, 1.0f);
        yield return new WaitForSecondsRealtime(1.0f);
        gameController.GameResume();
        LoadScene.Load("StartScene");
    }

    protected override void OnEnable()
    {
    }

    protected override void OnDisable()
    {
    }


}
