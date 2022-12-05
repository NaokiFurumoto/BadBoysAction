using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �Z�[�u�f�[�^�p�N���X
/// </summary>
[Serializable]
public class SaveData 
{
    //�g�p�ł���X�^�~�i��
    public int StaminaNumber;

    //���j��
    public int KillsNumber;

    //�n�C�X�R�A
    public int HiScoreNumber;

    //���C�t��
    public int LifeNumber;

    //�X�e�[�W���x��
    public int GemeLevel;

    //�v���C��
    public int PlayTime;

    //BGM����
    public float BGM_Volume;

    //SE����
    public float SE_Volume;

    //�L���\������F�ۋ��@�\
    public bool IsShowAds;

    //���f��Ԃ̔���
    public bool IsBreak;

    //�Z�[�u��������
    public long saveTime;

    //���x���A�b�v�p�J�E���g��
    public int changeKillCount;

    //�K�v�o���l
    public int levelupNeedCount;

    //�����Ԋu����
    public float createDelayTime;

    //�G��ʕ\����
    public int enemyScreenDisplayIndex;

    //�Q�[���̏��
    public INGAME_STATE gameState;

    //���������ʕ\��
    public bool IsFirstViewOpen;

    //�C�����[�h�J���FLV99���B���A�ۋ��A�X�^�~�i����

}
