using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// �L�����N�^�[�̈ړ��Ɋւ���N���X
/// </summary>
public partial class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// �^�b�`�N���X
    /// </summary>
    private InputManager inputManager;

    [SerializeField]
    private float moveSpeed;

    /// <summary>
    /// �^�b�v�����ʒu
    /// </summary>
    private Vector2 tapPos;

    /// <summary>
    /// �����ׂ�����
    /// </summary>
    [SerializeField]
    private Vector2 direction;

    /// <summary>
    /// �v���[���[�̌����̈ꎞ�I�ȑޔ�
    /// </summary>
    private Vector3 tempScale;

    /// <summary>
    /// �A�j���[�V����
    /// </summary>
    [SerializeField]
    private Animator playerAnim;

    /// <summary>
    /// �J�����̎擾
    /// </summary>
    private Camera mainCamera;

    [SerializeField]
    private GameController gameController;

    private PlayerStatusController playerStatusController;

    #region �v���p�e�B
    public Vector2 Direction => direction;
    #endregion

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// �v���C���[���ړ�������
    /// </summary>
    private void Update()
    {
        if (gameController.State != INGAME_STATE.PLAYING)
            return;

        //���S���Ă�����ړ������Ȃ�
        if (playerStatusController.IsDead)
            return;

        ////��ʃ^�b�v���ꂽ�����������
        if (!inputManager.TouchFlag)
            return;

        PlayerTurning();
        CharacterMovement();
    }

    /// <summary>
    /// ������
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
    /// �^�b�v�����ꏊ�ɕ����]��
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
    /// �����ɍ��킹���A�j���[�V�����̐؂�ւ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private (float, float) PlayerAnimation(float x, float y)
    {
        //0.5�̐��������ɍ��킹��
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        tempScale = transform.localScale;
        tempScale.x = x > 0 ? Mathf.Abs(tempScale.x) : -Mathf.Abs(tempScale.x);
        transform.localScale = tempScale;

        //�A�j���[�V�����̂��߂ɏ�����������
        x = Mathf.Abs(x);
        playerAnim.SetFloat("FaceX", x);
        playerAnim.SetFloat("FaceY", y);
        return (x, y);
    }

    /// <summary>
    /// �v���C���[�̈ړ�
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
