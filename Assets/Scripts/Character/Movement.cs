using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �L�����̈ړ��x�[�X�N���X
/// TODO:�s�v�I�I�G�L�����ɕύX
/// </summary>
public class Movement : MonoBehaviour
{
    #region �ϐ�
    /// <summary>
    /// �G�̈ړ��X�s�[�h
    /// </summary>
    [SerializeField]
    protected float xSpeed = 1.5f, ySpeed = 1.5f;

    /// <summary>
    /// �ړ���
    /// </summary>
    private Vector2 moveDelta;
    #endregion

    #region �v���p�e�B
    public Vector2 MoveDelta => moveDelta;
    #endregion

    /// <summary>
    /// �L�����N�^�[���ړ�������
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    protected virtual void CharacterMovement(float x, float y)
    {
        moveDelta = new Vector2(x * xSpeed, y * ySpeed);
        transform.Translate(moveDelta.x * Time.deltaTime,
                            moveDelta.y * Time.deltaTime, 0);
    }

}
