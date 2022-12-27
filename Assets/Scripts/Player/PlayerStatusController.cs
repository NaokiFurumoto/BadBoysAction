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
    /// 無敵Root
    /// </summary>
    [SerializeField]
    private GameObject mutekiRoot;

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
    /// クールタイム判定
    /// </summary>
    private bool isCoolTimeCheck;

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

    /// <summary>
    /// トロフィー無敵モード
    /// </summary>
    private bool isMuteki;

    /// <summary>
    /// 無敵モード
    /// </summary>
    [SerializeField]
    protected bool MUTEKI = false;

    /// <summary>
    /// キャッシュ用
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;

    #region プロパティ
    public bool IsDead => isDead;
    public bool IsMuteki => isMuteki;

    public Transform PlayerCenter => playerCenter;

    public SpriteRenderer Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }

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
        FM = SoundManager.Instance;
        appSound = AppSound.Instance;

        spriteTransform = sprite?.transform;
        //sprite.color = Color.red;
        isCoolTimeCheck = false;
        isDead   = false;
        isMuteki = false;

#if UNITY_EDITOR
        //無敵ならば
        if (MUTEKI)
        {
            life = 1000;
            lifesManager.SetLife(life);
        }
#endif

        startPosition = transform.position;
        attackRoot.SetActive(true);
        mutekiRoot.SetActive(false);
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
    /// 無敵開始処理
    /// アイテム取得後呼ばれる
    /// </summary>
    /// <param name="isMuteki">無敵判定</param>
    public void MutekiAttack()
    {
        //取得アイテム削除
        OnComplate();

        if (isMuteki)
        {
            StopCoroutine("MutekiActions");
            PlayerEffectManager.Instance.DeleteSelectEffects(EFFECT_TYPE.MUTEKI);
        }

        SwitchAttackType(true);
        var effect = PlayerEffectManager.Instance.EffectPlay(EFFECT_TYPE.MUTEKI);
        StartCoroutine("MutekiActions", effect);
        isMuteki = true;
    }

    /// <summary>
    /// 無敵アクション中
    /// </summary>
    /// <returns></returns>
    private IEnumerator MutekiActions(GameObject effect)
    {
        yield return new WaitForSeconds(MUTEKI_TIMES);

        SwitchAttackType(false);
        PlayerEffectManager.Instance.DeleteEffect(effect);
        isMuteki = false;
    }

    /// <summary>
    /// 攻撃方法切替
    /// </summary>
    /// <param name="isMuteki">無敵判定</param>
    private void SwitchAttackType(bool isMuteki)
    {
        //攻撃アニメーションの初期化
        attackerManager.SetAllAnimatorIdle();

        attackRoot.SetActive(!isMuteki);
        mutekiRoot.SetActive(isMuteki);
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        if (life <= 0 || isDead || isCoolTimeCheck || isMuteki)
            return;

        isCoolTimeCheck = true;
        life -= damage;
        lifesManager.SetLife(life);

        //カメラアクション
        CameraAction.PlayerDamage();
        animator.SetTrigger("Damage");

        if ( life <= 0) //死亡処理
        {
            FM.FadeOutVolume(appSound.BGM_STAGE, 0.0f, 0.5f, false);
            isCoolTimeCheck = false;
            Dead();
        }
        else//ダメージ処理
        {
            FM.PlayOneShot(appSound.SE_PL_DAMAGE);
            //リアクション
            spriteTransform.DOPunchScale(
                PLAYER_SHAKESTRENGTH,
                PLAYER_SHAKETIME).OnComplete(() =>
                {
                    spriteTransform.localScale = INIT_SCALE;
                    isCoolTimeCheck = false;
                });
        }
    }

    /// <summary>
    /// プレイヤーのライフ設定
    /// </summary>
    /// <param name="lifeNum"></param>
    public void PlayerSetLife(int lifeNum)
    {
        life = lifeNum;
    }

    /// <summary>
    /// ライフ回復
    /// </summary>
    public void RecoveryLife()
    {
        PlayerEffectManager.Instance.EffectPlay(EFFECT_TYPE.LIFE_RECOVERY);

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
        FM.PlayOneShot(appSound.SE_PL_DEATH);
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

        attackerManager?.ActivateAttacker(ATTACK_DIRECTION.FRONT);

        isDead = false;
        life = 1;
        lifesManager.SetLife(life);

        attackRoot.SetActive(true);
    }
   
}
 
