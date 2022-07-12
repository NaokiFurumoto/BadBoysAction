using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �L�����N�^�[�̈ړ��Ɋւ���N���X
/// </summary>
public class PlayerMovement : Movement
{
    //���L�������w�ɒǐ�������

    /// <summary>
    /// �^�b�v�����ʒu
    /// </summary>
    private Vector2 tapPos;

    /// <summary>
    /// �����ׂ�����
    /// </summary>
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

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// �v���C���[���ړ�������
    /// </summary>
    private void FixedUpdate()
    {
        //�^�b�v���͈ʒu���擾
        //�h���b�O���̈ʒu���擾
        //���݂̈ꂩ�瓯��

        //��ʂ��^�b�v
        if( Input.GetMouseButton(0))
        {
            //������ς���
            //var pointX = Input.mousePosition.x;
            //var pointY = Input.mousePosition.y;
        }
       

    }

    /// <summary>
    /// ������
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
    /// �^�b�v�����ꏊ�ɕ����]��
    /// </summary>
    private void PlayerTurning()
    {
        tapPos      = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction   = new Vector2(tapPos.x - transform.position.x,
                                tapPos.y - transform.position.y).normalized;

        PlayerAnimation(direction.x, direction.y);
    }

    /// <summary>
    /// �����ɍ��킹���A�j���[�V�����̐؂�ւ�
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void PlayerAnimation(float x, float y)
    {
        //0.5�̐��������ɍ��킹��
        x = Mathf.RoundToInt(x);
        y = Mathf.RoundToInt(y);

        tempScale   = transform.localScale;
        tempScale.x = x > 0 ? Mathf.Abs(tempScale.x) : -Mathf.Abs(tempScale.x);
        transform.localScale = tempScale;

        //�A�j���[�V�����̂��߂ɏ�����������
        x = Mathf.Abs(x);
        playerAnim.SetFloat("FaceX", x);
        playerAnim.SetFloat("FaceY", y);
    }

    

}
