using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : MonoBehaviour
{
    /// <summary>
    /// �v���C���[�ƓG�̍ŏ��ڋߋ���
    /// </summary>
    public readonly static float PL_EN_DISTANCE = 0.2f;

    /// <summary>
    /// �G����΂��ꂽ���̕ǂ̐ڐG��
    /// </summary>
    public readonly static int WALL_DAMAGETIMES = 3;

    /// <summary>
    /// �v���C���[�̍U����
    /// </summary>
    public readonly static float ATTACK_POWER = 20.0f;

    /// <summary>
    /// �v���C���[�̍ő僉�C�t�l
    /// </summary>
    public readonly static int MAX_LIFEPOINT = 5;

    /// <summary>
    /// �v���C���[�̍ŏ����C�t�l
    /// </summary>
    public readonly static int START_LIFEPOINT = 2;

    /// <summary>
    /// �G�̍ő僉�C�t�l
    /// </summary>
    public readonly static int ENMAX_LIFEPOINT = 100;

    /// <summary>
    /// �v���C���[���󂯂�_���[�W
    /// </summary>
    public static readonly int PLAYER_DAMAGE = 1;

    /// <summary>
    /// ���C�t�񕜒l
    /// </summary>
    public readonly static int RECOVERY_LIFEPOINT = 1;

    /// <summary>
    /// �V�F�C�N����
    /// </summary>
    public static readonly float SHAKETIME = 0.3f;

    /// <summary>
    /// �V�F�C�N�̋���
    /// </summary>
    public static readonly Vector3 SHAKESTRENGTH = new Vector3(1.2f, 1.2f, 1.2f);

    /// <summary>
    /// Stay�V�F�C�N����
    /// </summary>
    public static readonly float STAY_SHAKETIME = 0.6f;

    /// <summary>
    /// Stay_�V�F�C�N�̋���
    /// </summary>
    public static readonly Vector3 STAY_SHAKESTRENGTH = new Vector3(0.0f, 0.2f, 0.0f);

    /// <summary>
    /// ���񔼌a
    /// </summary>
    public static readonly float EN_AROUND = 2.0f;

    /// <summary>
    /// �m�b�N�o�b�N�ړ�����
    /// </summary>
    public static readonly float NOCKBACK_DIFF = 0.5f;

    /// <summary>
    /// �m�b�N�o�b�N�ړ�����
    /// </summary>
    public static readonly float NOCKBACK_TIME = 0.3f;

}
