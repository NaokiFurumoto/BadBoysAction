using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// 敵移動クラス
/// </summary>
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
    private Vector2 moveDelta;

    ///// <summary>
    ///// ターゲットを発見してるかの判定
    ///// </summary>
    //private bool hasPlayerTarget;

    /// <summary>
    /// プレイヤー情報
    /// </summary>
    private GameObject player;
    private Transform playerCenter;
    private Vector3 playerLastPos;
        
    /// <summary>
    /// 自身の情報
    /// </summary>
    private Vector3 startPos, movePos;

    /// <summary>
    /// 追いかけるスピード
    /// </summary>
    [SerializeField]
    private float chaseSpeed = 0.8f;

    /// <summary>
    /// 回転の遅延
    /// </summary>
    [SerializeField]
    private float turningDelay = 1f;

    /// <summary>
    /// 次に方向転換可能な時間
    /// </summary>
    [SerializeField]
    private float turningTimeDelay = 1f;

    /// <summary>
    /// プレイヤーの位置を最後に把握した時間
    /// </summary>
    private float lastFollowTime;

    /// <summary>
    /// 向き保存用
    /// </summary>
    private Vector3 tempScale;

    /// <summary>
    /// 敵のアニメーター
    /// </summary>
    [SerializeField]
    private Animator animator;

    /// <summary>
    /// 本体：向き変更用
    /// </summary>
    private Transform body;

    /// <summary>
    /// 敵のステータス管理クラス
    /// </summary>
    private EnemyStatusController enemyStatusController;

    /// <summary>
    /// 敵の攻撃クラス
    /// </summary>
    private EnemyAttack enemyAttack;
    #endregion

    #region プロパティ
    public Vector2    MoveDelta       => moveDelta;
    #endregion

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        player                = GameObject.FindWithTag("Player").gameObject;
        playerCenter          = player?.GetComponent<PlayerStatusController>().PlayerCenter;
        playerLastPos         = playerCenter.position;
        startPos              = transform.position;
        lastFollowTime        = Time.time;
        turningTimeDelay     *= turningDelay;
        enemyStatusController = GetComponent<EnemyStatusController>();
        enemyAttack           = GetComponent<EnemyAttack>();
        body                  = animator.gameObject.transform;

        if (animator == null)
        {
            animator = GameObject.FindGameObjectWithTag("EnemyAnimator").
                                  GetComponent<Animator>();
        }
    }

    /// <summary>
    /// 移動更新
    /// </summary>
    private void FixedUpdate()
    {
        //自身が死亡状態か、ダメージを受けてたら実行しない
        if (enemyStatusController.IsDead || enemyStatusController.IsDamage)
        {
            return;
        }

        MoveAnimation();
        TurnAround();
        ChaseingPlayer();
    }

    //プレイヤーを追いかける
    private void ChaseingPlayer()
    {
        //プレーヤーを発見
        if (enemyStatusController.HasPlayerTarget)
        {
            //攻撃していない場合
            if (!enemyAttack.IsAttacked)
            {
                //追いかける
                Chase();
            }
            else//攻撃した場合
            {
                //クールダウン経過していない
                if (!enemyAttack.IsDamageCoolDown)
                {
                    //プレイヤーから離れる？
                    //ノックバック？？
                }
                else//クールダウンが経過した
                {
                    //攻撃していないものとする
                    enemyAttack.IsAttacked = false;
                }
            }
        }
        else//発見していない場合
        {
            //何かしらの処理
        }

        //移動させる
        EnemyMove(movePos.x, movePos.y);
    }

    /// <summary>
    /// 発見時に追いかける
    /// </summary>
    private void Chase()
    {
        //まっすぐ進んだ判定
        if(Time.time - lastFollowTime > turningTimeDelay)
        {
            playerLastPos = player.transform.position;
            lastFollowTime = Time.time;
        }

        //十分離れてる：TODO　0.15ｆをどうする？
        if (Vector3.Distance(transform.position, playerLastPos) > PL_EN_DISTANCE)
        {
            movePos = (playerLastPos - transform.position).normalized * chaseSpeed;
        }
        else//十分近づいてる
        {
            //移動停止：攻撃？？
            movePos = Vector3.zero;
        }
    }


    /// <summary>
    /// 敵を移動させる
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    protected virtual void EnemyMove(float x, float y)
    {
        moveDelta = new Vector2(x * xSpeed, y * ySpeed);
        transform.Translate(moveDelta.x * Time.deltaTime,
                            moveDelta.y * Time.deltaTime, 0);
    }

    /// <summary>
    /// 向きの変更
    /// </summary>
    private void TurnAround()
    {
        tempScale = body.localScale;
        if (enemyStatusController.HasPlayerTarget)
        {
            if (playerCenter.position.x > transform.position.x)
            {
                tempScale.x = Mathf.Abs(tempScale.x);
            }

            if (playerCenter.position.x < transform.position.x)
            {
                tempScale.x = -Mathf.Abs(tempScale.x);
            }
            else//探知できていない場合は生成された位置によって向きを変更
            {
                if (startPos.x > transform.position.x)
                {
                    tempScale.x = Mathf.Abs(tempScale.x);
                }

                if (startPos.x < transform.position.x)
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
    private void MoveAnimation()
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
