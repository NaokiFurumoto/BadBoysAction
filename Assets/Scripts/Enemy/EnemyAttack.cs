using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�U���N���X
/// </summary>
public class EnemyAttack : MonoBehaviour
{
    /// <summary>
    /// �U����
    /// </summary>
    [SerializeField]
    private int damageAmount = 1;

    /// <summary>
    /// �U������
    /// </summary>
    [SerializeField]
    private bool isAttacked = false;

    /// <summary>
    /// �N�[���_�E������
    /// </summary>
    [SerializeField]
    private float damageCoolDown = 1f;

    /// <summary>
    /// �N�[���_�E���v���p
    /// </summary>
    private float damageCoolDownTimer;
}
