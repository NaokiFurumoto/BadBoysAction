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
    private int damageAmount = 1;

    /// <summary>
    /// 攻撃判定
    /// </summary>
    [SerializeField]
    private bool isAttacked = false;

    /// <summary>
    /// クールダウン時間
    /// </summary>
    [SerializeField]
    private float damageCoolDown = 1f;

    /// <summary>
    /// クールダウン計測用
    /// </summary>
    private float damageCoolDownTimer;

    #region プロパティ
    public bool IsAttacked { get { return isAttacked; } 
                             set { isAttacked = value; } }
    /// <summary>
    /// クールダウンが経過したかどうか
    /// </summary>
    public bool IsDamageCoolDown => Time.time > damageCoolDownTimer;
    #endregion

    /// <summary>
    /// 接触時の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーに接触
        if (collision.CompareTag("Player") && !isAttacked)
        {
            //攻撃を受けてなければ
            damageCoolDownTimer = Time.time + damageCoolDown;
            isAttacked = true;
            collision.GetComponent<PlayerStatusController>().Damage(damageAmount);
        }
    }
}
