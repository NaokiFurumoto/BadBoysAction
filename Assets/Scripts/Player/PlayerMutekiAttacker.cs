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
                //位置補正
                var itemSetPos = _enemyStatus.transform.position;
                itemSetPos.x = Mathf.Clamp(itemSetPos.x,-4.0f, 4.0f);
                itemSetPos.y = Mathf.Clamp(itemSetPos.y,-6.5f, 8.0f);

                //アイテム抽選
                ItemController.Instance.DropItemLottery(itemSetPos);

                _enemyStatus.SetDamageStatus();
                _enemyStatus.PlayEffect();
                _enemyStatus.PlayerDamage(playerMovement.Direction.normalized, ATTACK_POWER);
                CameraAction.EnemyDamage();
                return;
            }
        }
    }
}
