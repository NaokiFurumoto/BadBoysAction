using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : DropItem
{
    /// <summary>
    /// ƒvƒŒƒCƒ„[‚ÆÕ“Ë
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        var status = collision.gameObject.GetComponent<PlayerStatusController>();
        if(status != null) 
        {
            status.OnComplate = null;
            status.OnComplate += Destroy;
            status?.RecoveryLife();
        }
    }

    /// <summary>
    /// íœ
    /// </summary>
    public override void Destroy()
    {
        base.Destroy();
    }
}
