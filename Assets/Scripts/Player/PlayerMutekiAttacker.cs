using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

public class PlayerMutekiAttacker : MonoBehaviour
{
    /// <summary>
    /// 移動クラス
    /// </summary>
    private PlayerMovement playerMovement;

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

            if (_enemyStatus.State == ENEMY_STATE.DAMAGE)
                return;

            if (_enemyStatus.State == ENEMY_STATE.NOCKBACK || _enemyStatus.State == ENEMY_STATE.MOVE)
            {
                //アイテム抽選
                ItemController.Instance.DropItemLottery(_enemyStatus.transform.position);

                _enemyStatus.SetDamageStatus();
                _enemyStatus.PlayEffect();
                _enemyStatus.PlayerDamage(playerMovement.Direction.normalized, ATTACK_POWER);
                CameraAction.EnemyDamage();
                return;
            }
        }
    }
}
