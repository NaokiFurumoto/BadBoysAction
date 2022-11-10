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

    //停止中判定
    private bool IsInterval;

    /// <summary>
    /// UI操作
    /// </summary>
    private UiController uiController;

    private ItemController itemController;

    //切り替え用カウント数：100になると0とする
    [SerializeField]
    private int changeKillCount;

    #region プロパティ
    public int GameLevel { get { return gameLevel; } set { gameLevel = value; } }
    #endregion

    /// <summary>
    /// 自身の初期化
    /// </summary>
    private void Awake() { InitializeThis(); }
    private void InitializeThis()
    {
        gameLevel = 1;
        changeKillCount = 0;
        IsInterval = false;
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

        itemController = GameObject.FindGameObjectWithTag("ItemController").
                                    GetComponent<ItemController>();
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


    //撃破数に応じて、レベルの変更
    public void ChangeUpdateGenerator()
    {
        changeKillCount++;
        if (changeKillCount > LEVELUP_COUNT)
        {
            //レベルアップ
            gameLevel++;
            uiController.SetGameLevel(gameLevel);

            //インターバルを設ける
            ChangeGeneratorState(GENERATOR_STATE.STOP);

            //体力ドロップ
            itemController.SetDropItem(DROPITEM_TYPE.LIFE);
            itemController.CreateDropItem();
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
        //待機:レベルが上がれば伸ばす
        await UniTask.Delay(2000);
        IsInterval = false;
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

    //生成された全ての敵の削除
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
        ChangeGeneratorState(GENERATOR_STATE.GENERATE);
    }

}
