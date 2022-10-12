using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///　敵が衝突した際にエフェクトを表示させる
/// </summary>

public class WallEffect : MonoBehaviour
{
    private enum EFFECT_TYPE
    {
        NONE,
        TOP,
        DOWN,
        LEFT,
        RIGHT,
    }

    /// <summary>
    /// 表示エフェクト
    /// </summary>
    [SerializeField]
    private GameObject effect;

    /// <summary>
    /// 表示エフェクト
    /// </summary>
    [SerializeField]
    private EFFECT_TYPE type;

    /// <summary>
    /// 差分
    /// </summary>
    [SerializeField]
    private float diff;

    /// <summary>
    /// 表示位置
    /// </summary>
    private Vector2 effectPosition;

    /// <summary>
    /// 初期設定
    /// </summary>
    private Vector2 SetEffectPosition(Vector2 pos)
    {
        var x = pos.x;
        var y = pos.y;

        switch (type)
        {
            case EFFECT_TYPE.TOP:
                y += diff;
                break;
            case EFFECT_TYPE.DOWN:
                y -= diff;
                break;
            case EFFECT_TYPE.LEFT:
                x -= diff;
                break;
            case EFFECT_TYPE.RIGHT:
                x += diff;
                break;
        }

        return new Vector2(x, y);
    }


    /// <summary>
    /// 敵と衝突
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (collisionObject.tag == "Enemy") 
        {
            var controller = collision.gameObject.GetComponent<EnemyStatusController>();
            if (controller == null || !controller.IsDamage)
                return;

            //敵がダメージ中の対応
            {
                var pos = SetEffectPosition(collisionObject.transform.position);
                Instantiate(effect,pos,effect.transform.rotation);
            }
        }
    }


}
