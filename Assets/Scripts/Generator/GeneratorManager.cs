using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 生成器親
/// 生成器の表示非表示管理
/// </summary>

public enum GAMELEVEL
{
    LEVEL_1,//撃破数100まで
    LEVEL_2,//100以上500未満
    LEVEL_3,//500以上1000未満
    LEVEL_4,//1000以上2000未満
    LEVEL_5,//2000以上5000未満
    LEVEL_6,//5000以上10000未満
    LEVEL_7,//10000以上50000未満
    LEVEL_8,//50000以上-
}
public class GeneratorManager : MonoBehaviour
{
    /// <summary>
    /// 生成器クラス
    /// </summary>
    [SerializeField]
    private List<EnemyGenerator> enemyGenerators = new List<EnemyGenerator>();

    //稼働中の生成器
    private List<EnemyGenerator> activeEnemyGenerators = new List<EnemyGenerator>();

    /// <summary>
    /// ゲームレベル
    /// </summary>
    [SerializeField]
    private GAMELEVEL level;

    /// <summary>
    /// UI操作
    /// </summary>
    private UiController uiController;

    //最大生成器表示数：5

    //切り替え用カウント数：100になると0とする
    [SerializeField]
    private int ChangeKillCount;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        //開始時は１つ表示
        activeEnemyGenerators.Add(enemyGenerators[0]);
        enemyGenerators[0].gameObject.SetActive(true);

        ChangeKillCount = 0;

        level = GAMELEVEL.LEVEL_1;

        uiController = GameObject.FindGameObjectWithTag("UI").
                                  GetComponent<UiController>();
        
        //開始時にストップ
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

    /// <summary>
    /// 表示中の生成器の状態を変更
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
    /// 表示中の生成器Active切替
    /// </summary>
    public void ChangeGeneratorActive(bool _active)
    {
        foreach (var generator in activeEnemyGenerators)
        {
           generator.gameObject.SetActive(_active);
        }
    }

    /// <summary>
    /// 生成器を個数表示
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
    /// ゲームレベルの変更
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
    /// 撃破数に応じて表示する生成器を変化
    /// 100毎に生成を変化させる
    /// </summary>
    public void ChangeUpdateGenerator()
    {
        //値の更新
        ChangeKillCount++;

        if (ChangeKillCount >= 100)
        {
            //一度停止
            ChangeGeneratorState(GENERATOR_STATE.STOP);
            ChangeGeneratorActive(false);

            //レベルに応じて切替
            switch (level)
            {
                case GAMELEVEL.LEVEL_1:
                    //開始時だから何もしない
                    break;
                case GAMELEVEL.LEVEL_2:
                    //生成器２個表示
                    SetGeneratorLevel(2);
                    break;
                case GAMELEVEL.LEVEL_3:
                    //生成器3個表示
                    SetGeneratorLevel(3);
                    break;
                case GAMELEVEL.LEVEL_4:
                    //生成器4個表示
                    SetGeneratorLevel(4);
                    break;
                case GAMELEVEL.LEVEL_5:
                    //生成器5個表示
                    SetGeneratorLevel(5);
                    break;
                case GAMELEVEL.LEVEL_6:
                    //生成器6個表示
                    SetGeneratorLevel(6);
                    break;
                case GAMELEVEL.LEVEL_7:
                    //生成器7個表示
                    SetGeneratorLevel(7);
                    break;
                case GAMELEVEL.LEVEL_8:
                    //生成器8個表示
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

