using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// 敵移動クラス
/// </summary>

///移動タイプ
public enum ENEMY_MOVETYPE
{
    NONE,   //未設定
    CHASE,  //まっすぐ向かう
    AROUND, //周回
    ZIGZAG,//ジグザグ
    RAPIDED,//急接近
    //ESCAPE,
    HORMING,//ホーミング
}
public class EnemyMovement : MonoBehaviour
{
    #region 変数
    /// <summary>
    /// 移動スピード
    /// </summary>
    [SerializeField]
    protected float xSpeed = 1.5f, ySpeed = 1.5f;

    /// <summary>
    /// 移動量
    /// </summary>
    protected Vector2 moveDelta;

    /// <summary>
    /// プレイヤー情報
    /// </summary>
    protected GameObject player;
    protected Transform playerCenter;
    protected Vector3 playerLastPos;
        
    /// <summary>
    /// 自身の情報
    /// </summary>
    protected Vector3 startPos, movePos;

    /// <summary>
    /// 追いかけるスピード
    /// </summary>
    [SerializeField]
    protected float chaseSpeed = 0.8f;

    /// <summary>
    /// 回転の遅延
    /// </summary>
    [SerializeField]
    protected float turningDelay = 1;

    /// <summary>
    /// 次に方向転換可能な時間
    /// </summary>
    [SerializeField]
    protected float turningTimeDelay = 1f;

    /// <summary>
    /// プレイヤーの位置を最後に把握した時間
    /// </summary>
    protected float lastFollowTime;

    /// <summary>
    /// 向き保存用
    /// </summary>
    protected Vector3 tempScale;

    /// <summary>
    /// 敵のアニメーター
    /// </summary>
    [SerializeField]
    protected  Animator animator;

    /// <summary>
    /// 本体：向き変更用
    /// </summary>
    [SerializeField]
    protected  Transform body;

    /// <summary>
    /// 敵のステータス管理クラス
    /// </summary>
    protected  EnemyStatusController enemyStatusController;

    /// <summary>
    /// 敵の攻撃クラス
    /// </summary>
    protected  EnemyAttack enemyAttack;

    /// <summary>
    /// 敵の攻撃クラス
    /// </summary>
    protected Transform enemyTrans;

    /// <summary>
    /// 移動タイプ
    /// </summary>
    [SerializeField]
    protected  ENEMY_MOVETYPE enemyMoveType;
    #endregion

    #region プロパティ
    public Vector2 MoveDelta => moveDelta;
    public ENEMY_MOVETYPE MoveType => enemyMoveType;
    #endregion

    protected void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected virtual void Initialize()
    {
        player                = GameObject.FindWithTag("Player").gameObject;
        playerCenter          = player?.GetComponent<PlayerStatusController>().PlayerCenter;
        playerLastPos         = playerCenter.position;
        startPos              = transform.position;
        lastFollowTime        = Time.time;
        turningTimeDelay     *= turningDelay;
        enemyTrans            = transform;
        enemyStatusController = GetComponent<EnemyStatusController>();
        enemyAttack           = GetComponent<EnemyAttack>();

        if (animator == null)
        {
            animator = GameObject.FindGameObjectWithTag("EnemyAnimator").
                                  GetComponent<Animator>();
        }
    }

    /// <summary>
    /// 移動更新
    /// </summary>
    protected void FixedUpdate()
    {
        //自身が死亡状態か、ダメージを受けてたら実行しない
        if (enemyStatusController.IsDead
            || enemyStatusController.State == ENEMY_STATE.DAMAGE
            || enemyStatusController.State == ENEMY_STATE.NOCKBACK)
        {
            return;
        }

        MoveAnimation();
        TurnAround();

        //共通処理
        if(enemyStatusController.HasPlayerTarget)
        {
            if (!enemyAttack.IsAttacked)
            {
                //クラス先で実行
                TypeMove();
            }
            else//攻撃した場合
            {
                //クールダウン中
                if (enemyAttack.IsDamageCoolDown)
                {
                    //何らかの処理
                }
                else//クールダウンが経過した
                {
                    //攻撃していないものとする
                    enemyAttack.IsAttacked = false;
                }
            }
        }
        else
        {
            //発見していない場合の処理
        }
    }

    
    protected virtual void TypeMove() { }


    protected virtual void BaseMoving(float x, float y)
    {
        if (enemyStatusController.IsDead
           || enemyStatusController.State == ENEMY_STATE.DAMAGE
           || enemyStatusController.State == ENEMY_STATE.NOCKBACK)
        {
            return;
        }
        moveDelta = new Vector2(x * xSpeed, y * ySpeed);
        transform.Translate(moveDelta.x * Time.deltaTime,
                            moveDelta.y * Time.deltaTime, 0);
    }

    /// <summary>
    /// 向きの変更
    /// </summary>
    protected void TurnAround()
    {
        tempScale = body.localScale;
        if (enemyStatusController.HasPlayerTarget)
        {
            if (playerCenter.position.x > enemyTrans.position.x)
            {
                tempScale.x = Mathf.Abs(tempScale.x);
            }

            if (playerCenter.position.x < enemyTrans.position.x)
            {
                tempScale.x = -Mathf.Abs(tempScale.x);
            }
            else//探知できていない場合は生成された位置によって向きを変更
            {
                if (startPos.x > enemyTrans.position.x)
                {
                    tempScale.x = Mathf.Abs(tempScale.x);
                }

                if (startPos.x < enemyTrans.position.x)
                {
                    tempScale.x = -Mathf.Abs(tempScale.x);
                }
            }
        }
        //上記設定後に反映
        body.localScale = tempScale;
    }

    /// <summary>
    /// 移動アニメーションの切替:待機か歩き
    /// </summary>
    protected void MoveAnimation()
    {
        //移動中ならば呼ばない用にする
        if (enemyStatusController?.State == ENEMY_STATE.MOVE)
            return;
       
        if(moveDelta.sqrMagnitude > 0)
        {
            animator.SetBool("Walk", true);
            enemyStatusController.SetEnemyState(ENEMY_STATE.MOVE);
        }
        else
        {
            animator.SetBool("Walk", false);
            enemyStatusController.SetEnemyState(ENEMY_STATE.IDLE);
        }
    }
}
