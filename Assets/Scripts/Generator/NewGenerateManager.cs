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

    //��~������
    private bool IsInterval;

    /// <summary>
    /// UI����
    /// </summary>
    private UiController uiController;

    private ItemController itemController;

    //�؂�ւ��p�J�E���g���F100�ɂȂ��0�Ƃ���
    [SerializeField]
    private int changeKillCount;

    #region �v���p�e�B
    public int GameLevel { get { return gameLevel; } set { gameLevel = value; } }
    #endregion

    /// <summary>
    /// ���g�̏�����
    /// </summary>
    private void Awake() { InitializeThis(); }
    private void InitializeThis()
    {
        gameLevel = 1;
        changeKillCount = 0;
        IsInterval = false;
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

        itemController = GameObject.FindGameObjectWithTag("ItemController").
                                    GetComponent<ItemController>();
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


    //���j���ɉ����āA���x���̕ύX
    public void ChangeUpdateGenerator()
    {
        changeKillCount++;
        if (changeKillCount > LEVELUP_COUNT)
        {
            //���x���A�b�v
            gameLevel++;
            uiController.SetGameLevel(gameLevel);

            //�C���^�[�o����݂���
            ChangeGeneratorState(GENERATOR_STATE.STOP);

            //�̗̓h���b�v
            itemController.SetDropItem(DROPITEM_TYPE.LIFE);
            itemController.CreateDropItem();
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
        //�ҋ@:���x�����オ��ΐL�΂�
        await UniTask.Delay(2000);
        IsInterval = false;
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

    //�������ꂽ�S�Ă̓G�̍폜
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
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

}
