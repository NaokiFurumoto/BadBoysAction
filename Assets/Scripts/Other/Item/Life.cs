using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : DropItem
{
    /// <summary>
    /// ÉvÉåÉCÉÑÅ[Ç∆è’ìÀ
    /// </summary>
    /// <param name="collision"></param>
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        var status = collision.gameObject.GetComponent<PlayerStatusController>();
        if(status != null) 
        {
            SoundManager.Instance.PlayOneShot(AppSound.Instance.SE_ITEM_LIFE);
            status.OnComplate = null;
            status.OnComplate += Destroy;
            status?.RecoveryLife();
        }
    }

    /// <summary>
    /// çÌèú
    /// </summary>
    public override void Destroy()
    {
        base.Destroy();
    }
}
