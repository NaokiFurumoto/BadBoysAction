using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレーヤーのステータス管理
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    /// <summary>
    /// 最大ライフ
    /// TODO:ゲームマネージャに持たせる？
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
    /// TODO:ゲームマネージャに持たせる？
    /// </summary>
    [SerializeField]
    private int startLife = 1;

    [SerializeField]
    private Transform playerCenter;

    /// <summary>
    /// TODO:ゲームマネージャに持たせる
    /// </summary>
    public static readonly int DAMAGE = 1;

    /// <summary>
    /// TODO:ゲームマネージャに持たせる
    /// </summary>
    public static readonly int LIFEPOINT = 1;

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
        //初回ならば
        life = startLife;
        isDead = false;
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
        if( life <= 0)
        {
            isDead = true;
            //死亡処理
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

        life += LIFEPOINT;
        //ライフアイコン点灯
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public void Dead()
    {
        //死亡アニメーション
        //スタミナを減らす
        //スタミナなければGAMEOVER
    }
   
}
 
