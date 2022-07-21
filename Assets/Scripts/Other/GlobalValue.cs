using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�ƓG�̍ŏ��ڋߋ���
    /// </summary>
    public readonly static float PL_EN_DISTANCE = 0.05f;

    /// <summary>
    /// �G����΂��ꂽ���̕ǂ̐ڐG��
    /// </summary>
    public readonly static int WALL_DAMAGETIMES = 3;

    /// <summary>
    /// �v���C���[�̍U����
    /// </summary>
    public readonly static float ATTACK_POWER = 30.0f;

    /// <summary>
    /// �v���C���[�̍ő僉�C�t�l
    /// </summary>
    public readonly static int MAX_LIFEPOINT = 5;

    /// <summary>
    /// �v���C���[�̍ŏ����C�t�l
    /// </summary>
    public readonly static int START_LIFEPOINT = 1;

    /// <summary>
    /// �v���C���[���󂯂�_���[�W
    /// </summary>
    public static readonly int PLAYER_DAMAGE = 1;

    /// <summary>
    /// ���C�t�񕜒l
    /// </summary>
    public readonly static int RECOVERY_LIFEPOINT = 1;

    /// <summary>
    /// �G�L�����V�F�C�N����
    /// </summary>
    public static readonly float ENEMY_SHAKETIME = 0.3f;

    /// <summary>
    /// �G�L�����V�F�C�N�̋���
    /// </summary>
    public static readonly float ENEMY_SHAKESTRENGTH = 1.2f;

}
