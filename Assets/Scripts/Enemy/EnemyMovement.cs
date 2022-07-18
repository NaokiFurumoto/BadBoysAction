using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    /// <summary>
    /// ターゲットを発見してるかの判定
    /// </summary>
    private bool hasPlayerTarget;

    /// <summary>
    /// プレイヤー情報
    /// </summary>
    private Transform player;
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

    private EnemyStatusController enemyController;
    #endregion

    #region プロパティ
    public Vector2    MoveDelta       => moveDelta;
    public bool       HasPlayerTarget => hasPlayerTarget;
    public GameObject PlayerObject    => player.gameObject;
    #endregion

    private void Start()
    {
        player            = GameObject.FindWithTag("Player").transform;
        playerLastPos     = player.position;
        startPos          = transform.position;
        lastFollowTime    = Time.time;
        turningTimeDelay *= turningDelay;
        enemyController   = GetComponent<EnemyStatusController>();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {

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
}
