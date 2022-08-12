using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using System.Linq;
/// <summary>
/// 敵生成クラス
/// </summary>
public enum GENERATOR_RANK
{
    NONE,
    FIRST,
    SECOND,
    THIRD,
    FOURTH,
    FIFTH,
    SIX,
    SEVENTH,
    EIGHT,
}

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

public class EnemyGenerator : MonoBehaviour
{

    #region 敵に関して
    /// <summary>
    /// 生成オブジェクトリスト
    /// </summary>
    [SerializeField]
    private List<GameObject> enemyObjects = new List<GameObject>();

    /// <summary>
    /// 生成割合
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
    private static int enemyCreateCount;

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
    #endregion

    #region 生成機に関して
    /// <summary>
    /// 生成ランク
    /// </summary>
    [SerializeField]
    private GENERATOR_RANK rank;

    /// <summary>
    /// 状態
    /// </summary>
    [SerializeField]
    private GENERATOR_STATE state;

    /// <summary>
    /// 自身の位置
    /// </summary>
    private Vector3 rootPosition;

    //生成間隔時間
    [SerializeField]
    private float createDelayTime;

    /// <summary>
    /// 計測時間
    /// </summary>
    private float progressTime;
    #endregion

    #region プロパティ
    public GENERATOR_STATE State { get { return state; } set { state = value; } }
    #endregion

    /// <summary>
    /// 有効時に呼ばれる
    /// </summary>
    private void OnEnable()
    {
        Initialize();

        //生成間隔設定
        SetCreateDelay();
    }

    /// <summary>
    /// 無効時に呼ばれる
    /// </summary>

    private void OnDisable()
    {
        
    }

    private void Initialize()
    {
        //敵
        enemyCreateCount = 0;
        rootPosition = transform.position;
        enemyTypeCount = enemyObjects.Count;

        //生成機
        state = GENERATOR_STATE.STOP;
        createDelayTime = 0;
        progressTime = 0;
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController")
                                   .GetComponent<GameController>();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        //ゲーム開始していなければ動かさない
        //if (gameController.State != INGAME_STATE.PLAYING)
        //    return;

        if (state == GENERATOR_STATE.STOP)
            return;

        //生成中
        if (state == GENERATOR_STATE.GENERATE)
        {
            //経過時間
            progressTime += Time.deltaTime;

            if(progressTime > createDelayTime)
            {
                //敵のセット
                createEnemyObj = SetEnemy();
                CreateEnemy();
                progressTime = 0;
            }
        }
    }

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

        //敵のHPを設定する処理必要？
        enemyCreateCount++;
        //生成チェック
        CheckOverChangeState();
    }

    /// <summary>
    /// 生成位置の設定
    /// </summary>
    private void SetEnemyPos()
    {
        float x, y;

        do
        {
            x = Random.Range(rootPosition.x - EN_CREATEPOS_RADIUS,
                         rootPosition.x + EN_CREATEPOS_RADIUS);
            y = Random.Range(rootPosition.y - EN_CREATEPOS_RADIUS,
                                   rootPosition.y + EN_CREATEPOS_RADIUS);

        } while (Mathf.Abs(x - tempPos_x) <= CREATE_DIFFX || Mathf.Abs(y - tempPos_y) <= CREATE_DIFFY);

        tempPos_x = x;
        tempPos_y = y;

        createEnemyPos = new Vector3((float)tempPos_x, (float)tempPos_y, 0);
    }

    //生成間隔の設定処理
    private void SetCreateDelay()
    {
        createDelayTime = 1.0f;
    }


    /// <summary>
    /// 生成する敵キャラを設定
    /// </summary>
    private GameObject SetEnemy()
    {
        var totalRatio = TotalRatio();
        var createNum = Random.Range(1,totalRatio);
       
        var total = 0;
        for (int i = 0; i < enemyTypeCount; i++)
        {
            total += enemyEncounts[i];
            if(createNum <= total)
            {
                return enemyObjects[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 合計エンカウント数
    /// この処理の前にエンカウント数を変更する
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
    /// 最大生成数によってステートの変更
    /// どこからか呼ぶ必要がある
    /// </summary>
    public void CheckOverChangeState()
    {
        if (enemysParent == null)
            return;
        
        if(enemysParent.childCount >= MAX_GE_CREATECOUNT)
        {
            state = GENERATOR_STATE.STOP;
        }
        else
        {
            state = GENERATOR_STATE.GENERATE;
        }
    }

    //敵のHPを設定：敵キャラのコードを呼び出す
    
}
