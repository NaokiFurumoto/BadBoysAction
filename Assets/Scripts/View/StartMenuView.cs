using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using static GlobalValue;
/// <summary>
/// �X�^�[�g�V�[���̃��j���[��ʂɊւ��鏈��
/// </summary>
public class StartMenuView : ViewBase
{
    /// <summary>
    /// BGM�X���C�_�[
    /// </summary>
    [SerializeField]
    private MenuObject_Sliderbar slider_BGM;

    /// <summary>
    /// SE�X���C�_�[
    /// </summary>
    [SerializeField]
    private MenuObject_Sliderbar slider_SE;

    /// <summary>
    /// ���[�h�f�[�^
    /// </summary>
    private SaveData loadingData;

    /// <summary>
    /// �Z�[�u�N���A�R�[���o�b�N
    /// </summary>
    private UnityAction saveDataClearCallback;

    /// <summary>
    /// �_�C�A���O
    /// </summary>
    private CommonDialog dialog;

    /// <summary>
    /// ��ʗL����
    /// </summary>
    protected override void OnEnable()
    {
        loadingData = SaveManager.Instance.Load();
        slider_BGM.SetValue();
        slider_SE.SetValue();

        saveDataClearCallback = SaveDataClearCallback;
        SetSliderVolume();
    }

    protected override void OnDisable()
    {
        //�f�[�^�̐��`
        loadingData.BGM_Volume = slider_BGM.CurosorPosition.x;
        loadingData.SE_Volume = slider_SE.CurosorPosition.x;

        //�Z�[�u����
        SaveManager.Instance.Save(loadingData);

        SetInitialize(false);
    }

    /// <summary>
    /// �X���C�h�o�[�̏�����
    /// ���[�h�����l���Z�b�g����
    /// </summary>
    void SetSliderVolume()
    {
        slider_BGM.SetPosition(new Vector2(loadingData.BGM_Volume, 0.0f));
        slider_SE.SetPosition(new Vector2(loadingData.SE_Volume, 0.0f));

        SetInitialize(true);
    }

    /// <summary>
    /// �X���C�_�[�̏�������Ԑݒ�
    /// </summary>
    /// <param name="isInit"></param>
    private void SetInitialize(bool isInit)
    {
        slider_BGM.Initialized = isInit;
        slider_SE.Initialized = isInit;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// �Z�[�u�f�[�^�_�C�A���O�̕\��
    /// </summary>
    public void OpenSaveDataDialog()
    {
        dialog = CommonDialog.ShowDialog(SAVEDATA_CLEAR_TITlE, SAVEDATA_CLEAR_DESC, YES, NO, saveDataClearCallback, null);
    }

    /// <summary>
    /// �Z�[�u�f�[�^�N���A
    /// </summary>
    private void SaveDataClearCallback()
    {
        var data = SaveManager.Instance.ChangeCleartDate(loadingData);
        SaveManager.Instance.Save(data);

        slider_BGM.SetPosition(new Vector2(0.5f, 0.0f));
        slider_SE.SetPosition(new Vector2(0.5f, 0.0f));
        Destroy(dialog.gameObject);
    }

}
