using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
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
    private float moveSpeed;

    /// <summary>
    /// タップした位置
    /// </summary>
    private Vector2 tapPos;

    /// <summary>
    /// 向くべき方向
    /// </summary>
    [SerializeField]
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

    [SerializeField]
    private GameController gameController;

    private PlayerStatusController playerStatusController;

    #region プロパティ
    public Vector2 Direction => direction;
    #endregion

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// プレイヤーを移動させる
    /// </summary>
    private void Update()
    {
        if (gameController.State != INGAME_STATE.PLAYING)
            return;

        //死亡していたら移動させない
        if (playerStatusController.IsDead)
            return;

        ////画面タップされたら方向を向く
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
        mainCamera = Camera.main;
        inputManager = InputManager.Instance;
        direction = -Vector2.up;

        if (playerAnim == null)
        {
            playerAnim = GameObject.FindGameObjectWithTag("PlayerAnimator")
                                   .GetComponent<Animator>();
        }

        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController")
                                       .GetComponent<GameController>();
        }

        playerStatusController = GetComponent<PlayerStatusController>();
    }

    /// <summary>
    /// タップした場所に方向転換
    /// </summary>
    private void PlayerTurning()
    {
        tapPos = inputManager.TouchingPos;
        direction = new Vector2(tapPos.x - transform.position.x,
                                  tapPos.y - transform.position.y).normalized;

        var playerDirection = PlayerAnimation(direction.x, direction.y);
        ChangeAttacker(playerDirection.Item1, playerDirection.Item2);
    }

    /// <summary>
    /// 向きに合わせたアニメーションの切り替え
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private (float, float) PlayerAnimation(float x, float y)
    {
        //0.5の数を偶数に合わせる
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        tempScale = transform.localScale;
        tempScale.x = x > 0 ? Mathf.Abs(tempScale.x) : -Mathf.Abs(tempScale.x);
        transform.localScale = tempScale;

        //アニメーションのために初期化させる
        x = Mathf.Abs(x);
        playerAnim.SetFloat("FaceX", x);
        playerAnim.SetFloat("FaceY", y);
        return (x, y);
    }

    /// <summary>
    /// プレイヤーの移動
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CharacterMovement()
    {
        var pos = Vector2.Lerp(transform.position,
                               inputManager.TouchingPos,
                               moveSpeed * Time.deltaTime);

        var x = Mathf.Clamp(pos.x, PL_MINMOVE_X, PL_MAXMOVE_X);
        var y = Mathf.Clamp(pos.y, PL_MINMOVE_Y, PL_MAXMOVE_Y);

        transform.position = new Vector2(x, y);
    }
}
