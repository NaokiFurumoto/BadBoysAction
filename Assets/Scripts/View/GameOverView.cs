using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverView : ViewBase
{
    /// <summary>
    /// UI�R���g���[���[
    /// </summary>
    [SerializeField]
    private UiController uiController;

    /// <summary>
    /// ���j��
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_Kills;

    /// <summary>
    /// �n�C�X�R�A
    /// </summary>
    [SerializeField]
    private GameObject hiScoreObject;

    /// <summary>
    /// �N���b�J�[
    /// </summary>
    [SerializeField]
    private GameObject ClackerRightObject;

    /// <summary>
    /// �N���b�J�[
    /// </summary>
    [SerializeField]
    private GameObject ClackerLeftObject;

    protected override  void OnEnable()
    {
        base.OnEnable();

        HiScoreEffect(false);
        uiController ??= GameObject.FindGameObjectWithTag("UI").
                                     GetComponent<UiController>();
        //���j���̕\��
        text_Kills.text = uiController?.GetTextKillsNumber().ToString();
    }

    protected override void OnDisable() { }

    /// <summary>
    /// �A�j���[�V�����I����ɌĂ΂��
    /// </summary>
    public override void OpenEndAnimation() 
    {
        //���f���A����
        uiController.SetIsBreak(false);

        //�n�C�X�R�A�X�V����Ă����
        var score = uiController.GetKillsNumber();
        var hiScore = uiController.GetHiScore();

        if (score > hiScore)
        {
            //�n�C�X�R�A���o�\��
            HiScoreEffect(true);
            uiController.SetHiScore(score);
        }

        //�X�V��ɕۑ�
        var saveData = SaveManager.Instance.Load();
        saveData.HiScoreNumber = uiController.GetHiScore();
        SaveManager.Instance.Save(saveData);
    }

    /// <summary>
    /// �n�C�X�R�A���o�ؑ�
    /// </summary>
    /// <param name="enable"></param>
    public void HiScoreEffect(bool enable)
    {
        hiScoreObject.SetActive(enable);
        ClackerRightObject.SetActive(enable);
        ClackerLeftObject.SetActive(enable);
    }

    /// <summary>
    /// �n�C�X�R�A���V�F�A����
    /// </summary>
    public void SnsShare()
    {
        SnsManager.Instance?.Tweet();
    }

    

}
