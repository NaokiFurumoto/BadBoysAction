using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region �ϐ�
    /// <summary>
    /// ���C���J����
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// �^�b�`�̔���
    /// </summary>
    private bool touchFlag;

    /// <summary>
    /// �^�b�`�ʒu
    /// </summary>
    [SerializeField]
    private Vector2 touchBeginPos, touchingPos, touchLastPos;

    /// <summary>
    /// �^�b�`���
    /// </summary>
    private TouchPhase touchPhase;
    #endregion

    #region �v���p�e�B
    public bool TouchFlag           => touchFlag;
    public Vector2 TouchBeginPos    => touchBeginPos;
    public Vector2 TouchingPos      => touchingPos;
    public Vector2 TouchingLastPos  => touchLastPos;
    public TouchPhase TouchPhase    => touchPhase;
    #endregion

    public static InputManager Instance;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        mainCamera      = Camera.main;
        Instance        = this;
        touchFlag       = false;
        touchBeginPos   = touchingPos
                        = touchLastPos 
                        = Vector2.zero;
        touchPhase      = TouchPhase.Began;
    }

    private void Update()
    {
        //�Q�[���v���C���Ɏ��s������
        //Editor
        if (Application.isEditor)
        {
            //�������u��
            if (Input.GetMouseButtonDown(0))
            {
                touchFlag       = true;
                touchPhase      = TouchPhase.Began;
                touchBeginPos   = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            //�������u��
            if (Input.GetMouseButtonUp(0))
            {
                touchFlag       = false;
                touchPhase      = TouchPhase.Ended;
                touchLastPos    = touchingPos;
                touchBeginPos   = touchingPos
                                = Vector2.zero;
            }

            //�������ςȂ�
            if (Input.GetMouseButton(0))
            {
                touchPhase    = TouchPhase.Moved;
                touchingPos   = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else//�[��
        {
            //TODO:�ǉ��ŕK�v����
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchingPos = touch.position;
                touchPhase  = touch.phase;
                touchFlag   = true;
            }
        }
    }
}
