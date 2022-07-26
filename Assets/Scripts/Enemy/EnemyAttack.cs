using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GlobalValue;
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

    /// <summary>
    /// ステータス操作
    /// </summary>
    private EnemyStatusController enemyStatusController;

    /// <summary>
    /// 移動操作
    /// </summary>
    private EnemyMovement enemyMovement;

    /// <summary>
    /// 自身のTransform
    /// </summary>
    private Transform trans;


    private void Start()
    {
        Initialize();   
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        enemyStatusController = GetComponent<EnemyStatusController>();
        enemyMovement         = GetComponent<EnemyMovement>();
        isAttacked            = false;
        damageCoolDownTimer   = 0;
        trans                 = this.gameObject.transform;
    }

    #region プロパティ
    public bool IsAttacked { get { return isAttacked; } 
                             set { isAttacked = value; } }
    /// <summary>
    /// クールダウン中判定
    /// </summary>
    public bool IsDamageCoolDown => Time.time < damageCoolDownTimer;
    #endregion

    /// <summary>
    /// 接触時の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレーヤーの場合
        if (collision.CompareTag("Player"))
        {
            //ダメージ中
            if (enemyStatusController.IsDamage)
                return;

            //クールダウン中
            if (IsDamageCoolDown)
                return;

            if (!isAttacked)
            {
                //クールダウン中は攻撃判定取れない
                damageCoolDownTimer = Time.time + damageCoolDown;
                isAttacked = true;
                collision.GetComponent<PlayerStatusController>().Damage(damageAmount);

                //ノックバック
                NockBack();

                return;
            }
        }//敵の場合
        else if (collision.CompareTag("Enemy"))
        {
            var enemyCtrl = collision.gameObject?.
                            GetComponent<EnemyStatusController>();
            if (enemyCtrl.State == ENEMY_STATE.DEATH)
                return;

            if(enemyStatusController.State == ENEMY_STATE.DAMAGE)
            {
                enemyCtrl.EnemyDamage();
            }
        }
    }

    /// <summary>
    /// ノックバック
    /// </summary>
    private void NockBack()
    {
        var nockbackPos = new Vector3();
        var delta = -(enemyMovement.MoveDelta);
        var distance = new Vector3(Mathf.Sign(delta.x), Mathf.Sign(delta.y), 0);

        nockbackPos.x = trans.position.x + (NOCKBACK_DIFF * distance.x);
        nockbackPos.y = trans.position.y + (NOCKBACK_DIFF * distance.y);

        this.transform.DOMove(nockbackPos, NOCKBACK_TIME);
    }
}
