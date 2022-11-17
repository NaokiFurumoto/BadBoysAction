using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;

public enum DROPITEM_TYPE
{
    NONE,
    LIFE,
    MUTEKI
}
public abstract class DropItem : MonoBehaviour
{
    /// <summary>
    /// �h���b�v�A�C�e���^�C�v
    /// </summary>
    [SerializeField]
    private DROPITEM_TYPE type;

    /// <summary>
    /// �P��
    /// </summary>
    [SerializeField]
    private int amount;

    public DROPITEM_TYPE Type => type;
    public int Amount => amount;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(DROPITEM_DELETE_TIMES);
        Destroy();
        ItemController.Instance.RemoveNullItem();
    }

    public abstract void OnTriggerEnter2D(Collider2D collision);
    
    /// <summary>
    /// �폜
    /// </summary>
    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

}
