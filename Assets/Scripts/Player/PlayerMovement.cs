using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// キャラクターの移動に関するクラス
/// </summary>
public partial class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// タッチクラス
    /// </summary>
    private InputManager inputManager;

    [SerializeField]
    private float moveSpeed = 2.0f; 

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
    private void Update()
    {
        //タップ入力位置を取得
        //ドラッグ中の位置を取得
        //現在の一から同期

        //画面タップされたら方向を向く
        if (!inputManager.TouchFlag)    
            return;

        PlayerTurning();
        CharacterMovement();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        mainCamera     = Camera.main;
        inputManager   = InputManager.Instance;

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
        tapPos      = inputManager.TouchingPos; 
        direction   = new Vector2(tapPos.x - transform.position.x,
                                  tapPos.y - transform.position.y).normalized;

        var playerDirection = PlayerAnimation(direction.x, direction.y);
        ChangeAttacker(playerDirection.Item1, playerDirection.Item2);
    }

    /// <summary>
    /// 向きに合わせたアニメーションの切り替え
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private (float,float) PlayerAnimation(float x, float y)
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
        return (x,y);
    }

    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CharacterMovement()
    {
        transform.position = Vector2.Lerp(transform.position,
                                          inputManager.TouchingPos,
                                          moveSpeed * Time.deltaTime);
    }



}
