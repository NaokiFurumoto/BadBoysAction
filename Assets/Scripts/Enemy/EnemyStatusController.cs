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
    NOCKBACK,//ノックバック中
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
    //[SerializeField]
    //private bool isDamage;

    /// <summary>
    /// 本体
    /// </summary>
    [SerializeField]
    private Transform body;

    /// <summary>
    /// 本体
    /// </summary>
    [SerializeField]
    private GameObject spriteBody;

    /// <summary>
    /// 敵のタイプ
    /// </summary>
    [SerializeField]
    private ENEMY_MOVETYPE moveType;

    /// <summary>
    /// 移動処理クラス
    /// </summary>
    private EnemyMovement enemyMovement;

    /// <summary>
    /// 色変更用
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// 色変更用
    /// </summary>
    private TrailRenderer trail;

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
    /// Collider2D
    /// </summary>
    private CircleCollider2D collider;

    /// <summary>
    /// UIController
    /// </summary>
    private UiController uiController;

    /// <summary>
    /// Effect
    /// </summary>
    [SerializeField]
    private GameObject effect;

    /// <summary>
    /// Shadow
    /// </summary>
    [SerializeField]
    private GameObject shadow;

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
    //public bool IsDamage
    //{
    //    get { return isDamage; }
    //    set { isDamage = value; }
    //}
    public bool HasPlayerTarget => hasPlayerTarget;
    public Rigidbody2D Rigid2D => rigid2D;
    public int Life => life;
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
        InitializeComponent();

        state = ENEMY_STATE.NONE;

        //isDamage = false;
        isDead = false;

        trail.enabled = false;

        //体力設定
        life = enemyLifeAc.SetCreateLife();
        enemyLifeAc.SetLifeText(life);

        wallDamageTimes = 0;

        //発見判定
        hasPlayerTarget = true;
    }

    private void InitializeComponent()
    {
        rigid2D = GetComponent<Rigidbody2D>();

        collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;

        animator = spriteBody.gameObject.GetComponent<Animator>();
        sprite = spriteBody.GetComponent<SpriteRenderer>();

        trail = spriteBody.gameObject.GetComponent<TrailRenderer>();

        uiController = GameObject.FindGameObjectWithTag("UI").
                                  GetComponent<UiController>();

        enemyMovement = gameObject.GetComponent<EnemyMovement>();
        moveType = enemyMovement.MoveType;
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
    /// プレイヤーから攻撃を受けた時
    /// </summary>
    public void PlayerDamage(Vector2 direction, float power)
    {
        sprite.color = Color.red;
        body.DOPunchScale(
            SHAKESTRENGTH,
            SHAKETIME
        ).OnComplete(() =>
        {
            collider.isTrigger = false;
            sprite.color = Color.white;
            trail.enabled = true;
            animator.SetTrigger("Damage");
            shadow.SetActive(false);

            rigid2D.AddForce(direction * power, ForceMode2D.Impulse);
        });
    }

    /// <summary>
    /// 敵から攻撃を受けた時
    /// </summary>
    public void EnemyDamage(int _damage)
    {
        life -= _damage;
        enemyLifeAc.SetLifeText(life);

        if (life <= 0)
        {
            state = ENEMY_STATE.DEATH;
            rigid2D.simulated = false;
            shadow.SetActive(false);

            EnemyDead();
        }
        else
        {
            if (state == ENEMY_STATE.DAMAGE)
                return;

            state = ENEMY_STATE.NOCKBACK;
            sprite.color = Color.red;

            body.DOPunchPosition(
                STAY_SHAKESTRENGTH,
                STAY_SHAKETIME
            ).OnComplete(() =>
            {
                sprite.color = Color.white;
                state = ENEMY_STATE.MOVE;
            });
        }
    }

    /// <summary>
    /// 壁と衝突
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" && state == ENEMY_STATE.DAMAGE)
        {
            wallDamageTimes++;
        }

        if (life > 0 && !isDead)
        {
            if (wallDamageTimes >= life)
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
        animator.SetTrigger("Dead");
    }

    /// <summary>
    /// 死亡アニメーション完了後に呼ばれる
    /// </summary>
    public void DeadEndCallback()
    {
        //削除：更新するまでは残る
        Destroy(this.gameObject);
        uiController?.SetPlayKillsNumber();
    }

    /// <summary>
    /// エフェクト表示
    /// </summary>
    public void PlayEffect()
    {
        effect?.SetActive(true);
    }


    #region パラメーター変更
    /// <summary>
    /// ダメージ中のステータス変更
    /// </summary>
    public void SetDamageStatus()
    {
        this.gameObject.layer = 11;
        state = ENEMY_STATE.DAMAGE;
    }

    /// <summary>
    /// 死亡中のステータス変更
    /// </summary>
    public void SetDeadStatus()
    {
        life = 0;
        isDead = true;
        state = ENEMY_STATE.DEATH;
        rigid2D.simulated = false;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f); 

        enemyLifeAc.ChangeActiveLifeImage(false);
        trail.enabled = false;
    }

    //生成時の体力設定
    public void SetCreateLife()
    {
        //ライフは1から10まで
        //タイプによってライフの変更
        //ライフ割合ステータスクラスがいる
    }
    #endregion
}
