using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// プレーヤーのステータス管理
/// </summary>
public class PlayerStatusController : MonoBehaviour
{
    //■変数
    //最大体力
    //現在の体力
    //初期ライフ
    //吹き飛ばす力
    //無敵状態
    //生存判定

    /// <summary>
    /// 最大ライフ
    /// TODO:ゲームマネージャに持たせる？
    /// </summary>
    [SerializeField]
    private int maxLife;

    /// <summary>
    /// 現在のライフ
    /// </summary>
    [SerializeField]
    private int life;

    /// <summary>
    /// 初期ライフ
    /// TODO:ゲームマネージャに持たせる？
    /// </summary>
    [SerializeField]
    private int startLife = 1;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        //初回ならば
        life = startLife;
    }




    //■関数
    //ライフを減らす
    //ライフを増やす
    //死亡判定
    //向いてる方向？
}
 
