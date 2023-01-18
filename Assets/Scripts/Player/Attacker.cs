using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// キャラクターの攻撃に関するクラス
/// </summary>

//攻撃方向
public enum ATTACK_DIRECTION
{
    NONE,
    FRONT,      //正面
    UP,         //上
    SIDE,       //横
    SIDEUP,     //斜め上
    SIDEDOWN    //斜め下
}

public class Attacker : MonoBehaviour
{
    /// <summary>
    /// Sprite
    /// </summary>
    [SerializeField]
    private GameObject sprite;

    /// <summary>
    /// アニメーション
    /// </summary>
    private Animator animator;

    /// <summary>
    /// 攻撃の方向
    /// </summary>
    [SerializeField]
    private ATTACK_DIRECTION attackType;

    /// <summary>
    /// 移動クラス
    /// </summary>
    private PlayerMovement playerMovement;

    /// <summary>
    /// キャッシュ用
    /// </summary>
    private AppSound appSound;
    private SoundManager FM;

    #region プロパティ
    /// <summary>
    /// 攻撃方向の取得
    /// </summary>
    public ATTACK_DIRECTION AttackType => attackType;
    public Animator AttackAnimator => animator;
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
        playerMovement = GameObject.FindGameObjectWithTag("Player").
                                    GetComponent<PlayerMovement>();

        animator = sprite?.GetComponent<Animator>();

        //キャッシュ
        appSound = AppSound.Instance;
        FM = SoundManager.Instance;
    }

    /// <summary>
    /// センサーに衝突
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var _enemyStatus = collision.GetComponent<EnemyStatusController>();

            if (_enemyStatus.State == ENEMY_STATE.DAMAGE)
                return;

            if(_enemyStatus.State == ENEMY_STATE.NOCKBACK || _enemyStatus.State == ENEMY_STATE.MOVE)
            {
                //SE
                FM.PlayOneShot(appSound.SE_PL_ATK);

                //位置補正
                var itemSetPos = _enemyStatus.transform.position;
                itemSetPos.x = Mathf.Clamp(itemSetPos.x, -4.0f, 4.0f);
                itemSetPos.y = Mathf.Clamp(itemSetPos.y, -6.5f, 8.0f);

                //アイテムドロップ抽選
                ItemController.Instance.DropItemLottery(itemSetPos);

                animator.SetTrigger("Attack");
                _enemyStatus.SetDamageStatus();
                _enemyStatus.PlayEffect();
                _enemyStatus.PlayerDamage(playerMovement.Direction.normalized, ATTACK_POWER);
                CameraAction.EnemyDamage();
                return;
            }
        }
    }

    /// <summary>
    /// センサーに衝突中
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision) { }


    /// <summary>
    /// センサーから抜けた
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //攻撃したキャラが抜けたら
        }
    }

    /// <summary>
    /// アニメーションの初期化設定
    /// 初期ステートに戻す
    /// </summary>
    public void SetAnimationIdle()
    {
        this.animator?.Play("idle");
    }
}
