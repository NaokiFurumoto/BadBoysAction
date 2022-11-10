using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using static GlobalValue;
using System.Threading;

///// <summary>
///// 敵生成器クラス
///// </summary>
//public enum GENERATOR_INDEX
//{
//    NONE,
//    FIRST,
//    SECOND,
//    THIRD,
//    FOURTH,
//    FIFTH,
//    SIX,
//    SEVENTH,
//    EIGHT,
//}

/// <summary>
/// 生成状態状態
/// </summary>
public enum GENERATOR_STATE
{
    NONE,
    GENERATE,//生成中
    STOP,//停止中
    RESUME,//再開
}
public class NewEnemyGenerator : MonoBehaviour
{
    #region 敵に関して
    /// <summary>
    /// 生成する敵のオブジェクトリスト
    /// </summary>
    [SerializeField]
    private List<GameObject> enemyObjects = new List<GameObject>();

    /// <summary>
    /// 生成割合:レベルによって変化する値
    /// </summary>
    [SerializeField]
    private List<int> enemyEncounts = new List<int>();

    /// <summary>
    /// 敵生成親
    /// </summary>
    [SerializeField]
    private Transform enemysParent;

    [SerializeField]
    private GameController gameController;

    /// <summary>
    /// 敵生成数
    /// </summary>
    private int enemyCreateCount;

    /// <summary>
    /// 敵種類数
    /// </summary>
    private int enemyTypeCount;

    /// <summary>
    /// 生成位置
    /// </summary>
    private Vector3 createEnemyPos;

    /// <summary>
    /// 同じ位置に生成しない対応
    /// </summary>
    private float tempPos_x, tempPos_y = 0.0f;

    /// <summary>
    /// 生成する敵
    /// </summary>
    private GameObject createEnemyObj;

    private Vector3 rightTopScreen;
    private Vector3 leftBottomScreen;

    private List<GameObject> enemyAllObjects = new List<GameObject>();
    #endregion


    #region 生成機に関して
    /// <summary>
    /// 状態
    /// </summary>
    [SerializeField]
    private GENERATOR_STATE state;

    /// <summary>
    /// 自身の位置
    /// </summary>
    private Vector3 rootPosition;

    //生成間隔時間：レベルによって変化させる
    [SerializeField]
    private float createDelayTime;

    //生成遅延開始時間
    private int startDelayTaskTime;

    /// <summary>
    /// 計測時間：Time.deltaTimeで加算していく
    /// </summary>
    private float progressTime;
    #endregion

    #region プロパティ
    public GENERATOR_STATE State { get { return state; } set { state = value; } }
    public List<GameObject> EnemyAllObjects => enemyAllObjects;

    //レベルによって親から設定させる値
    public float CreateDelayTime { get { return createDelayTime; } set { createDelayTime = value; } }
    public List<int> EnemyEncounts { get { return enemyEncounts; } set { enemyEncounts = value; } }
    public int StartDelayTaskTime { get { return startDelayTaskTime; } set { startDelayTaskTime = value; } }
    #endregion

    /// <summary>
    /// 自身の初期化
    /// </summary>
    private void Awake() { InitializeThis(); }
    private void InitializeThis()
    {
        //スクリーン範囲
        rightTopScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        leftBottomScreen = Camera.main.ScreenToWorldPoint(Vector3.zero);

        //敵
        enemyCreateCount = 0;
        rootPosition = transform.position;
        enemyTypeCount = enemyObjects.Count;

        //生成機
        state = GENERATOR_STATE.STOP;
        progressTime = 0.0f;
        startDelayTaskTime = START_CREATE_DIFF;

        //レベルによって変化させる
        createDelayTime = 0.01f;

        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController")
                                   .GetComponent<GameController>();
        }
    }

    /// <summary>
    /// 外部初期初期化
    /// </summary>
    private void Start() { InitializeOther(); }
    private void InitializeOther() { }

    #region UniTask
    /// <summary>
    /// ゲーム開始時に呼ばれる処理
    /// </summary>
    public async UniTask StartCallGenerator()
    {
        await UniTask.Delay(startDelayTaskTime);
        state = GENERATOR_STATE.GENERATE;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //ゲーム開始していなければ動かさない
        if (gameController.State != INGAME_STATE.PLAYING)
            return;

        //生成中
        if (state == GENERATOR_STATE.GENERATE)
        {
            //経過時間
            progressTime += Time.deltaTime;

            if (progressTime > createDelayTime)
            {
                //生成チェック
                if (IsCheckOver()) return;

                //敵のセット
                createEnemyObj = SetEnemy();
                CreateEnemy();
                progressTime = 0;
            }
        }
    }
    #endregion

    /// <summary>
    /// 敵生成
    /// 指定の位置に指定Objを生成
    /// </summary>
    private void CreateEnemy()
    {
        if (createEnemyObj == null)
            return;

        SetEnemyPos();

        var obj = Instantiate(createEnemyObj, createEnemyPos, Quaternion.identity);
        obj.transform.SetParent(enemysParent);
        enemyAllObjects.Add(obj);

        enemyCreateCount++;
    }

    /// <summary>
    /// 生成位置の設定
    /// </summary>
    private void SetEnemyPos()
    {
        float x, y;
        bool isCheck;

        do
        {
            x = Random.Range(leftBottomScreen.x - ENEMY_CREATE_DIFF_MAX,
                              rightTopScreen.x + ENEMY_CREATE_DIFF_MAX);
            y = Random.Range(leftBottomScreen.y - ENEMY_CREATE_DIFF_MAX,
                              rightTopScreen.y + ENEMY_CREATE_DIFF_MAX);

            isCheck = (leftBottomScreen.x - ENEMY_CREATE_DIFF_MIN <= x)
                    && (rightTopScreen.x + ENEMY_CREATE_DIFF_MIN >= x)
                    && (leftBottomScreen.y - ENEMY_CREATE_DIFF_MIN <= y)
                    && (rightTopScreen.y + ENEMY_CREATE_DIFF_MIN >= y);
                   

            //isCheck = leftBottomScreen.x - ENEMY_CREATE_DIFF_MIN <= x ? rightTopScreen.x + ENEMY_CREATE_DIFF_MIN >= x
            //        : leftBottomScreen.y - ENEMY_CREATE_DIFF_MIN <= y ? rightTopScreen.y + ENEMY_CREATE_DIFF_MIN >= y
            //        : false;

        } while (isCheck);

        tempPos_x = x;
        tempPos_y = y;

        createEnemyPos = new Vector3((float)tempPos_x, (float)tempPos_y, 0);
    }


    /// <summary>
    /// 生成する敵キャラを設定
    /// </summary>
    private GameObject SetEnemy()
    {
        var totalRatio = TotalRatio();
        var createNum = Random.Range(1, totalRatio);

        var total = 0;
        for (int i = 0; i < enemyTypeCount; i++)
        {
            total += enemyEncounts[i];
            if (createNum <= total)
            {
                return enemyObjects[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 合計エンカウント数
    /// </summary>
    /// <returns></returns>
    private int TotalRatio()
    {
        return enemyEncounts.Sum();
    }

    /// <summary>
    /// 生成間隔の変更
    /// </summary>
    private void ChangeCreateDelay()
    {
        //撃破数によって間隔を短く：最小と最大を設ける
        //撃破数が100を超える度に生成間隔を-0.1秒づつ短くする？
        //最小値と最大値の間
    }

    /// <summary>
    /// 画面に表示させる最大生成数チェック
    /// </summary>
    public bool IsCheckOver()
    {
        return enemysParent.childCount >= ENEMY_SCREEN_MAXCOUNT ? true : false;
    }

    /// <summary>
    /// 表示されてる敵の削除
    /// </summary>
    public void DeleteEnemys()
    {
        if (enemyAllObjects.Count == 0)
            return;

        foreach (var enemy in enemyAllObjects)
        {
            Destroy(enemy);
        }

        enemyAllObjects.Clear();
        enemyCreateCount = 0;
    }



    //レベルアップ時の更新処理
    public void LevelUpdate(int level)
    {
        //createDelayTaskTime:生成間隔


        //最初から有効にする
        //ゲームレベルによって敵の生成間隔と、敵の種類をを変化させる。：ランダム性がいる？
        //ゲームレベルによって最大生成数を変化させる？
        //Unitaskで生成？Whileで？

    }
    //リトライ用のリフレッシュ処理が必要








    //最初から有効にする
    //ゲームレベルによって敵の生成間隔と、敵の種類をを変化させる。：ランダム性がいる？
    //ゲームレベルによって最大生成数を変化させる？
    //Unitaskで生成？Whileで？

}


