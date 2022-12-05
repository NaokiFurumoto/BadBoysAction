using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// セーブデータ用クラス
/// </summary>
[Serializable]
public class SaveData 
{
    //使用できるスタミナ数
    public int StaminaNumber;

    //撃破数
    public int KillsNumber;

    //ハイスコア
    public int HiScoreNumber;

    //ライフ数
    public int LifeNumber;

    //ステージレベル
    public int GemeLevel;

    //プレイ回数
    public int PlayTime;

    //BGM音量
    public float BGM_Volume;

    //SE音量
    public float SE_Volume;

    //広告表示判定：課金機能
    public bool IsShowAds;

    //中断状態の判定
    public bool IsBreak;

    //セーブした時間
    public long saveTime;

    //レベルアップ用カウント数
    public int changeKillCount;

    //必要経験値
    public int levelupNeedCount;

    //生成間隔時間
    public float createDelayTime;

    //敵画面表示数
    public int enemyScreenDisplayIndex;

    //ゲームの状態
    public INGAME_STATE gameState;

    //初回説明画面表示
    public bool IsFirstViewOpen;

    //修羅モード開放：LV99到達か、課金、スタミナ無限

}
