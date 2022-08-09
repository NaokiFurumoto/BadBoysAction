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
    public readonly static int WALL_DAMAGETIMES = 4;

    /// <summary>
    /// �v���C���[�̍U����
    /// </summary>
    public readonly static float ATTACK_POWER = 20.0f;

    /// <summary>
    /// �v���C���[�̍ő僉�C�t�l
    /// </summary>
    public readonly static int MAX_LIFEPOINT = 10;

    /// <summary>
    /// �v���C���[�̍ŏ����C�t�l
    /// </summary>
    public readonly static int START_LIFEPOINT = 1;

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
    public static readonly float SHAKETIME = 0.4f;

    /// <summary>
    /// �V�F�C�N�̋���
    /// </summary>
    public static readonly Vector3 SHAKESTRENGTH = new Vector3(1.2f, 1.2f, 1.2f);

    /// <summary>
    /// Stay�V�F�C�N����
    /// </summary>
    public static readonly float STAY_SHAKETIME = 0.7f;

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

    /// <summary>
    /// �G�������a
    /// </summary>
    public static readonly float EN_CREATEPOS_RADIUS = 2.0f;

    /// <summary>
    /// �G��������X
    /// </summary>
    public static readonly float CREATE_DIFFX = 0.4f;

    /// <summary>
    /// �G��������Y
    /// </summary>
    public static readonly float CREATE_DIFFY = 0.5f;

    //�ŏ��������ԁF�ő吶������

    /// </summary>
    /// 1�̐�����ŕ\�������ő吔
    /// <summary>
    public static readonly int MAX_GE_CREATECOUNT = 30;

    /// <summary>
    /// �h���b�v�A�C�e���z�u�ʒu�ŏ�X
    /// </summary>
    public static readonly float DROPITEM_POSX_MIN = -2.5f;

    /// <summary>
    /// �h���b�v�A�C�e���z�u�ʒu�ő�X
    /// </summary>
    public static readonly float DROPITEM_POSX_MAX = 2.5f;

    /// <summary>
    /// �h���b�v�A�C�e���z�u�ʒu�ŏ�Y
    /// </summary>
    public static readonly float DROPITEM_POSY_MIN = -4.2f;

    /// <summary>
    /// �h���b�v�A�C�e���z�u�ʒu�ő�Y
    /// </summary>
    public static readonly float DROPITEM_POSY_MAX = 4.2f;

    /// <summary>
    /// �X�^�~�i�񕜎���
    /// </summary>
    public static float STAMINA_RECOVERY_TIME = 300.0f;

    /// <summary>
    /// �ő�X�^�~�i��
    /// </summary>
    public static int STAMINA_MAXNUMBER = 3;

    /// <summary>
    /// �X�^�[�g�J�n����
    /// </summary>
    public static float START_PLAYINGTIME = 2.0f;

    //�G�L�����̈ړ��X�s�[�h�����F100�̌��j��
    //Chase�F�ŏ�0.5�ő�2�@�F����0.1
    //ZigZag�F�ŏ�0.5�ő�2�@�F����0.1
    //Around:�ŏ�0.5�ő�3�F0.1�@��]����2-5�F�����_���@�X�s�[�h2/10�F0.2
}
