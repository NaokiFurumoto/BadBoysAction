using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///�@�G���Փ˂����ۂɃG�t�F�N�g��\��������
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
    /// �\���G�t�F�N�g
    /// </summary>
    [SerializeField]
    private GameObject effect;

    /// <summary>
    /// �\���G�t�F�N�g
    /// </summary>
    [SerializeField]
    private EFFECT_TYPE type;

    /// <summary>
    /// ����
    /// </summary>
    [SerializeField]
    private float diff;

    /// <summary>
    /// �\���ʒu
    /// </summary>
    private Vector2 effectPosition;

    /// <summary>
    /// �����ݒ�
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
    /// �G�ƏՓ�
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

            //�G���_���[�W���̑Ή�
            {
                var pos = SetEffectPosition(collisionObject.transform.position);
                Instantiate(effect,pos,effect.transform.rotation);
            }
        }
    }


}
