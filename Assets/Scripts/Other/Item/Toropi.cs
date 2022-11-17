using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toropi : DropItem
{
    /// <summary>
    /// �v���C���[�ƏՓ�
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        var status = collision.gameObject.GetComponent<PlayerStatusController>();
        if (status != null)
        {
            status.OnComplate = null;
            status.OnComplate += Destroy;
            status?.MutekiAttack();
        }
    }

    /// <summary>
    /// �폜
    /// </summary>
    public override void Destroy()
    {
        base.Destroy();
    }
}
