using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GlobalValue;
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
    private int life = 1;

    /// <summary>
    /// EnemyLifeAction
    /// </summary>
    [SerializeField]
    private EnemyLifeAction enemyLifeAc;

    /// <summary>
    /// 壁に当たった回数
    /// </summary>
    [SerializeField]
    private int wallDamageTimes;

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
    /// 本体
    /// </summary>
    [SerializeField]
    private Transform body;

    /// <summary>
    /// 色変更用
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// アニメーション
    /// </summary>
    private Animator animator;

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
    private CircleCollider2D collider;

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
        rigid2D     = GetComponent<Rigidbody2D>();
        collider    = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;

        animator    = body.gameObject.GetComponent<Animator>();
        sprite      = body.GetComponent<SpriteRenderer>();

        state       = ENEMY_STATE.NONE;
        isDamage    = false;
        isDead      = false;

        enemyLifeAc.SetLifeText(life);

        wallDamageTimes = 0;
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
    /// 攻撃を受けた時
    /// </summary>
    public void Damage(Vector2 direction,float power)
    {
        sprite.color = Color.red;

        body.DOShakeScale(
            duration: ENEMY_SHAKETIME,
            strength: ENEMY_SHAKESTRENGTH
        ).OnComplete(() =>
        {
            sprite.color = Color.white;
            rigid2D.AddForce(direction * power, ForceMode2D.Impulse);
        });
    }

    /// <summary>
    /// 壁と衝突
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && isDamage)
        {
            wallDamageTimes++;
        }

        if (life > 0 && !isDead)
        {
            if (wallDamageTimes >= WALL_DAMAGETIMES)
            {
                EnemyDead();
            }
        }
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public void EnemyDead()
    {
        SetDeadStatus();
        //死亡アニメーション
        animator.SetTrigger("Dead");
    }

    /// <summary>
    /// 死亡アニメーション完了後に呼ばれる
    /// </summary>
    public void DeadEndCallback()
    {
        //一旦非表示
        this.gameObject.SetActive(false);
    }


    #region パラメーター変更
    /// <summary>
    /// ダメージ中のステータス変更
    /// </summary>
    public void SetDamageStatus()
    {
        isDamage = true;
        state = ENEMY_STATE.DAMAGE;
        collider.isTrigger = false;
    }

    /// <summary>
    /// 死亡中のステータス変更
    /// </summary>
    public void SetDeadStatus()
    {
        life    = 0;
        isDead  = true;
        state   = ENEMY_STATE.DEATH;
        rigid2D.simulated = false;  
    }
    #endregion
}
