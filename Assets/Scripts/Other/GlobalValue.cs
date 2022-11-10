using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GlobalValue
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
    public readonly static float ATTACK_POWER = 30.0f;

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
    /// �v���C���[�ړ��͈͍ő�X
    /// </summary>
    public static readonly float PL_MAXMOVE_X = 4;

    /// <summary>
    /// �v���C���[�ړ��͈͍ŏ�X
    /// </summary>
    public static readonly float PL_MINMOVE_X = -4;

    /// <summary>
    /// �v���C���[�ړ��͈͍ő�Y
    /// </summary>
    public static readonly float PL_MAXMOVE_Y = 7;

    /// <summary>
    /// �v���C���[�ړ��͈͍ŏ�Y
    /// </summary>
    public static readonly float PL_MINMOVE_Y = -8;

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
    public static readonly float STAY_SHAKETIME = 1.0f;

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
    public static readonly float EN_CREATEPOS_RADIUS = 4.0f;

    /// <summary>
    /// �G��������X
    /// </summary>
    public static readonly float CREATE_DIFFX = 0.4f;

    /// <summary>
    /// �G��������Y
    /// </summary>
    public static readonly float CREATE_DIFFY = 0.5f;

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
    //public static float STAMINA_RECOVERY_TIME = 1800.0f;
    public static float STAMINA_RECOVERY_TIME = 3600.0f;

    /// <summary>
    /// �ő�X�^�~�i��
    /// </summary>
    public static int STAMINA_MAXNUMBER = 3;

    /// <summary>
    /// �X�^�[�g�J�n����
    /// </summary>
    public static float START_PLAYINGTIME = 2.0f;

    /// <summary>
    /// Fade����
    /// </summary>
    public static float FADETIME = 2.0f;

    /// <summary>
    /// ����Scale
    /// </summary>
    public static Vector3 INIT_SCALE = new Vector3(1.0f,1.0f,1.0f);

    /// <summary>
    /// �T�E���h�O���[�v
    /// </summary>
    public static string SOUNDGROUP_NID = "SoundGroup_";

    /// <summary>
    /// �J���������T�C�Y
    /// </summary>
    public static float CAMERA_INITSIZE = 8.5f;

    /// <summary>
    /// �J���������ʒu
    /// </summary>
    public static Vector3 CAMERA_INITPOS = new Vector3(0.0f, 0.0f, -10.0f);

    /// <summary>
    /// �ėp�_�C�A���O�̃p�X
    /// </summary>
    public static string COMMONDIALOG_PREFAB_NAME = "Prefabs/Common/CommonDialog";

    /// <summary>
    /// �X�^�~�i�񕜃_�C�A���O�̃p�X
    /// </summary>
    public static string STAMINADIALOG_PREFAB_NAME = "Prefabs/Common/StaminaRecoveryDialog";

    public readonly static string ISO_8601_FORMAT     = "yyyy-MM-ddTHH:mm:ss.fffZ";

    public readonly static float STOP_TIME     = Mathf.Infinity;

    /// <summary>
    /// �G�̐����ʒu�̍���
    /// </summary>
    public readonly static float ENEMY_CREATE_DIFF_MAX = 5.0f;
    public readonly static float ENEMY_CREATE_DIFF_MIN = 1.0f;

    /// <summary>
    /// �J�n���̓G�����x������
    /// </summary>
    public readonly static int START_CREATE_DIFF = 1000;

    /// <summary>
    /// ��ʂɕ\��������ő吔
    /// </summary>
    public readonly static int ENEMY_SCREEN_MAXCOUNT = 100;

    /// <summary>
    /// ���x���A�b�v��
    /// </summary>
    public readonly static int LEVELUP_COUNT = 50;

    /// <summary>
    /// �ő�Q�[�����x��
    /// </summary>
    public readonly static int MAX_GAMELEVEL = 100;


    //�G�L�����̈ړ��X�s�[�h�����F100�̌��j��
    //Chase�F�ŏ�0.5�ő�2�@�F����0.1
    //ZigZag�F�ŏ�0.5�ő�2�@�F����0.1
    //Around:�ŏ�0.5�ő�3�F0.1�@��]����2-5�F�����_���@�X�s�[�h2/10�F0.2
}
