using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// ������e
/// ������̕\����\���Ǘ�
/// </summary>

public enum GAMELEVEL
{
    LEVEL_1,//���j��100�܂�
    LEVEL_2,//100�ȏ�500����
    LEVEL_3,//500�ȏ�1000����
    LEVEL_4,//1000�ȏ�2000����
    LEVEL_5,//2000�ȏ�5000����
    LEVEL_6,//5000�ȏ�10000����
    LEVEL_7,//10000�ȏ�50000����
    LEVEL_8,//50000�ȏ�-
}
public class GeneratorManager : MonoBehaviour
{
    /// <summary>
    /// ������N���X
    /// </summary>
    [SerializeField]
    private List<EnemyGenerator> enemyGenerators = new List<EnemyGenerator>();

    //�ғ����̐�����
    private List<EnemyGenerator> activeEnemyGenerators = new List<EnemyGenerator>();

    /// <summary>
    /// �Q�[�����x��
    /// </summary>
    [SerializeField]
    private GAMELEVEL level;

    /// <summary>
    /// UI����
    /// </summary>
    private UiController uiController;

    //�ő吶����\�����F5

    //�؂�ւ��p�J�E���g���F100�ɂȂ��0�Ƃ���
    [SerializeField]
    private int ChangeKillCount;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        //�J�n���͂P�\��
        activeEnemyGenerators.Add(enemyGenerators[0]);
        enemyGenerators[0].gameObject.SetActive(true);

        ChangeKillCount = 0;

        level = GAMELEVEL.LEVEL_1;

        uiController = GameObject.FindGameObjectWithTag("UI").
                                  GetComponent<UiController>();
        
        //�J�n���ɃX�g�b�v
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

    /// <summary>
    /// �\�����̐�����̏�Ԃ�ύX
    /// </summary>
    public void ChangeGeneratorState(GENERATOR_STATE _state)
    {
        foreach (var generator in enemyGenerators)
        {
            if (generator.gameObject.activeSelf)
            {
                generator.State = _state;
            }
        }
    }

    /// <summary>
    /// �\�����̐�����Active�ؑ�
    /// </summary>
    public void ChangeGeneratorActive(bool _active)
    {
        foreach (var generator in activeEnemyGenerators)
        {
           generator.gameObject.SetActive(_active);
        }
    }

    /// <summary>
    /// ����������\��
    /// </summary>
    /// <param name="_level"></param>
    private void SetGeneratorLevel(int _level)
    {
        activeEnemyGenerators.Clear();

        IEnumerable<EnemyGenerator> changeGenerators = new List<EnemyGenerator>();
        changeGenerators = enemyGenerators?.OrderBy(i => Guid.NewGuid()).Take(_level);

        activeEnemyGenerators = changeGenerators.ToList();

        foreach (var generator in activeEnemyGenerators)
        {
            generator.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// �Q�[�����x���̕ύX
    /// </summary>
    /// <param name="_amount"></param>
    public void ChangeGameLevel(int _amount)
    {
        if ( 100 <= _amount && _amount < 500)
        {
            level = GAMELEVEL.LEVEL_2;
        }
        else if (500 <= _amount && _amount < 1000)
        {
            level = GAMELEVEL.LEVEL_3;
        }
        else if (1000 <= _amount && _amount < 2000)
        {
            level = GAMELEVEL.LEVEL_4;
        }
        else if (2000 <= _amount && _amount < 5000)
        {
            level = GAMELEVEL.LEVEL_5;
        }
        else if (5000 <= _amount && _amount < 10000)
        {
            level = GAMELEVEL.LEVEL_6;
        }
        else if (10000 <= _amount && _amount < 50000)
        {
            level = GAMELEVEL.LEVEL_7;
        }
        else if (50000 <= _amount)
        {
            level = GAMELEVEL.LEVEL_8;
        }
    }

    /// <summary>
    /// ���j���ɉ����ĕ\�����鐶�����ω�
    /// 100���ɐ�����ω�������
    /// </summary>
    public void ChangeUpdateGenerator()
    {
        //�l�̍X�V
        ChangeKillCount++;

        if (ChangeKillCount >= 100)
        {
            //��x��~
            ChangeGeneratorState(GENERATOR_STATE.STOP);
            ChangeGeneratorActive(false);

            //���x���ɉ����Đؑ�
            switch (level)
            {
                case GAMELEVEL.LEVEL_1:
                    //�J�n�������牽�����Ȃ�
                    break;
                case GAMELEVEL.LEVEL_2:
                    //������Q�\��
                    SetGeneratorLevel(2);
                    break;
                case GAMELEVEL.LEVEL_3:
                    //������3�\��
                    SetGeneratorLevel(3);
                    break;
                case GAMELEVEL.LEVEL_4:
                    //������4�\��
                    SetGeneratorLevel(4);
                    break;
                case GAMELEVEL.LEVEL_5:
                    //������5�\��
                    SetGeneratorLevel(5);
                    break;
                case GAMELEVEL.LEVEL_6:
                    //������6�\��
                    SetGeneratorLevel(6);
                    break;
                case GAMELEVEL.LEVEL_7:
                    //������7�\��
                    SetGeneratorLevel(7);
                    break;
                case GAMELEVEL.LEVEL_8:
                    //������8�\��
                    SetGeneratorLevel(8);
                    break;
                default:
                    break;
            }
            ChangeGeneratorState(GENERATOR_STATE.GENERATE);
            ChangeKillCount = 0;
        }
    }

}

