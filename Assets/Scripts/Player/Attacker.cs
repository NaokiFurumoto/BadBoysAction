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

    #region プロパティ
    /// <summary>
    /// 攻撃方向の取得
    /// </summary>
    public ATTACK_DIRECTION AttackType => attackType;
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
            var _rigid2D = _enemyStatus.Rigid2D;

            //ダメージを受けていない状態かつ敵が移動中
            if(!_enemyStatus.IsDamage && _enemyStatus.State == ENEMY_STATE.MOVE)
            {
                animator.SetTrigger("Attack");

                _enemyStatus.SetDamageStatus();
                _enemyStatus.Damage(playerMovement.Direction.normalized, ATTACK_POWER);
            }
        }
    }

    /// <summary>
    /// センサーに衝突中
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
    }

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
}
