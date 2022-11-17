using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃の管理 / 切替
/// </summary>
public class AttackerManager : MonoBehaviour
{
    /// <summary>
    /// すべての攻撃クラス
    /// </summary>
    [SerializeField]
    private Attacker[] attackers;

    /// <summary>
    /// 選択中の攻撃クラス
    /// </summary>
    private Attacker currentAttacker;

    /// <summary>
    /// 攻撃：外部参照用
    /// </summary>
    public Attacker CurrentAttacker => currentAttacker;

    private void Start()
    {
        DeActivateAllAttackers();
        Initialize();
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    private void Initialize()
    {
        //設定されていなければ
        if(attackers.Length == 0)
        {
            int childCount = transform.childCount; 
            attackers = new Attacker[childCount];
            for (int i = 0; childCount > i; i++)
            {
                attackers[i] = transform.GetChild(i).gameObject.GetComponent<Attacker>();
            }
        }

        //初期はFront
        currentAttacker = attackers[0];
        currentAttacker.gameObject.SetActive(true);
    }

    /// <summary>
    /// すべての攻撃状態を非表示に
    /// </summary>
    public void DeActivateAllAttackers()
    {
        for (int i = 0; i < attackers.Length; i++)
        {
            attackers[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 特定の攻撃を表示する
    /// </summary>
    /// <param name="direction"></param>
    public void ActivateAttacker(ATTACK_DIRECTION direction)
    {
        if (direction == ATTACK_DIRECTION.NONE)
            return;

        for (int i = 0; i < attackers.Length; i++)
        {
            if(attackers[i].AttackType == direction)
            {
                currentAttacker.gameObject.SetActive(false);
                currentAttacker = attackers[i];
                currentAttacker.gameObject.SetActive(true);
            }
        }
    }

    
    /// <summary>
    /// 全Attackのアニメーションのリセット
    /// </summary>
    public void SetAllAnimatorIdle()
    {
        foreach(var anim in this.attackers)
        {
            anim?.SetAnimationIdle();
        }
    }
}
