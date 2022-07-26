using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using DG.Tweening;

/// <summary>
/// プレーヤーのステータス管理
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    /// <summary>
    /// 最大ライフ
    /// </summary>
    [SerializeField]
    private int maxLife;

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

    #region プロパティ
    public bool IsDead => isDead;

    public Transform PlayerCenter => playerCenter;
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
        sprite   = body.GetComponent<SpriteRenderer>();
        animator = body.gameObject.GetComponent<Animator>();    
        life     = startLife 
                 = START_LIFEPOINT;
        isDead   = false;
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
        if( life <= 0) //死亡処理
        {
            isDead = true;
            //接触判定を無くす
            Dead();
        }
        else//ダメージ処理
        {
            sprite.color = Color.red;

            transform.DOPunchScale(
                SHAKESTRENGTH,
                SHAKETIME).OnComplete(() =>
                {
                    sprite.color = Color.white;
                });
        }
    }

    /// <summary>
    /// ライフ回復
    /// </summary>
    public void RecoveryLife()
    {
        //最大値以上は回復しない
        if (life >= maxLife)
            return;

        life += RECOVERY_LIFEPOINT;
        //ライフアイコン点灯
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public void Dead()
    {
        //死亡アニメーション
        animator.SetTrigger("Dead");
        //スタミナを減らす
        //スタミナなければGAMEOVER
    }
   
}
 
