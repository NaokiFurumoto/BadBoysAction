using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using DG.Tweening;
using System;

/// <summary>
/// プレーヤーのステータス管理
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    /// <summary>
    /// 現在のライフ
    /// </summary>
    [SerializeField]
    private int life;

    /// <summary>
    /// 初期ライフ
    /// </summary>
    [SerializeField]
    private int startLife;

    [SerializeField]
    private Transform playerCenter;

    /// <summary>
    /// 本体
    /// </summary>
    [SerializeField]
    private Transform body;

    /// <summary>
    /// 攻撃Root
    /// </summary>
    [SerializeField]
    private GameObject attackRoot;

    /// <summary>
    /// 色変更用
    /// </summary>
    private SpriteRenderer sprite;

    /// <summary>
    /// 色変更用
    /// </summary>
    private Transform spriteTransform;

    /// <summary>
    /// アニメーション
    /// </summary>
    private Animator animator;

    /// <summary>
    /// 攻撃クラス
    /// </summary>
    private AttackerManager  attackerManager;

    /// <summary>
    /// 死亡判定
    /// </summary>
    private bool isDead;

    /// <summary>
    /// Life管理
    /// </summary>
    private LifesManager lifesManager;

    /// <summary>
    /// ゲーム管理
    /// </summary>
    private GameController gameController;

    /// <summary>
    /// ゲーム開始位置
    /// </summary>
    private Vector2 startPosition;

#if UNITY_EDITOR
    /// <summary>
    /// 無敵モード
    /// </summary>
    [SerializeField]
    protected bool MUTEKI = false;
#endif

    #region プロパティ
    public bool IsDead => isDead;

    public Transform PlayerCenter => playerCenter;
    #endregion

    #region コールバック
    public Action OnComplate;
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

        spriteTransform = sprite.transform;
        isDead   = false;

        //無敵ならば
        if (MUTEKI)
        {
            life = 1000;
        }

        lifesManager?.SetLife(life);
        startPosition = transform.position;
        attackRoot.SetActive(true);
    }

    private void InitializeComponent()
    {
        sprite = body.GetComponent<SpriteRenderer>();
        animator = body.gameObject.GetComponent<Animator>();
        lifesManager = GameObject.FindGameObjectWithTag("LifesRoot").
                                  GetComponent<LifesManager>();
        gameController = GameObject.FindGameObjectWithTag("GameController").
                                    GetComponent<GameController>();
        attackerManager = GameObject.FindGameObjectWithTag("PlayerAttack").
                                    GetComponent<AttackerManager>();
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        if (life <= 0 || isDead)
            return;

        life -= damage;
        lifesManager.SetLife(life);

        if ( life <= 0) //死亡処理
        {
            Dead();
        }
        else//ダメージ処理
        {
            //センサーの非表示

            //リアクション
            sprite.color = Color.red;
            spriteTransform.DOPunchScale(
                SHAKESTRENGTH,
                SHAKETIME).OnComplete(() =>
                {
                    sprite.color = Color.white;
                    spriteTransform.localScale = INIT_SCALE;
                });
        }
    }

    /// <summary>
    /// ライフ回復
    /// </summary>
    public void RecoveryLife()
    {
        //最大値以上は回復しない
        if (life >= MAX_LIFEPOINT)
        {
            OnComplate();
            return;
        }

        life += RECOVERY_LIFEPOINT;

        //ライフアイコン点灯
        lifesManager?.SetLife(life);

        //ライフアイコン削除
        OnComplate();
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public void Dead()
    {
        isDead = true;
        attackRoot.SetActive(false);
        animator.SetTrigger("Dead");
    }

    /// <summary>
    /// リトライ処理
    /// </summary>
    public void RetryPlayer()
    {
        var player = this.gameObject;
        if (!player.activeSelf)
        {
            player.SetActive(true);
        }
        player.transform.position = startPosition;
        attackerManager.ActivateAttacker(ATTACK_DIRECTION.FRONT);

        isDead = false;
        life = 1;
        lifesManager.SetLife(life);

        attackRoot.SetActive(true);
    }
   
}
 
