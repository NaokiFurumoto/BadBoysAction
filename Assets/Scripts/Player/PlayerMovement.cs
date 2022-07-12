using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// キャラクターの移動に関するクラス
/// </summary>
public class PlayerMovement : Movement
{
    //■キャラを指に追随させる

    /// <summary>
    /// タップした位置
    /// </summary>
    private Vector2 tapPos;

    /// <summary>
    /// 向くべき方向
    /// </summary>
    private Vector2 direction;

    /// <summary>
    /// プレーヤーの向きの一時的な退避
    /// </summary>
    private Vector3 tempScale;

    /// <summary>
    /// アニメーション
    /// </summary>
    [SerializeField]
    private Animator playerAnim;

    /// <summary>
    /// カメラの取得
    /// </summary>
    private Camera mainCamera;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// プレイヤーを移動させる
    /// </summary>
    private void FixedUpdate()
    {
        //タップ入力位置を取得
        //ドラッグ中の位置を取得
        //現在の一から同期

        //画面をタップ
        if( Input.GetMouseButton(0))
        {
            //向きを変える
            //var pointX = Input.mousePosition.x;
            //var pointY = Input.mousePosition.y;
        }
       

    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        mainCamera = Camera.main;

        if(playerAnim == null)
        {
            playerAnim = GameObject.FindGameObjectWithTag("PlayerAnimator")
                                   .GetComponent<Animator>();
        }
    }

    /// <summary>
    /// タップした場所に方向転換
    /// </summary>
    private void PlayerTurning()
    {
        tapPos      = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction   = new Vector2(tapPos.x - transform.position.x,
                                tapPos.y - transform.position.y).normalized;

        PlayerAnimation(direction.x, direction.y);
    }

    /// <summary>
    /// 向きに合わせたアニメーションの切り替え
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void PlayerAnimation(float x, float y)
    {
        //0.5の数を偶数に合わせる
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        tempScale   = transform.localScale;
        tempScale.x = x > 0 ? Mathf.Abs(tempScale.x) : -Mathf.Abs(tempScale.x);
        transform.localScale = tempScale;

        //アニメーションのために初期化させる
        x = Mathf.Abs(x);
        playerAnim.SetFloat("FaceX", x);
        playerAnim.SetFloat("FaceY", y);
    }

    

}
