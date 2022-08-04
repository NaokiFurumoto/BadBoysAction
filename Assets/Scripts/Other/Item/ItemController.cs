using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static GlobalValue;
/// <summary>
/// �h���b�v�A�C�e���Ɋւ��鐧��
/// </summary>
public class ItemController : MonoBehaviour
{
    /// <summary>
    /// �h���b�v�A�C�e�����X�g
    /// </summary>
    [SerializeField]
    private List<DropItem> dropItems = new List<DropItem>();

    [SerializeField]
    private Transform dropRoot;

    [SerializeField]
    private Vector3 dropItemPos;

    /// <summary>
    /// �h���b�v�A�C�e���^�C�v
    /// </summary>
    private DROPITEM_TYPE dropItemType;

    /// <summary>
    /// �I�����ꂽ�h���b�v�A�C�e��
    /// </summary>
    private DropItem selectDropItem;

    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        dropItemType = DROPITEM_TYPE.NONE;
    }

    /// <summary>
    /// �h���b�v�A�C�e���̐ݒ�
    /// </summary>
    /// <param name="_itemType"></param>
    public void SetDropItem(DROPITEM_TYPE _itemType)
    {
        foreach(var item in dropItems)
        {
            if(item.Type == _itemType)
            {
                selectDropItem = item;
                return;
            }
        }
    }

    /// <summary>
    /// �h���b�v�A�C�e���̔z�u�ʒu�̐ݒ�
    /// </summary>
    private void SetDropPosition()
    {
        var x = Random.Range(DROPITEM_POSX_MIN, DROPITEM_POSX_MAX);
        var y = Random.Range(DROPITEM_POSY_MIN, DROPITEM_POSY_MAX);

        dropItemPos = new Vector3(x, y, 0);
    }

    /// <summary>
    /// �h���b�v�A�C�e���z�u
    /// </summary>
    public void CreateDropItem()
    {
        if (selectDropItem == null)
            return;

        SetDropPosition();

        var obj = Instantiate(selectDropItem,dropItemPos,Quaternion.identity);
        obj.transform.SetParent(dropRoot);
    }
}
