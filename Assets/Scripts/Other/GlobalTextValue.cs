﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 主にテキストの表示クラス
/// </summary>
public partial class GlobalValue
{

    public readonly static string YES = "はい";
    public readonly static string NO = "いいえ";
    public readonly static string OK = "OK";
    public readonly static string CLOSE = "閉じる";
    public readonly static string MOVIECHECK = "視聴";
    public readonly static string LOADBATTLE_YES = "再開する";
    public readonly static string LOADBATTLE_NO = "最初から";
   

    /// <summary>
    /// スタミナ全回復ダイアログタイトル
    /// </summary>
    public readonly static string STAMINA_FULLRECOVERY_TITLE = "スタミナ全回復";
    /// <summary>
    /// スタミナ全回復ダイアログ文言
    /// </summary>
    public readonly static string STAMINA_FULLRECOVERY_DESC = "スタミナが全回復されました。";


    /// <summary>
    /// スタミナが足らないタイトル
    /// </summary>
    public readonly static string STAMINA_LESS_TITLE = "スタミナが足りません!!";
    /// <summary>
    /// スタミナが足らない文言
    /// </summary>
    public readonly static string STAMINA_LESS_DESC = "動画視聴することで<br>スタミナが全回復されます。<br>視聴しますか？。";


    /// <summary>
    /// セーブデータ関連
    /// </summary>
    public readonly static string SAVEDATA_CLEAR_TITlE = "データクリア";
    public readonly static string SAVEDATA_CLEAR_DESC = "セーブデータを初期化します。<br>よろしいですか？";

    /// <summary>
    /// タイトル戻る
    /// </summary>
    public readonly static string OPTION_GOSTART_TITLE = "タイトルに戻る";
    public readonly static string OPTION_GOSTART_DESC = "現在のバトルを終了して<br>タイトルに戻ります。<br>よろしいでしょうか？";

    /// <summary>
    /// 中断データ
    /// </summary>
    public readonly static string LOADBATTLE_TITLE = "戦闘中！！";
    public readonly static string LOADBATTLE_DESC = "中断された戦闘データがあります。<br>再開しますか？";


}
