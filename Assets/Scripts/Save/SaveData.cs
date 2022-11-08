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
    public GAMELEVEL GemeLevel;

    //プレイ回数
    public int PlayTime;

    //BGM音量
    public float BGM_Volume;

    //SE音量
    public float SE_Volume;

    //GameOver判定：途中データ用
    //public bool IsGameOver;

    //スタミナ回復用時間観測
}
