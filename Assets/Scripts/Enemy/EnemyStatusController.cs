using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵の状態管理クラス
/// </summary>

///状態
public enum ENEMY_STATE
{
    NONE,//未設定
    IDLE,//待機
    MOVE,//移動中
    ATTACK,//攻撃中
    DAMAGE,//ダメージを受けてる
    POOL,//Poolされてる
    DEATH,//死亡
}
public class EnemyStatusController : MonoBehaviour
{
    /// <summary>
    /// 体力
    /// </summary>
    [SerializeField]
    private int Life = 1;

    /// <summary>
    /// ターゲットを発見してるかの判定
    /// </summary>
    private bool hasPlayerTarget;

    /// <summary>
    /// 敵の状態
    /// </summary>
    [SerializeField]
    private ENEMY_STATE state;

    /// <summary>
    /// ダメージ判定
    /// </summary>
    [SerializeField]
    private bool isDamage;

    /// <summary>
    /// 死亡判定
    /// </summary>
    private bool isDead;

    /// <summary>
    /// RigidBody2D
    /// </summary>
    private Rigidbody2D rigid2D;

    /// <summary>
    /// RigidBody2D
    /// </summary>
    private CircleCollider2D collider2D;


    #region プロパティ
    public ENEMY_STATE State
    {
        get { return state; }
        set { state = value; }
    }
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }
    public bool IsDamage
    {
        get { return isDamage; }
        set { isDamage = value; }
    }
    public bool HasPlayerTarget => hasPlayerTarget;
    public Rigidbody2D Rigid2D => rigid2D;
    public CircleCollider2D Collider2D => collider2D;
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
        rigid2D    = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CircleCollider2D>();
        collider2D.isTrigger = true;
       // state      = ENEMY_STATE.NONE;
        isDamage   = false;
        isDead     = false;
        //TODO:とりあえず発見
        hasPlayerTarget = true;
    }

    /// <summary>
    /// 状態設定
    /// </summary>
    /// <param name="state"></param>
    public void SetEnemyState(ENEMY_STATE state)
    {
        this.state = state;
    }

    /// <summary>
    /// ダメージ中のステータス変更
    /// </summary>
    public void SetDamageStatus()
    {
        isDamage = true;
        state = ENEMY_STATE.DAMAGE;
        collider2D.isTrigger = false;
    }

    /// <summary>
    /// 攻撃を受けた時
    /// </summary>
    public void Damage(Vector2 direction,float power)
    {
        rigid2D.AddForce(direction * power, ForceMode2D.Impulse);
    }
}
