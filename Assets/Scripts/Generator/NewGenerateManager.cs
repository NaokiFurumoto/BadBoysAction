using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using static GlobalValue;

/// <summary>
/// 生成器親
/// 生成器の制御
/// </summary>
public class NewGenerateManager : MonoBehaviour
{
    /// <summary>
    /// 生成器クラス
    /// </summary>
    [SerializeField]
    private NewEnemyGenerator enemyGenerator;

    /// <summary>
    /// ゲームレベル
    /// 1 - 100 最大MAX表示
    /// </summary>
    [SerializeField]
    private int gameLevel;

    /// <summary>
    /// 停止中判定
    /// </summary>
    private bool IsInterval;

    /// <summary>
    /// UI操作
    /// </summary>
    private UiController uiController;

    /// <summary>
    /// アイテム操作クラス
    /// </summary>
    private ItemController itemController;

    /// <summary>
    /// レベルアップ用カウント数
    /// </summary>
    [SerializeField]
    private int changeKillCount;

    /// <summary>
    /// 必要経験値：変動させる
    /// </summary>
    [SerializeField]
    private int levelupNeedCount;


    #region プロパティ
    public int GameLevel { get { return gameLevel; } set { gameLevel = value; } }
    #endregion

    /// <summary>
    /// ゲーム開始時の初期化
    /// </summary>
    public void InitializeThis()
    {
        gameLevel = 1;
        changeKillCount = 0;
        IsInterval = false;
        levelupNeedCount = LEVELUP_COUNT;
        //敵生成初期化
        enemyGenerator.InitializeData();
    }

    /// <summary>
    /// ロード時の初期化
    /// </summary>
    public void InitializeLoaded()
    {
        gameLevel = uiController.GetGameLevel();
        changeKillCount = GetChangeKillCount();
        IsInterval = false;
        levelupNeedCount = GetLevelupNeedCount();
        //敵生成初期化
        enemyGenerator.InitializeLoadedData();
    }

    /// <summary>
    /// 外部初期初期化
    /// </summary>
    private void Start() { InitializeOther().Forget(); }
    private async UniTask InitializeOther()
    {
        //1フレーム待ち
        await UniTask.Yield();
        uiController = GameObject.FindGameObjectWithTag("UI").
                                 GetComponent<UiController>();
        itemController = ItemController.Instance;

        //開始時にストップ
        ChangeGeneratorState(GENERATOR_STATE.STOP);
    }

    /// <summary>
    /// 開始時の対応
    /// </summary>
    public void StartGenerate()
    {
        //敵生成開始
        enemyGenerator.StartCallGenerator().Forget();
    }

    /// <summary>
    /// 撃破数に応じて、レベルの変更
    /// </summary>
    public void ChangeUpdateGenerator()
    {
        changeKillCount++;
        if (changeKillCount >= levelupNeedCount)
        {
            //レベルアップ
            gameLevel++;
            uiController.SetGameLevel(gameLevel);
            enemyGenerator.LevelUpdate();

            levelupNeedCount += ADDLEVELUP_COUNT;

            //インターバルを設ける
            ChangeGeneratorState(GENERATOR_STATE.STOP);

            //体力ドロップ
            itemController.SetDropItem(DROPITEM_TYPE.LIFE);
            itemController.CreateDropItem(true);
            changeKillCount = 0;

            //インターバル中は実行させない
            if (!IsInterval)
            {
                IsInterval = true;
                GenerateStandBy().Forget();
            }
        }
    }

    /// <summary>
    /// 生成器の状態を変更
    /// </summary>
    public void ChangeGeneratorState(GENERATOR_STATE _state)
    {
        enemyGenerator.State = _state;
    }

    /// <summary>
    /// 生成レベルアップ時の更新処理
    /// </summary>
    /// <returns></returns>
    private async UniTask GenerateStandBy()
    {
        await UniTask.Delay(LEVELUP_INTERVAL);
        IsInterval = false;
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

    /// <summary>
    /// 全ての敵の削除
    /// </summary>
    private void DeleteEnemys()
    {
        enemyGenerator.DeleteEnemys();
    }

    /// <summary>
    /// リトライ処理
    /// </summary>
    public void RetryGenerator()
    {
        DeleteEnemys();
        InitializeThis();
        enemyGenerator.RetryInitialize();
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

    /// <summary>
    /// レベルアップ用カウント数
    /// </summary>
    /// <returns></returns>
    public int GetChangeKillCount()
    {
        return changeKillCount;
    }

    /// <summary>
    /// 必要経験値
    /// </summary>
    /// <returns></returns>
    public int GetLevelupNeedCount()
    {
        return levelupNeedCount;
    }

    /// <summary>
    /// レベルアップ用カウント数
    /// </summary>
    /// <returns></returns>
    public void SetChangeKillCount(int count)
    {
        changeKillCount = count;
    }

    /// <summary>
    /// 必要経験値
    /// </summary>
    /// <returns></returns>
    public void SetLevelupNeedCount(int count)
    {
        levelupNeedCount = count;
    }

}
