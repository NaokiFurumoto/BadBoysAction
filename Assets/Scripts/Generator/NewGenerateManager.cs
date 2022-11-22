using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using static GlobalValue;

/// <summary>
/// ������e
/// ������̐���
/// </summary>
public class NewGenerateManager : MonoBehaviour
{
    /// <summary>
    /// ������N���X
    /// </summary>
    [SerializeField]
    private NewEnemyGenerator enemyGenerator;

    /// <summary>
    /// �Q�[�����x��
    /// 1 - 100 �ő�MAX�\��
    /// </summary>
    [SerializeField]
    private int gameLevel;

    /// <summary>
    /// ��~������
    /// </summary>
    private bool IsInterval;

    /// <summary>
    /// UI����
    /// </summary>
    private UiController uiController;

    /// <summary>
    /// �A�C�e������N���X
    /// </summary>
    private ItemController itemController;

    /// <summary>
    /// ���x���A�b�v�p�J�E���g��
    /// </summary>
    [SerializeField]
    private int changeKillCount;

    /// <summary>
    /// �K�v�o���l�F�ϓ�������
    /// </summary>
    [SerializeField]
    private int levelupNeedCount;


    #region �v���p�e�B
    public int GameLevel { get { return gameLevel; } set { gameLevel = value; } }
    #endregion

    /// <summary>
    /// �Q�[���J�n���̏�����
    /// </summary>
    public void InitializeThis()
    {
        gameLevel = 1;
        changeKillCount = 0;
        IsInterval = false;
        levelupNeedCount = LEVELUP_COUNT;
        //�G����������
        enemyGenerator.InitializeData();
    }

    /// <summary>
    /// ���[�h���̏�����
    /// </summary>
    public void InitializeLoaded()
    {
        gameLevel = uiController.GetGameLevel();
        changeKillCount = GetChangeKillCount();
        IsInterval = false;
        levelupNeedCount = GetLevelupNeedCount();
        //�G����������
        enemyGenerator.InitializeLoadedData();
    }

    /// <summary>
    /// �O������������
    /// </summary>
    private void Start() { InitializeOther().Forget(); }
    private async UniTask InitializeOther()
    {
        //1�t���[���҂�
        await UniTask.Yield();
        uiController = GameObject.FindGameObjectWithTag("UI").
                                 GetComponent<UiController>();
        itemController = ItemController.Instance;

        //�J�n���ɃX�g�b�v
        ChangeGeneratorState(GENERATOR_STATE.STOP);
    }

    /// <summary>
    /// �J�n���̑Ή�
    /// </summary>
    public void StartGenerate()
    {
        //�G�����J�n
        enemyGenerator.StartCallGenerator().Forget();
    }

    /// <summary>
    /// ���j���ɉ����āA���x���̕ύX
    /// </summary>
    public void ChangeUpdateGenerator()
    {
        changeKillCount++;
        if (changeKillCount >= levelupNeedCount)
        {
            //���x���A�b�v
            gameLevel++;
            uiController.SetGameLevel(gameLevel);
            enemyGenerator.LevelUpdate();

            levelupNeedCount += ADDLEVELUP_COUNT;

            //�C���^�[�o����݂���
            ChangeGeneratorState(GENERATOR_STATE.STOP);

            //�̗̓h���b�v
            itemController.SetDropItem(DROPITEM_TYPE.LIFE);
            itemController.CreateDropItem(true);
            changeKillCount = 0;

            //�C���^�[�o�����͎��s�����Ȃ�
            if (!IsInterval)
            {
                IsInterval = true;
                GenerateStandBy().Forget();
            }
        }
    }

    /// <summary>
    /// ������̏�Ԃ�ύX
    /// </summary>
    public void ChangeGeneratorState(GENERATOR_STATE _state)
    {
        enemyGenerator.State = _state;
    }

    /// <summary>
    /// �������x���A�b�v���̍X�V����
    /// </summary>
    /// <returns></returns>
    private async UniTask GenerateStandBy()
    {
        await UniTask.Delay(LEVELUP_INTERVAL);
        IsInterval = false;
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

    /// <summary>
    /// �S�Ă̓G�̍폜
    /// </summary>
    private void DeleteEnemys()
    {
        enemyGenerator.DeleteEnemys();
    }

    /// <summary>
    /// ���g���C����
    /// </summary>
    public void RetryGenerator()
    {
        DeleteEnemys();
        InitializeThis();
        enemyGenerator.RetryInitialize();
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

    /// <summary>
    /// ���x���A�b�v�p�J�E���g��
    /// </summary>
    /// <returns></returns>
    public int GetChangeKillCount()
    {
        return changeKillCount;
    }

    /// <summary>
    /// �K�v�o���l
    /// </summary>
    /// <returns></returns>
    public int GetLevelupNeedCount()
    {
        return levelupNeedCount;
    }

    /// <summary>
    /// ���x���A�b�v�p�J�E���g��
    /// </summary>
    /// <returns></returns>
    public void SetChangeKillCount(int count)
    {
        changeKillCount = count;
    }

    /// <summary>
    /// �K�v�o���l
    /// </summary>
    /// <returns></returns>
    public void SetLevelupNeedCount(int count)
    {
        levelupNeedCount = count;
    }

}
