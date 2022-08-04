using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DROPITEM_TYPE
{
    NONE,
    LIFE,
}
public abstract class DropItem : MonoBehaviour
{
    /// <summary>
    /// ドロップアイテムタイプ
    /// </summary>
    [SerializeField]
    private DROPITEM_TYPE type;

    /// <summary>
    /// 単位
    /// </summary>
    [SerializeField]
    private int amount;

    public DROPITEM_TYPE Type => type;
    public int Amount => amount;

    public abstract void OnTriggerEnter2D(Collider2D collision);
    
    /// <summary>
    /// 削除
    /// </summary>
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

}
