using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// キャラクターの攻撃に関するクラス
/// </summary>

//攻撃方向
public enum ATTACK_DIRECTION
{
    NONE,
    FRONT,      //正面
    UP,         //上
    SIDE,       //横
    SIDEUP,     //斜め上
    SIDEDOWN    //斜め下
}

public class Attacker : MonoBehaviour
{
    /// <summary>
    /// センサー
    /// </summary>
    [SerializeField]
    private CircleCollider2D collider;

    /// <summary>
    /// Sprite
    /// </summary>
    [SerializeField]
    private GameObject sprite;

    /// <summary>
    /// アニメーション
    /// </summary>
    private Animator animator;

    /// <summary>
    /// 攻撃の方向
    /// </summary>
    [SerializeField]
    private ATTACK_DIRECTION attackType;

    /// <summary>
    /// 攻撃方向の取得
    /// </summary>
    public ATTACK_DIRECTION AttackType => attackType;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        animator = sprite?.GetComponent<Animator>();
    }

    /// <summary>
    /// センサーに1度衝突した
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //TODO:一度攻撃してると実行しない
            sprite.SetActive(true);
            //吹き飛ばす：秒後でもいい？
        }
    }

    /// <summary>
    /// センサーから抜けた
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
       
        if (collision.CompareTag("Enemy"))
        {
            //攻撃したキャラが抜けたら
            //TODO:アニメーションの切替に変更した方が軽い
            //待機中のアニメーションを登録する
            sprite.SetActive(false);
            //吹き飛ばす：秒後でもいい？
        }
    }

    //吹き飛ばす処理

    
}
