using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// キャラの移動ベースクラス
/// TODO:不要！！敵キャラに変更
/// </summary>
public class Movement : MonoBehaviour
{
    #region 変数
    /// <summary>
    /// 敵の移動スピード
    /// </summary>
    [SerializeField]
    protected float xSpeed = 1.5f, ySpeed = 1.5f;

    /// <summary>
    /// 移動量
    /// </summary>
    private Vector2 moveDelta;
    #endregion

    #region プロパティ
    public Vector2 MoveDelta => moveDelta;
    #endregion

    /// <summary>
    /// キャラクターを移動させる
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
