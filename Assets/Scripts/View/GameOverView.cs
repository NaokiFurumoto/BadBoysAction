using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// �����N�{�^��
    /// </summary>
    [SerializeField]
    private Button btn_Rank;

    /// <summary>
    /// ���[�U�[
    /// </summary>
    [SerializeField]
    private UserAuth user;

    /// <summary>
    /// �L���b�V���p
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;


    protected override  void OnEnable()
    {
        base.OnEnable();
        FM = SoundManager.Instance;
        appSound = AppSound.Instance;

        btn_Rank.interactable = false;
        HiScoreEffect(false);
        uiController ??= GameObject.FindGameObjectWithTag("UI").
                                     GetComponent<UiController>();
        user ??= GameObject.FindGameObjectWithTag("User").
                                     GetComponent<UserAuth>();
        //���j���̕\��
        text_Kills.text = uiController?.GetTextKillsNumber().ToString();
    }

    public void GameOverSE()
    {
        FM.PlayOneShot(appSound.SE_GAMEOVER);
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
            FM.PlayOneShot(appSound.SE_HISCORE);
            btn_Rank.interactable = user.IsLogin ? true : false;
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
        FM.PlayOneShot(appSound.SE_MENU_OK);
        SnsManager.Instance?.Tweet();
    }

    /// <summary>
    /// �����L���O�{�^���ɑJ��
    /// </summary>
    public void OnClickRank()
    {
        FM.PlayOneShot(appSound.SE_MENU_OK);
        SceneManager.sceneLoaded += KeepScore;
        naichilab.RankingLoader.Instance.SendScoreAndShowRanking(uiController.GetKillsNumber());
    }

    /// <summary>
    /// �����L���O�V�[���ɓn������
    /// </summary>
    /// <param name="nextScene"></param>
    /// <param name="mode"></param>
    private void KeepScore(Scene nextScene, LoadSceneMode mode)
    {
        var rank = GameObject.Find("RankingSceneManager").GetComponent<RankingScene>();
        rank.UserName = user.CurrentPlayer;
        rank.Score = uiController.GetKillsNumber();
        rank.Hiscore = uiController.GetHiScore();
        rank.IsGameOver = true;
        // �C�x���g�̍폜
        SceneManager.sceneLoaded -= KeepScore;
    }

}
