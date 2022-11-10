using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GlobalValue;
/// <summary>
/// �C���Q�[��UI�Ɋւ��鑀��/�X�V�Ȃ�
/// </summary>
public class UiController : MonoBehaviour
{
    /// <summary>
    /// �Q�[�����x���\��
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_gameLevel;

    /// <summary>
    /// ���x����
    /// </summary>
    [SerializeField]
    private int gameLevel;

    /// <summary>
    /// ���x��MAX�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject levelMaxObject;

    /// <summary>
    /// ���C�t�Ǘ�
    /// </summary>
    [SerializeField]
    private LifesManager lifesManager;

    /// <summary>
    /// �X�^�~�i�Ǘ�
    /// </summary>
    [SerializeField]
    private StaminasManager staminasManager;

    /// <summary>
    /// ���j���e�L�X�g�\��
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_Kills;

    /// <summary>
    /// ���j��
    /// </summary>
    [SerializeField]
    private int killsNum;

    /// <summary>
    /// �n�C�X�R�A
    /// </summary>
    private int hiScore;

    /// <summary>
    /// �n�C�X�R�A�e�L�X�g�\��
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text_HiScore;

    /// <summary>
    /// �v���C��
    /// </summary>
    [SerializeField]
    private int playTime;

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    private bool isGameOver;

    /// <summary>
    /// ������e�Ǘ��N���X
    /// </summary>
    private NewGenerateManager generatorManager;

    /// <summary>
    /// ���g�̏�����
    /// </summary>
    private void Awake() { }

    #region �v���p�e�B
    public LifesManager LifesManager => lifesManager;
    public StaminasManager StaminasManager => staminasManager;
    public int KillsNUM { get { return killsNum; } set {killsNum = value;} }

    public int HiScore => hiScore;

    public int PlayTime { get { return playTime; } set { playTime = value; } }

    #endregion

    /// <summary>
    /// �O��������
    /// </summary>
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        generatorManager = GameObject.FindGameObjectWithTag("GeneratorRoot").
                                      GetComponent<NewGenerateManager>();
        isGameOver = false;
    }

    /// <summary>
    /// �G���S���̌��j���̐ݒ�
    /// </summary>
    /// <param name="_number"></param>
    public void SetPlayKillsNumber()
    {
        if (text_Kills == null)
            return;
        killsNum++;
        text_Kills.text = killsNum.ToString();
        generatorManager?.ChangeUpdateGenerator();
    }

    
    /// <summary>
    /// ���j���̎擾
    /// </summary>
    public int GetTextKillsNumber()
    {
        int num;
        if (int.TryParse(text_Kills.text,out num))
            return num;

        return 0;
    }

   
    /// <summary>
    /// �X�R�A�̃N���A
    /// </summary>
    public void UpdateScore()
    {
        killsNum = 0;
        text_Kills.text = "0";
        text_HiScore.text = hiScore.ToString();
        SetGameLevel(1);
    }

   //-------------------------------Save�ELoad----------------------------------------//
   #region Load�֘A
    ////// <summary>
    /// ���񃍁[�h��̌��j���ݒ�
    /// </summary>
    /// <param name="_number"></param>
    public void SetLoadingKillsNumber(int num)
    {
        if (text_Kills == null)
            return;

        killsNum = num;
        text_Kills.text = killsNum.ToString();
    }

    /// <summary>
    /// �n�C�X�R�A�̐ݒ�
    /// </summary>
    public void SetHiScore(int score)
    {
       hiScore = score;
    }

    /// <summary>
    /// �Q�[�����x���ύX
    /// </summary>
    public void SetGameLevel(int level)
    {
        gameLevel = level;
        gameLevel = Mathf.Clamp(gameLevel, 1, MAX_GAMELEVEL);
        text_gameLevel.text = gameLevel.ToString();

        if (level >= MAX_GAMELEVEL)
        {
            text_gameLevel.gameObject.SetActive(false);
            levelMaxObject.gameObject.SetActive(true);
        }
        else
        {
            text_gameLevel.gameObject.SetActive(true);
            levelMaxObject.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �v���C�񐔂̐ݒ�
    /// </summary>
    /// <param name="playTime"></param>
    public void SetPlayTime(int playTime)
    {
        this.playTime = playTime;
    }

    /// <summary>
    /// �v���C�񐔒ǉ�
    /// </summary>
    /// <param name="playTime"></param>
    public void AddPlayTime()
    {
        this.playTime++;
    }

    /// <summary>
    /// �X�^�~�i�ݒ�
    /// </summary>
    /// <param name="num"></param>
    public void SetStamina(int num)
    {
        if(num <= 0)
        {
            //�X�^�~�i��S�Ďg�p�s�Ƃ���
            staminasManager.SetAllDisable();
        }
        else
        {
            staminasManager.SetStaminaNumber(num);
        }
    }

    /// <summary>
    /// ���C�t�ݒ�
    /// </summary>
    /// <param name="num"></param>
    public void SetLife(int num)
    {
        if (num <= START_LIFEPOINT)
        {
            num = START_LIFEPOINT;
        }
        //���C�t�𐔒l�ŉ�
        lifesManager.SetLife(num);
    }

    /// <summary>
    /// �Q�[���I�[�o�[����
    /// </summary>
    /// <param name="num"></param>
    public void SetIsGameOver(bool judge)
    {
        isGameOver = judge;
    }
    #endregion

    #region Save�֘A
    /// <summary>
    /// �c��X�^�~�i���擾
    /// </summary>
    /// <param name="num"></param>
    public int GetStamina() => staminasManager.GetUseStaminaNumber();

    /// <summary>
    /// ���j���擾
    /// </summary>
    /// <param name="num"></param>
    public int GetKillsNumber() => killsNum;

    /// <summary>
    /// �n�C�X�R�A�擾
    /// </summary>
    /// <returns></returns>
    public int GetHiScore() => hiScore;

    /// <summary>
    /// ���C�t���擾�F�r���p
    /// </summary>
    /// <returns></returns>
    public int GetLifeNum() => lifesManager.GetLifeNum();

    /// <summary>
    /// �Q�[�����x���̎擾
    /// </summary>
    /// <returns></returns>
    public int GetGameLevel() => gameLevel;

    /// <summary>
    /// �v���C�񐔂̎擾
    /// </summary>
    /// <returns></returns>
    public int GetPlayTime() => playTime;

    /// <summary>
    ///  �Q�[���I�[�o�[����
    /// </summary>
    public bool GetIsGameOver() => isGameOver;
    #endregion

    
}
