using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵攻撃クラス
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    /// <summary>
    /// 攻撃力
    /// </summary>
    [SerializeField]
    private int damageAmount;

    /// <summary>
    /// 攻撃判定
    /// </summary>
    [SerializeField]
    private bool isAttacked;

    /// <summary>
    /// クールダウン時間
    /// </summary>
    [SerializeField]
    private float damageCoolDown;

    /// <summary>
    /// クールダウン計測用
    /// </summary>
    private float damageCoolDownTimer;

    private void Start()
    {
        isAttacked = false;
        damageCoolDownTimer = 0;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        
    }

    #region プロパティ
    public bool IsAttacked { get { return isAttacked; } 
                             set { isAttacked = value; } }
    /// <summary>
    /// クールダウンが経過したかどうか
    /// </summary>
    public bool IsDamageCoolDown => Time.time > damageCoolDownTimer;
    #endregion

    /// <summary>
    /// プレイヤーと接触時の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsDamageCoolDown)
            return;

        //プレイヤーに接触:攻撃してない場合
        if (collision.CompareTag("Player") && !isAttacked)
        {
            //クールダウン中は攻撃判定取れない
            damageCoolDownTimer = Time.time + damageCoolDown;
            isAttacked = true;
            collision.GetComponent<PlayerStatusController>().Damage(damageAmount);
            //ノックバックさせる？
            return;
        }
    }
}
